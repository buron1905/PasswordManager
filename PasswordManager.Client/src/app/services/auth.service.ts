import { HttpClient } from '@angular/common/http';
import { Injectable, ResolvedReflectiveFactory } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private loginPath = `${environment.apiUrl}/identity/login`;
  private registerPath = `${environment.apiUrl}/identity/register`;
  
  constructor(private http: HttpClient, private router: Router, private jwtHelper: JwtHelperService) { }

  login(data : AbstractControl<any, any>) : Observable<any> {
    return this.http.post(this.loginPath, data);
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
    if(token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }

}
