import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import Validation from '../utils/validation';
import { ToastrService } from 'ngx-toastr';
import { RegisterModel } from '../models/register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm : FormGroup;
  loading = false;
  submitted = false;
  emailIsAlreadyUsed = false;
  toggledPassword = false;
  toggledConfirmationPassword = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private toastrService: ToastrService) {
    this.registerForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(12), Validators.maxLength(255)]],
      confirmPassword: ['', [Validators.required]]
      },
      {
        validators: Validation.match('password', 'confirmPassword')
      }
    );
  }

  ngOnInit(): void {
  }

  register(): void {
    this.submitted = true;
    this.emailIsAlreadyUsed = false;
    if (this.registerForm.invalid) {
      return;
    }
    this.loading = true;

    var registerFormValue: RegisterModel = { ...this.registerForm.value };

    this.authService.register(registerFormValue).subscribe(
      data => {
        this.toastrService.success('Registration successful');
        this.router.navigate(['/registration-successful']);
      },
      error => {
        this.loading = false;
        if (error.error.message == 'Email is already used by another user.')
          this.emailIsAlreadyUsed = true;
      }
    );
  }
  
  get f(): { [key: string]: AbstractControl } {
    return this.registerForm.controls;
  }

  get emailAddress() {
    return this.registerForm.get('emailAddress');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  togglePassword(): void {
    this.toggledPassword = !this.toggledPassword;
  }

  toggleConfirmationPassword(): void {
    this.toggledConfirmationPassword = !this.toggledConfirmationPassword;
  }

}
