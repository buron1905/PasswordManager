import { HttpClient } from '@angular/common/http';
import { Injectable, ResolvedReflectiveFactory } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { lastValueFrom, Observable } from 'rxjs';
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

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private loginPath = `${environment.apiUrl}/auth/login`;
  private registerPath = `${environment.apiUrl}/auth/register`;
  private tfaLoginPath = `${environment.apiUrl}/auth/tfa-login`;
  private tfaSetupPath = `${environment.apiUrl}/auth/tfa-setup`;
  private verifyEmailPath = `${environment.apiUrl}/auth/email-confirm`;
  
  constructor(private http: HttpClient, private router: Router, private encryptionService: EncryptionService, private jwtHelper: JwtHelperService) { }


  login(data: LoginModel): Observable<AuthResponseModel> {
    data.password = this.encryptionService.encryptRsa(data.password);
    return this.http.post<AuthResponseModel>(this.loginPath, data);
  }

  register(data: RegisterModel): Observable<RegisterResponseModel> {
    data.password = this.encryptionService.encryptRsa(data.password);
    data.confirmPassword = this.encryptionService.encryptRsa(data.confirmPassword);
    return this.http.post<RegisterResponseModel>(this.registerPath, data);
  }

  loginLocal(data: AuthResponseModel) : void {
    localStorage.setItem('expirationDateTime', data.expirationDateTime);
  }

  loginTfa(data: LoginTfaModel): Observable<AuthResponseModel> {
    data.password = this.encryptionService.encryptRsa(data.password);
    return this.http.post<AuthResponseModel>(this.tfaLoginPath, data);
  }
  
  getTfaSetup(): Observable<TfaSetup> {
    return this.http.get<TfaSetup>(`${this.tfaSetupPath}`);
  }

  setupTfa(data: AbstractControl<any, any>): Observable<TfaSetup> {
    return this.http.post<TfaSetup>(this.tfaSetupPath, data);
  }

  logout(redirect: boolean = true): void {
    localStorage.removeItem('expirationDateTime');
    if (redirect)
      this.router.navigate(['login']);
  }

  getExpirationDateTime(): string | null {
    return localStorage.getItem('expirationDateTime');
  }

  isAuthenticated() : boolean {
    const expirationDateTime = this.getExpirationDateTime();
    if (expirationDateTime)
      return true;
    else
      return false;
  }

  async getTokenIsValid(): Promise<boolean> {
    //const token = this.getToken();
    //if (token) {
    //  return (await lastValueFrom(this.http.post<{ isValid: boolean }>(this.tokenIsValidPath, token))).isValid;
    //}
    return false;
  }

  verifyEmail(data: EmailConfirmationModel): Observable<void> {
    return this.http.post<void>(`${this.verifyEmailPath}`, data);
  }
  
}
