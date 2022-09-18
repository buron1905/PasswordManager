import { HttpClient } from '@angular/common/http';
import { Injectable, ResolvedReflectiveFactory } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private loginPath = `${environment.apiUrl}/auth/login`;
  private registerPath = `${environment.apiUrl}/auth/register`;
  
  constructor(private http: HttpClient, private router: Router) { }

  login(data : AbstractControl<any, any>) : Observable<any> {
    return this.http.post(this.loginPath, data);
  }

  register(data : AbstractControl<any, any>) : Observable<any> {
    return this.http.post(this.registerPath, data);
  }

  logout() : void {
    this.removeToken();
    this.router.navigate(['/login']);
  }

  saveToken(token: string) : void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  removeToken() : void { 
    localStorage.removeItem('token');
  }

  isAuthenticated() : boolean {
    if(this.getToken()) {
      return true;
    }
    return false;
  }

}
