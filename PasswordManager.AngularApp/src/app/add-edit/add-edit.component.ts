import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ClipboardService } from 'ngx-clipboard';
import { ToastrService } from 'ngx-toastr';
import { ModalDeletePasswordComponent } from '../modal-delete-password/modal-delete-password.component';
import { ModalGeneratePasswordComponent } from '../modal-generate-password/modal-generate-password.component';
import { PasswordModel } from '../models/password.model';
import { PasswordGeneratorComponent } from '../password-generator/password-generator.component';
import { AuthService } from '../services/auth.service';
import { EncryptionService } from '../services/encryption.service';
import { PasswordService } from '../services/password.service';

@Component({
  selector: 'app-add-edit',
  templateUrl: './add-edit.component.html',
  styleUrls: ['./add-edit.component.css']
})
export class AddEditComponent implements OnInit {
  id: string;
  password: PasswordModel;
  passwordForm: FormGroup;
  loading = false;
  submitted = false;
  toggledPassword = false;

  isAddMode: boolean;

  constructor(private fb: FormBuilder, private passwordService: PasswordService, private router: Router, private toastrService: ToastrService,
    private route: ActivatedRoute, private clipboardService: ClipboardService, private modalService: NgbModal, private encryptionService: EncryptionService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.isAddMode = !this.id;

    this.passwordForm = this.fb.group({
      id: ['00000000-0000-0000-0000-000000000000', [this.isAddMode ? Validators.nullValidator : Validators.required]],
      idt: ['0001-01-01T00:00:00', [this.isAddMode ? Validators.nullValidator : Validators.required]],
      udt: ['0001-01-01T00:00:00', [this.isAddMode ? Validators.nullValidator : Validators.required]],
      udtLocal: ['0001-01-01T00:00:00', [this.isAddMode ? Validators.nullValidator : Validators.required]],
      ddt: ['0001-01-01T00:00:00', [this.isAddMode ? Validators.nullValidator : Validators.required]],
      deleted: [false, [this.isAddMode ? Validators.nullValidator : Validators.required]],
      passwordName: ['', [Validators.required]],
      userName: ['', [Validators.maxLength(512)]],
      passwordDecrypted: ['', [Validators.maxLength(1024)]],
      url: ['', [Validators.maxLength(2048)]],
      notes: ['', [Validators.maxLength(10000)]],
      favorite: [false, Validators.required]
    });

    if (!this.isAddMode) {
      this.fetchData();
    } else if (history.state.password) {
      this.password = history.state.password;
      this.password.id = '00000000-0000-0000-0000-000000000000';

      this.loading = true;
      this.password.passwordDecrypted = this.encryptionService.decryptUsingAES(this.password.passwordEncrypted, this.authService.cipherKey);
      this.password.url = this.encryptionService.decryptUsingAES(this.password.url, this.authService.cipherKey);
      this.password.notes = this.encryptionService.decryptUsingAES(this.password.notes, this.authService.cipherKey);
      this.loading = false;

      this.passwordForm.patchValue(this.password);
    }
  }

  fetchData() {
    this.loading = true;
    this.passwordService.get(this.id).subscribe(data => {
      this.password = data;
      this.passwordForm.patchValue(this.password);
      this.loading = false;
    },
      error => this.loading = false)
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.passwordForm.invalid) {
      return;
    }

    this.loading = true;

    if (this.isAddMode)
      this.createPassword();
    else
      this.updatePassword();
  }

  createPassword(): void {

    var passwordFormValue: PasswordModel = { ...this.passwordForm.value };

    this.passwordService.create(passwordFormValue).subscribe(
      data => {
        this.toastrService.success('Created');
        this.loading = false;
        this.router.navigate(['/passwords']);
      },
      error => {
        this.toastrService.error(error);
        this.loading = false;
      }
    );
  }

  updatePassword(): void {
    this.loading = true;
    this.passwordService.update(this.id, this.passwordForm.value).subscribe(
      data => {
        this.toastrService.success('Saved');
        this.loading = false;
        this.router.navigate(['/passwords']);
      },
      error => {
        this.toastrService.error(error);
        this.loading = false;
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.passwordForm.controls;
  }

  get passwordName() {
    return this.passwordForm.get('passwordName');
  }

  get userName() {
    return this.passwordForm.get('userName');
  }

  get passwordDecrypted() {
    return this.passwordForm.get('passwordDecrypted');
  }

  get url() {
    return this.passwordForm.get('url');
  }

  get notes() {
    return this.passwordForm.get('notes');
  }

  get favorite() {
    return this.passwordForm.get('favorite');
  }

  set favorite(value: any) {
    this.passwordForm.patchValue({ favorite: value });
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
            this.loading = false;
            this.router.navigate(['/passwords']);
          },
          error => {
            this.toastrService.error(error);
            this.loading = false;
          }
        );
      }
    });

  }

  copyToClipboard(text: string): void {
    this.clipboardService.copyFromContent(text);
    this.toastrService.success('Copied to clipboard');
  }

  togglePassword(): void {
    this.toggledPassword = !this.toggledPassword; 
  }

  toggleFavorite(): void {
    this.favorite = !this.favorite.value;
  }

  goToUrl(url: string): void {
    url = url.trim();
    if (url != '') {
      if (!url.startsWith('http://') && !url.startsWith('https://')) {
        url = 'http://' + url;
      }
      window.open(url, '_blank').focus();
    }
  }

  generatePassword(): void {

    const modalGeneratePassword = this.modalService.open(ModalGeneratePasswordComponent);
    modalGeneratePassword.result.then((result) => {
      if (result) {
        this.toastrService.success('Password generated');
        this.passwordForm.patchValue({ passwordDecrypted: result });
      }
    });

  }

}

