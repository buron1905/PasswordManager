import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import Validation from '../utils/validation';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm : FormGroup;
  submitted = false;
  emailIsAlreadyUsed = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private toastrService: ToastrService) {
    this.registerForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(255)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
      acceptTerms: [false, Validators.requiredTrue],
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
    if (this.registerForm.invalid) {
      return;
    }

    this.authService.register(this.registerForm.value).subscribe(
      data => {
        console.log(data);
        this.authService.login(data);
        this.toastrService.success('Registration successful');
        this.router.navigate(['/passwords']);

        // if(!data.success) {
        //   this.emailIsAlreadyUsed = true;
        // }
      }
    );
    console.log(this.registerForm.value);
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

  get acceptTerms() {
    return this.registerForm.get('acceptTerms');
  }
}
