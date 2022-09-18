import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  wrongCredentials = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  login() : void {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.authService.login(this.loginForm.value).subscribe(
      data => {
        console.log("Saving...");
        console.log("Data: (returned from server)");
        console.log(data);
        this.authService.saveToken(data.token);
        this.router.navigate(['/passwords']);
      }
    );
    console.log(this.loginForm.value);
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
}
