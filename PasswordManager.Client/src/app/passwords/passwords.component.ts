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

  constructor(private passwordService: PasswordService) {
  }

  ngOnInit(): void {
    this.getAllPasswords();
  }

  private getAllPasswords() {
    this.passwordService.get().subscribe(
      data => {
        this.passwords = data;
      });
  }
}
