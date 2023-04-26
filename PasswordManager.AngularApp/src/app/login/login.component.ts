import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { ModalTfaLoginComponent } from '../modal-tfa-login/modal-tfa-login.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  emailConfirmed = true;
  wrongCredentials = false;
  toggledPassword = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private toastrService: ToastrService, private modalService: NgbModal) {
    this.loginForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  login(): void {
    this.submitted = true;
    this.wrongCredentials = false;
    this.emailConfirmed = true;
    if (this.loginForm.invalid) {
      return;
    }
    this.loading = true;

    this.authService.authenticate(this.loginForm.value).subscribe(
      data => {
        this.loading = false;

        this.emailConfirmed = data.emailVerified;

        if (!this.emailConfirmed) {
          return;
        }

        if (data.isTfaEnabled) {
          const modalTfaLogin = this.modalService.open(ModalTfaLoginComponent);
          modalTfaLogin.componentInstance.emailAddressInput = this.emailAddress.value;
          modalTfaLogin.componentInstance.passwordInput = this.password.value;
          modalTfaLogin.result.then((result) => {
            if (result == true) {
              this.authService.login(data);
              this.toastrService.success('Login successful');
              this.router.navigate(['/passwords']);
            }
            else {
              this.toastrService.error("Two factor verification failed");
            }
          });
        }
        else {
          this.authService.login(data);
          this.toastrService.success('Login successful');
          this.router.navigate(['/passwords']);
        }
      },
      error => {
        this.wrongCredentials = true;
        this.loading = false;
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.loginForm.controls;
  }

  get emailAddress() {
    return this.loginForm.get('emailAddress');
  }

  get password() {
    return this.loginForm.get('password');
  }

  togglePassword(): void {
    this.toggledPassword = !this.toggledPassword;
  }

}
