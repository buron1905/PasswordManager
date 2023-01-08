import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PasswordService } from '../services/password.service';


@Component({
  selector: 'app-create-password',
  templateUrl: './create-password.component.html',
  styleUrls: ['./create-password.component.css']
})
export class CreatePasswordComponent implements OnInit {
  passwordForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private passwordService: PasswordService, private router: Router, private toastrService: ToastrService) {
    this.passwordForm = this.fb.group({
      passwordName: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      passwordDecrypted: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(255)]],
      description: ['', [Validators.maxLength(255)]]
    });
  }

  ngOnInit(): void {
  }

  create(): void {
    this.submitted = true;
    if (this.passwordForm.invalid) {
      return;
    }

    console.log(this.passwordForm.value);

    this.passwordService.create(this.passwordForm.value).subscribe(
      data => {
        this.toastrService.success('Saved');
        // open detail of password. Detail can be opened will be opened without calling of API, just by passing data
        this.router.navigate(['/passwords/details']);
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.passwordForm.controls;
  }

  get passwordName() {
    return this.passwordForm.get('passwordName');
  }
  
  get userName() {
    return this.passwordForm.get('userName');
  }

  get passwordDecrypted() {
    return this.passwordForm.get('passwordDecrypted');
  }

  get description() {
    return this.passwordForm.get('description');
  }

}
