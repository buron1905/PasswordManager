import { HttpClient } from '@angular/common/http';
import { Injectable, ResolvedReflectiveFactory } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { lastValueFrom, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from './../models/login.model';
import { AuthResponseModel } from '../models/auth-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private loginPath = `${environment.apiUrl}/auth/login`;
  private registerPath = `${environment.apiUrl}/auth/register`;
  private tokenIsValidPath = `${environment.apiUrl}/auth/token-is-valid`;
  
  constructor(private http: HttpClient, private router: Router, private jwtHelper: JwtHelperService) { }


  authenticate(data : AbstractControl<any, any>) : Observable<any> {
    return this.http.post<AuthResponseModel>(this.loginPath, data);
  }

  login(data: AuthResponseModel) : void {
    localStorage.setItem('token', data.Token);
    localStorage.setItem('refreshToken', data.RefreshToken);
    localStorage.setItem('expirationDateTime', data.ExpirationDateTime.toString());
  }

  register(data : AbstractControl<any, any>) : Observable<any> {
    return this.http.post(this.registerPath, data);
  }

  logout() : void {
    this.removeToken();
    this.router.navigate(['login']);
  }

  saveToken(token: string) : void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  private removeToken() : void { 
    localStorage.removeItem('token');
  }

  isAuthenticated() : boolean {
    const token = this.getToken();
    if (token)
      return true;
    else
      return false;
  }

  async getTokenIsValid(): Promise<boolean> {
    const token = this.getToken();
    if (token) {
      return (await lastValueFrom(this.http.post<{ isValid: boolean }>(this.tokenIsValidPath, token))).isValid;
    }
    return false;
  }
  
}
