import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { ModalTfaLoginComponent } from '../modal-tfa-login/modal-tfa-login.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginModel } from '../models/login.model';

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

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router,
     private toastrService: ToastrService, private modalService: NgbModal) {
    this.loginForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(12), Validators.maxLength(255), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{12,}$/)]],
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

    let loginFormValue: LoginModel = { ...this.loginForm.value };

    this.authService.login(loginFormValue).subscribe(
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
              this.authService.setTokenExpiration(data.expirationDateTime);
              this.authService.setCipherKeyToSHA256Value(this.password.value);
              this.authService.refreshLogoutTimer();
              this.toastrService.success('Login successful');
              this.router.navigate(['/passwords']);
            }
            else {
              this.toastrService.error("Two factor verification failed");
            }
          });
        }
        else {
          this.authService.setTokenExpiration(data.expirationDateTime);
          this.authService.setCipherKeyToSHA256Value(this.password.value);
          this.authService.refreshLogoutTimer();
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
