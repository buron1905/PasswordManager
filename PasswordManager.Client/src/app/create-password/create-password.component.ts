import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PasswordService } from '../services/password.service';

@Component({
  selector: 'app-create-password',
  templateUrl: './create-password.component.html',
  styleUrls: ['./create-password.component.css']
})
export class CreatePasswordComponent implements OnInit {
  passwordForm: FormGroup;

  constructor(private fb: FormBuilder, private passwordService: PasswordService) {
    this.passwordForm = this.fb.group({
      PasswordName: ['', [Validators.required]],
      Username: ['', [Validators.required]],
      Password: ['', [Validators.required]],
      Description: ['']
    });
  }

  ngOnInit(): void {
  }

  create() : void {
    console.log(this.passwordForm.value);
    this.passwordService.create(this.passwordForm.value).subscribe(
      data => {
        console.log("Saving...");
        console.log("Data: (returned from server)");
        console.log(data);
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.passwordForm.controls;
  }

  get PasswordName() {
    return this.passwordForm.get('PasswordName');
  }
  
  get UserName() {
    return this.passwordForm.get('UserName');
  }

  get Password() {
    return this.passwordForm.get('Password');
  }

  get Description() {
    return this.passwordForm.get('Description');
  }

}
