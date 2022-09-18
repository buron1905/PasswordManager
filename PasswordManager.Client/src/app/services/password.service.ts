import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { Password } from '../models/password';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {

  private passwordsPath = `${environment.apiUrl}/passwords`;
  
  constructor(private http: HttpClient) { }

  create(data : AbstractControl<any, any>) : Observable<Password> {
    return this.http.post<Password>(this.passwordsPath, data);
  }

}
