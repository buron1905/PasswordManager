import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { toArray } from 'rxjs';
import { PasswordGeneratorService } from '../services/password-generator.service';

@Component({
  selector: 'app-password-generator',
  templateUrl: './password-generator.component.html',
  styleUrls: ['./password-generator.component.css']
})
export class PasswordGeneratorComponent implements OnInit {
  generatorForm: FormGroup;
  submitted = false;
  loading = false;

  generatedPassword: string;

  constructor(private fb: FormBuilder, private toastrService: ToastrService, private passwordGeneratorService: PasswordGeneratorService) {
    this.generatorForm = this.fb.group({
      passwordLength: [12, [Validators.required, Validators.min(5), Validators.max(256)]],
      useNumbers: [true, [Validators.nullValidator]],
      useSpecialChars: [true, [Validators.nullValidator]],
      useUppercase: [true, [Validators.nullValidator]],
      useLowercase: [true, [Validators.nullValidator]],
    });
  }

  ngOnInit(): void {
  }

  generate(): void {
    this.submitted = true;
    if (this.generatorForm.invalid) {
      return;
    }
    this.loading = true;

    this.passwordGeneratorService.generatePassword(this.generatorForm.value).subscribe(
      data => {
        this.generatedPassword = data.password;
        this.loading = false;
      },
      error => {
        this.toastrService.error(`Error when generating new password occured.\nError:${error}`);
        this.loading = false;
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.generatorForm.controls;
  }

  get passwordLength() {
    return this.generatorForm.get('passwordLength');
  }

  get useNumbers() {
    return this.generatorForm.get('useNumbers');
  }

  get useSpecialChars() {
    return this.generatorForm.get('useSpecialChars');
  }

  get useUppercase() {
    return this.generatorForm.get('useUppercase');
  }

  get useLowercase() {
    return this.generatorForm.get('useLowercase');
  }

}
