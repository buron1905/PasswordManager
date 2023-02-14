import { Component, OnInit } from '@angular/core';
import { PasswordService } from '../services/password.service';
import { PasswordModel } from '../models/password.model';

@Component({
  selector: 'app-passwords',
  templateUrl: './passwords.component.html',
  styleUrls: ['./passwords.component.css']
})
export class PasswordsComponent implements OnInit {
  passwords: PasswordModel[];
  loading = false;

  constructor(private passwordService: PasswordService) {
  }

  ngOnInit(): void {
    this.loading = true;
    this.getAllPasswords();
  }

  private getAllPasswords() {
    this.passwordService.get().subscribe(
      data => {
        this.passwords = data;
        this.loading = false;
      },
      error => {
        this.loading = false;
      });
  }

  selectAll(): void {
    (document.querySelectorAll('.rowCheckbox') as NodeListOf<HTMLInputElement>).forEach((checkbox) => {
      checkbox.checked = true;
    });
  }

  unselectAll(): void {
    (document.querySelectorAll('.rowCheckbox') as NodeListOf<HTMLInputElement>).forEach((checkbox) => {
      checkbox.checked = false;
    });
  }

}
