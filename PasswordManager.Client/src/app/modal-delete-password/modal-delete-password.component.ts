import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal-delete-password',
  templateUrl: './modal-delete-password.component.html',
  styleUrls: ['./modal-delete-password.component.css']
})
export class ModalDeletePasswordComponent implements OnInit {
  @Input() public passwordsGuids: Array<string>;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  sendResponse(response: boolean) {
    this.activeModal.close(response);
  }

}
