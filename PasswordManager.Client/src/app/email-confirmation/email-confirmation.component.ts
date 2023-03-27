import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmailConfirmationModel } from '../models/email-confirmation.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {
  showSuccess = false;
  loading = false;
  verifyEmailDTO: EmailConfirmationModel;

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) {
    this.verifyEmailDTO = { Email: '', Token: '' };
  }

  ngOnInit(): void {
    this.verifyEmailDTO.Email = this.route.snapshot.params['email'];
    this.verifyEmailDTO.Token = this.route.snapshot.params['token'];

    this.verifyEmail();
  }

  verifyEmail() {
    this.loading = true;
    this.authService.verifyEmail(this.verifyEmailDTO).subscribe(
      data => {
        this.showSuccess = true;
        this.loading = false;
      },
      error => {
        this.showSuccess = false;
        this.loading = false;
      });
  }

}
