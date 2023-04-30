import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginTfaModel } from '../models/login-tfa.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-modal-tfa-login',
  templateUrl: './modal-tfa-login.component.html',
  styleUrls: ['./modal-tfa-login.component.css']
})
export class ModalTfaLoginComponent implements OnInit {
  @Input() public emailAddressInput: string;
  @Input() public passwordInput: string;

  tfaForm: FormGroup;
  loading = false;
  submitted = false;
  wrongCode = false;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private authService: AuthService) {
    this.tfaForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      code: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    if (this.emailAddressInput && this.passwordInput) {
      this.tfaForm.patchValue({ emailAddress: this.emailAddressInput });
      this.tfaForm.patchValue({ password: this.passwordInput });
    }
    else {
      console.log("Error receiving email and password in component.")
    }
  }

  sendResponse(response: boolean): void {
    this.activeModal.close(response);
  }
  
  get f(): { [key: string]: AbstractControl } {
    return this.tfaForm.controls;
  }

  get emailAddress() {
    return this.tfaForm.get('emailAddress');
  }

  get password() {
    return this.tfaForm.get('password');
  }

  get code() {
    return this.tfaForm.get('code');
  }

  verify(): void {
    this.submitted = true;
    this.wrongCode = false;
    if (this.tfaForm.invalid) {
      return;
    }
    this.loading = true;

    var tfaFormValue: LoginTfaModel = { ...this.tfaForm.value };

    this.authService.loginTfa(tfaFormValue).subscribe(
      data => {
        if (data.isAuthSuccessful) {
          this.authService.setTokenExpiration(data.expirationDateTime);
          this.authService.setCipherKey(this.password.value);
          this.authService.refreshLogoutTimer();
          this.sendResponse(true);
          this.loading = false;
        }
        else {
          this.wrongCode = true;
          this.loading = false;
        }
      },
      error => {
        this.wrongCode = true;
        this.loading = false;
      }
    );
  }

}
