import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginPath = `${environment.apiUrl}/auth/login`;
  private registerPath = `${environment.apiUrl}/auth/register`;
  
  constructor(private htttp: HttpClient) { }

  login(data : AbstractControl<any, any>) : Observable<any> {
    return this.htttp.post(this.loginPath, data);
  }

  register(data : AbstractControl<any, any>) : Observable<any> {
    return this.htttp.post(this.registerPath, data);
  }
}
