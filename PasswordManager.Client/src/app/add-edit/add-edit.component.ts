import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PasswordModel } from '../models/password.model';
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

  isAddMode: boolean;

  constructor(private fb: FormBuilder, private passwordService: PasswordService, private router: Router, private toastrService: ToastrService,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.isAddMode = !this.id;

    this.passwordForm = this.fb.group({
      id: ['00000000-0000-0000-0000-000000000000', [this.isAddMode ? Validators.nullValidator : Validators.required]],
      passwordName: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      passwordDecrypted: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(255)]],
      description: ['', [Validators.maxLength(255)]]
    });

    if (!this.isAddMode) {
      this.fetchData();
    }
  }

  fetchData() {
    this.passwordService.getPassword(this.id).subscribe(data => {
      this.password = data;
      this.passwordForm.patchValue(this.password);
    })
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
    this.passwordService.create(this.passwordForm.value).subscribe(
      data => {
        this.toastrService.success('Created');
        this.router.navigate(['/passwords']);
      },
      error => {
        this.loading = false;
      }
    );
  }

  updatePassword(): void {
    this.passwordService.update(this.id, this.passwordForm.value).subscribe(
      data => {
        this.toastrService.success('Saved');
        this.router.navigate(['/passwords']);
      },
      error => {
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

  get description() {
    return this.passwordForm.get('description');
  }

}

