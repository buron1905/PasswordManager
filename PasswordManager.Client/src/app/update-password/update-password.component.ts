import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, mergeMap } from 'rxjs/operators';
import { PasswordModel } from '../models/password.model';
import { PasswordService } from '../services/password.service';

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrls: ['./update-password.component.css']
})
export class UpdatePasswordComponent implements OnInit {
  id: string;
  password: PasswordModel;
  passwordForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private passwordService: PasswordService, private router: Router, private toastrService: ToastrService,
    private route: ActivatedRoute) {
    this.passwordForm = this.fb.group({
      passwordName: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      passwordDecrypted: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(255)]],
      description: ['', [Validators.maxLength(255)]]
    });
}

  ngOnInit(): void {
    this.fetchData();
  }

  // TODO: Duplicate in details
  fetchData() {
    this.route.params.pipe(map(params => {
      this.id = params['id'];
      return this.id;
    }), mergeMap(id => this.passwordService.getPassword(id))).subscribe(data => {
      this.password = data;
    })
  }

  update(): void {
    this.submitted = true;
    if (this.passwordForm.invalid) {
      return;
    }

    console.log(this.passwordForm.value);

    this.passwordService.update(this.id, this.passwordForm.value).subscribe(
      data => {
        this.toastrService.success('Saved');
        // TODO:
        // open detail of password. Detail can be opened will be opened without calling of API, just by passing data
        this.router.navigate([`/passwords/details/${data.id}`]);
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
