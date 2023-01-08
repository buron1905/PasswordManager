import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PasswordModel } from '../models/password.model';
import { PasswordService } from '../services/password.service';

@Component({
  selector: 'app-details-password',
  templateUrl: './details-password.component.html',
  styleUrls: ['./details-password.component.css']
})
export class DetailsPasswordComponent implements OnInit {
  id: string;
  password: PasswordModel;

  constructor(private passwordService: PasswordService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.id = params['id'];
    });
    //this.passwordService.get(this.id).subscribe({
      
    //})
  }

}
