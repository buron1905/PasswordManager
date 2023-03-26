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

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private loginPath = `${environment.apiUrl}/auth/login`;
  private registerPath = `${environment.apiUrl}/auth/register`;
  private tfaLoginPath = `${environment.apiUrl}/auth/tfa-login`;
  private tfaSetupPath = `${environment.apiUrl}/auth/tfa-setup`;
  private tfaDisablePath = `${environment.apiUrl}/auth/tfa-disable`;
  
  constructor(private http: HttpClient, private router: Router, private jwtHelper: JwtHelperService) { }


  authenticate(data: AbstractControl<any, any>): Observable<AuthResponseModel> {
    return this.http.post<AuthResponseModel>(this.loginPath, data);
  }

  register(data: AbstractControl<any, any>): Observable<AuthResponseModel> {
    return this.http.post<AuthResponseModel>(this.registerPath, data);
  }

  login(data: AuthResponseModel) : void {
    localStorage.setItem('expirationDateTime', data.expirationDateTime);
  }

  loginTfa(data: AbstractControl<any, any>): Observable<AuthResponseModel> {
    return this.http.post<AuthResponseModel>(this.tfaLoginPath, data);
  }
  
  getTfaSetup(): Observable<TfaSetup> {
    return this.http.get<TfaSetup>(`${this.tfaSetupPath}`);
  }

  enableTfa(data: AbstractControl<any, any>): Observable<TfaSetup> {
    return this.http.post<TfaSetup>(this.tfaSetupPath, data);
  }

  disableTfa(data: AbstractControl<any, any>): Observable<TfaSetup> {
    return this.http.post<TfaSetup>(this.tfaDisablePath, data);
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
  
}
