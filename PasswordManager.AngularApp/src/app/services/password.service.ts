import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { PasswordModel } from '../models/password.model';
import { ActivatedRoute } from '@angular/router';
import { EncryptionService } from './encryption.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {

  private passwordsPath = `${environment.apiUrl}/passwords`;
  
  constructor(private http: HttpClient, private route: ActivatedRoute, private encryptionService: EncryptionService) { }

  get(): Observable<PasswordModel[]> {
    return this.http.get<PasswordModel[]>(this.passwordsPath);
  }

  getPassword(id : string): Observable<PasswordModel> {
    return this.http.get<PasswordModel>(`${this.passwordsPath}/${id}`);
  }

  create(data: PasswordModel): Observable<PasswordModel> {
    data.passwordDecrypted = this.encryptionService.encryptAesAsync(data.passwordDecrypted, "asdfasdf");

    return this.http.post<PasswordModel>(this.passwordsPath, data);
  }

  update(id:string, data: AbstractControl<any, any>): Observable<PasswordModel> {
    return this.http.put<PasswordModel>(`${this.passwordsPath}/${id}`, data);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.passwordsPath}/${id}`);
  }

  deleteMany(guids: Array<string>): Observable<void> {
    return this.http.delete<void>(`${this.passwordsPath}`, { params: {guids} });
  }

}
