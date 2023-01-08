import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { PasswordModel } from '../models/password.model';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {

  private passwordsPath = `${environment.apiUrl}/passwords`;
  
  constructor(private http: HttpClient) { }

  get(): Observable<PasswordModel[]> {
    return this.http.get<PasswordModel[]>(this.passwordsPath);
  }

  //get(id : string): Observable<PasswordModel> {
  //  return this.http.get<PasswordModel>(this.passwordsPath);
  //}

  create(data : AbstractControl<any, any>) : Observable<PasswordModel> {
    return this.http.post<PasswordModel>(this.passwordsPath, data);
  }

}
