import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClipboardService } from 'ngx-clipboard';
import { ToastrService } from 'ngx-toastr';
import { toArray } from 'rxjs';
import { PasswordGeneratorService } from '../services/password-generator.service';

@Component({
  selector: 'app-password-generator',
  templateUrl: './password-generator.component.html',
  styleUrls: ['./password-generator.component.css']
})
export class PasswordGeneratorComponent implements OnInit {
  @Output() newPasswordEvent = new EventEmitter<string>();

  generatorForm: FormGroup;
  submitted = false;
  loading = false;

  generatedPassword: string = "";

  constructor(private fb: FormBuilder, private toastrService: ToastrService, private passwordGeneratorService: PasswordGeneratorService, private clipboardService: ClipboardService) {
    this.generatorForm = this.fb.group({
      passwordLength: [12, [Validators.required, Validators.min(5), Validators.max(256)]],
      useNumbers: [true, [Validators.nullValidator]],
      useSpecialChars: [true, [Validators.nullValidator]],
      useUppercase: [true, [Validators.nullValidator]],
      useLowercase: [true, [Validators.nullValidator]],
    });
  }

  ngOnInit(): void {
    this.onChanges();
    this.generate();
  }

  generate(): void {
    this.submitted = true;
    if (this.generatorForm.invalid) {
      return;
    }
    //this.loading = true;

    this.generatedPassword = this.passwordGeneratorService.generatePasswordFromModel(this.generatorForm.value);
    this.newPasswordEvent.emit(this.generatedPassword);

    //this.passwordGeneratorService.generatePassword(this.generatorForm.value).subscribe(
    //  data => {
    //    this.generatedPassword = data.password;
    //    this.loading = false;
    //  },
    //  error => {
    //    this.toastrService.error(`Error when generating new password occured.\nError:${error}`);
    //    this.loading = false;
    //  }
    //);
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

  copyToClipboard(text: string): void {
    this.clipboardService.copyFromContent(text);
    this.toastrService.success('Copied to clipboard');
  }

  onChanges(): void {
    this.generatorForm.valueChanges.subscribe(() => {
      this.generate();
    });
  }

}
