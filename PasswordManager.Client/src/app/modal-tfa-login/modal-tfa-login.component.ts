import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-modal-tfa-login',
  templateUrl: './modal-tfa-login.component.html',
  styleUrls: ['./modal-tfa-login.component.css']
})
export class ModalTfaLoginComponent implements OnInit {
  @Input() public token: string;

  tfaForm: FormGroup;
  loading = false;
  submitted = false;
  wrongCode = false;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private authService: AuthService) {
    this.tfaForm = this.fb.group({
      token: ['', [Validators.required]],
      code: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    if (this.token) {
      this.tfaForm.patchValue({ token: this.token });
    }
    else {
      console.log("Error receiving token in component.")
    }
  }

  sendResponse(response: boolean): void {
    this.activeModal.close(response);
  }
  
  get f(): { [key: string]: AbstractControl } {
    return this.tfaForm.controls;
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

    this.authService.loginTfa(this.tfaForm.value).subscribe(
      data => {
        if (data.isAuthSuccessful) {
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
