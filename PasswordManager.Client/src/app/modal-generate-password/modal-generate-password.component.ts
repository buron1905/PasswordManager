import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal-generate-password',
  templateUrl: './modal-generate-password.component.html',
  styleUrls: ['./modal-generate-password.component.css']
})
export class ModalGeneratePasswordComponent implements OnInit {

  generatedPassword: string;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  sendResponse(response: string): void {
    this.activeModal.close(response);
  }

  setGeneratedPassword(newPassword: string): void {
    this.generatedPassword = newPassword;
  }

}
