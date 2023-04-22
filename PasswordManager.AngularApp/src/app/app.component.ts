import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { BnNgIdleService } from 'bn-ng-idle';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Password Manager';

  constructor(http: HttpClient, private bnIdle: BnNgIdleService, private authService: AuthService) {
  }

  ngOnInit(): void {
    //// every 5 minutes of inactivity
    //this.bnIdle.startWatching(300).subscribe(async () => {
    //  if (this.authService.isAuthenticated()) {
    //    if (!(await this.authService.getTokenIsValid())) {
    //      this.authService.logout();
    //    }
    //  }
    //});
  }
}
