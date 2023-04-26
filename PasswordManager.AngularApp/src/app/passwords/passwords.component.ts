import { Component, OnInit } from '@angular/core';
import { PasswordService } from '../services/password.service';
import { PasswordModel } from '../models/password.model';
import { ToastrService } from 'ngx-toastr';
import { ClipboardService } from 'ngx-clipboard';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalDeletePasswordComponent } from '../modal-delete-password/modal-delete-password.component';
import { CallbackPipe } from '../utils/callback.pipe';

@Component({
  selector: 'app-passwords',
  templateUrl: './passwords.component.html',
  styleUrls: ['./passwords.component.css']
})
export class PasswordsComponent implements OnInit {
  passwordsAll: PasswordModel[];
  passwords: PasswordModel[];
  searchText: string = "";
  searchFavorites: boolean = false;
  loading = false;

  constructor(private passwordService: PasswordService, private toastrService: ToastrService, private clipboardService: ClipboardService, private router: Router,
    private modalService: NgbModal  ) { }

  ngOnInit(): void {
    this.loading = true;
    this.getAllPasswords();
  }

  getAllPasswords() {
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

  delete(guid: string): void {
    const modalDeletePassword = this.modalService.open(ModalDeletePasswordComponent);
    modalDeletePassword.componentInstance.passwordsGuids = new Array<string>(guid);
    modalDeletePassword.result.then((result) => {

      if (result) {
        this.loading = true;

        this.passwordService.delete(guid).subscribe(
          data => {
            this.toastrService.success('Password deleted');
            this.getAllPasswords();
            this.loading = false;
          },
          error => {
            this.toastrService.error("Error occured during saving");
            this.loading = false;
          }
        );
      }
    });
  }

  deleteSelected(): void {
    let passwordsArray: Array<string> = new Array();

    (document.querySelectorAll('.rowCheckbox') as NodeListOf<HTMLInputElement>).forEach((checkbox) => {
      if (checkbox.checked) {
        passwordsArray.push(checkbox.dataset['id']);
      }
    });

    if (passwordsArray.length > 0) {
      const modalDeletePassword = this.modalService.open(ModalDeletePasswordComponent);
      modalDeletePassword.componentInstance.passwordsGuids = passwordsArray;
      modalDeletePassword.result.then((result) => {

        if (result) {
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
      });
    }
  }

  copyToClipboard(text: string): void {
    this.clipboardService.copyFromContent(text);
    this.toastrService.success('Copied to clipboard');
  }

  toggleFavorite(): void {
    this.searchFavorites = !this.searchFavorites;
  }

  findByPasswordNameAndUsername(password: PasswordModel, filterText: string, searchFavorites: boolean = false): any {
    if (searchFavorites && !password.favorite) {
      return false;
    }

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
