import { HttpClient } from '@angular/common/http';
import { Injectable, ResolvedReflectiveFactory } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { lastValueFrom, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from './../models/login.model';
import { AuthResponseModel } from '../models/auth-response.model';
import { TfaSetup } from '../models/tfa-setup.model';
import { EmailConfirmationModel } from '../models/email-confirmation.model';
import { RegisterResponseModel } from '../models/register-response.model';
import { EncryptionService } from '../services/encryption.service';
import { RegisterModel } from '../models/register.model';
import { LoginTfaModel } from '../models/login-tfa.model';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private loginPath = `${environment.apiUrl}/auth/login`;
  private registerPath = `${environment.apiUrl}/auth/register`;
  private tfaLoginPath = `${environment.apiUrl}/auth/tfa-login`;
  private tfaSetupPath = `${environment.apiUrl}/auth/tfa-setup`;
  private verifyEmailPath = `${environment.apiUrl}/auth/email-confirm`;

  private logoutTimer: any;

  public cipherKey: string | null = null;
  public tokenExpirationDateTime: Date | null = null;

  constructor(private http: HttpClient, private router: Router, private encryptionService: EncryptionService, 
    private toastrService: ToastrService) { }

  setCipherKeyToSHA256Value(plainPassword: string): void {
    this.cipherKey = this.encryptionService.hashUsingSHA256(plainPassword);
  }

  setTokenExpiration(expirationDateTime: string): void {
    this.tokenExpirationDateTime = new Date(Date.parse(expirationDateTime));
  }

  refreshTokenExpirationDateTime(): void {
    // set 7 minutes from now (same as server with HttpOnly cookie)
    this.tokenExpirationDateTime = new Date(new Date().getTime() + 7 * 60000);
    this.refreshLogoutTimer();
  }

  refreshLogoutTimer(): void {
    if (this.logoutTimer) {
      clearTimeout(this.logoutTimer);
    }
    this.logoutTimer = setTimeout(() => {
      this.checkCanActivate(true);
    }, this.tokenExpirationDateTime.getTime() - new Date().getTime());
  }

  login(data: LoginModel): Observable<AuthResponseModel> {
    data.password = this.encryptionService.encryptUsingRSA(data.password);
    return this.http.post<AuthResponseModel>(this.loginPath, data);
  }

  register(data: RegisterModel): Observable<RegisterResponseModel> {
    data.password = this.encryptionService.encryptUsingRSA(data.password);
    data.confirmPassword = this.encryptionService.encryptUsingRSA(data.confirmPassword);
    return this.http.post<RegisterResponseModel>(this.registerPath, data);
  }

  verifyEmail(data: EmailConfirmationModel): Observable<void> {
    return this.http.post<void>(`${this.verifyEmailPath}`, data);
  }

  loginTfa(data: LoginTfaModel): Observable<AuthResponseModel> {
    data.password = this.encryptionService.encryptUsingRSA(data.password);
    return this.http.post<AuthResponseModel>(this.tfaLoginPath, data);
  }
  
  getTfaSetup(): Observable<TfaSetup> {
    return this.http.get<TfaSetup>(`${this.tfaSetupPath}`).pipe(
      map((authResponse: TfaSetup) => {
        this.refreshTokenExpirationDateTime();
        return authResponse;
      })
    );
  }

  setupTfa(data: AbstractControl<any, any>): Observable<TfaSetup> {
    return this.http.post<TfaSetup>(this.tfaSetupPath, data).pipe(
      map((authResponse: TfaSetup) => {
        this.refreshTokenExpirationDateTime();
        return authResponse;
      })
    );
  }

  logout(redirect: boolean = true): void {
    this.cipherKey = null;
    this.tokenExpirationDateTime = null;

    if (redirect)
      this.router.navigate(['login']);
  }

  isAuthenticated() : boolean {
    if (!this.cipherKey || !this.cipherKey.trim()) {
      return false;
    }

    if (!this.tokenExpirationDateTime){ 
      return false;
    }

    if (this.tokenExpirationDateTime.getTime() <= new Date().getTime()) {
      return false;
    }

    return true;
  }

  checkCanActivate(showLogoutIdleToast: boolean = false) : boolean {
    if(this.isAuthenticated()) {
      return true;
    } else {
      this.logout();
      if (showLogoutIdleToast) {
        this.toastrService.warning('You have been logged out due to inactivity.', 'Logged Out');
      }

      if (this.router.url !== '/login' && this.router.url !== '/register' && this.router.url !== '/email-confirm' && this.router.url !== '/home') {
        this.router.navigate(['login']);
      }
      return false;
    }
  }
  
}
