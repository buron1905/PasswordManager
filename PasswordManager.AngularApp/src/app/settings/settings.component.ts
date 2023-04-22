import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  tfaForm: FormGroup;
  loading = false;
  submitted = false;
  wrongCode = false;
  tfaEnabled = false;
  authKey = '';
  qrCodeSetupImageUrl = '';


  constructor(private fb: FormBuilder, private authService: AuthService, private toastrService: ToastrService) {
    this.tfaForm = this.fb.group({
      code: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.getTfaSetup();
  }

  getTfaSetup() {
    this.loading = true;
    this.authService.getTfaSetup().subscribe(
      data => {
        this.tfaEnabled = data.isTfaEnabled;
        this.authKey = data.authenticatorKey;
        this.qrCodeSetupImageUrl = data.qrCodeSetupImageUrl;
        this.loading = false;
      },
      error => {
        this.loading = false;
      });
  }

  enableTfa(): void {
    this.submitted = true;
    this.wrongCode = false;

    if (this.tfaForm.invalid) {
      return;
    }

    this.loading = true;

    this.authService.enableTfa(this.tfaForm.value).subscribe(
      data => {
        this.submitted = false;
        this.tfaEnabled = data.isTfaEnabled;
        this.authKey = data.authenticatorKey;
        this.qrCodeSetupImageUrl = data.qrCodeSetupImageUrl;
        this.tfaForm.patchValue({ code: '' });
        this.loading = false;
        this.toastrService.success('Enabled');
      },
      error => {

        this.wrongCode = true;
        this.loading = false;
      }
    );
  }

  disableTfa(): void {
    this.submitted = true;
    this.wrongCode = false;
    if (this.tfaForm.invalid) {
      return;
    }

    this.loading = true;

    this.authService.disableTfa(this.tfaForm.value).subscribe(
      data => {
        this.submitted = false;
        this.tfaEnabled = data.isTfaEnabled;
        this.authKey = data.authenticatorKey;
        this.qrCodeSetupImageUrl = data.qrCodeSetupImageUrl;
        this.tfaForm.patchValue({ code: '' });
        this.loading = false;
        this.toastrService.success('Disabled');
      },
      error => {
        this.wrongCode = true;
        this.loading = false;
      }
    );
  }

  verify(): void {
    this.submitted = true;
    this.wrongCode = false;
    if (this.tfaForm.invalid) {
      return;
    }
    this.loading = true;

    this.authService.loginTfa(this.tfaForm.value).subscribe(
      data => {
        if (data.isAuthSuccessful) {
          this.tfaForm.patchValue({ code: '' });
          this.loading = false;
        }
        else {
          this.wrongCode = true;
        }
      },
      error => {
        this.wrongCode = true;
        this.loading = false;
      }
    );
  }

  deleteAccount(): void {

  }

  get f(): { [key: string]: AbstractControl } {
    return this.tfaForm.controls;
  }

  get code() {
    return this.tfaForm.get('code');
  }

}
