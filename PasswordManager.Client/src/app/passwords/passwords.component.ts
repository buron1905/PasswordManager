import { Component, OnInit } from '@angular/core';
import { PasswordService } from '../services/password.service';
import { PasswordModel } from '../models/password.model';
import { ToastrService } from 'ngx-toastr';
import { ClipboardService } from 'ngx-clipboard';
import { Router } from '@angular/router';

@Component({
  selector: 'app-passwords',
  templateUrl: './passwords.component.html',
  styleUrls: ['./passwords.component.css']
})
export class PasswordsComponent implements OnInit {
  passwordsAll: PasswordModel[];
  passwords: PasswordModel[];
  searchText: string = "";
  loading = false;

  constructor(private passwordService: PasswordService, private toastrService: ToastrService, private clipboardService: ClipboardService, private router: Router) { }

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

  delete(guid: any): void {
    if (confirm(`Do you really want to delete selected password?`)) {
      this.loading = true;

      this.passwordService.delete(guid).subscribe(
        data => {
          this.toastrService.success('Password deleted');
          this.getAllPasswords();
          this.loading = false;
        },
        error => {
          this.toastrService.error(error);
          this.loading = false;
        }
      );
    }
  }

  deleteSelected(): void {

    let passwordsArray: Array<string> = new Array();

    (document.querySelectorAll('.rowCheckbox') as NodeListOf<HTMLInputElement>).forEach((checkbox) => {
      if (checkbox.checked) {
        passwordsArray.push(checkbox.dataset['id']);
      }
    });

    if (passwordsArray.length > 0) {
      if (confirm(`Do you really want to delete selected passwords?\n${passwordsArray.length} passwords will be deleted.`)) {
        this.loading = true;
        this.passwordService.deleteMany(passwordsArray).subscribe(
          data => {
            this.toastrService.success('Passwords deleted');
            this.getAllPasswords();
            this.loading = false;
          },
          error => {
            this.toastrService.error(error);
            this.loading = false;
          }
        );
      }
    }
  }

  copyToClipboard(text: string): void {
    this.clipboardService.copyFromContent(text);
    this.toastrService.success('Copied to clipboard');
  }

  findByPasswordNameAndUsername(password: PasswordModel, filterText: string): any {
    filterText = filterText?.trim().toLowerCase() ?? '';
    let passwordName: string = password.passwordName?.trim().toLowerCase() ?? '';
    let userName: string = password.userName?.trim().toLowerCase() ?? '';
    let notes: string = password.notes?.trim().toLowerCase() ?? '';

    let startsWith: boolean = passwordName.startsWith(filterText) ||
      userName.startsWith(filterText) ||
      notes.startsWith(filterText);

    let includes: boolean = passwordName.includes(filterText) ||
      userName.includes(filterText) ||
      notes.includes(filterText);

    return startsWith || includes;
  }

  openDetail(passwordId: string) {
    this.router.navigate(['/passwords/edit', passwordId]);
  }

}
