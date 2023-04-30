import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AbstractControl } from '@angular/forms';
import { Observable, map } from 'rxjs';
import { PasswordModel } from '../models/password.model';
import { ActivatedRoute } from '@angular/router';
import { EncryptionService } from './encryption.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {

  private passwordsPath = `${environment.apiUrl}/passwords`;
  
  constructor(private http: HttpClient, private route: ActivatedRoute, private encryptionService: EncryptionService, private authService: AuthService) { }

  // decryptOnlyNames is here for optimalization purpose 
  getAll(decryptNamesOnly : boolean = false): Observable<PasswordModel[]> {
    return this.http.get<PasswordModel[]>(this.passwordsPath).pipe(
      map((passwordModels: PasswordModel[]) => {
        this.authService.refreshTokenExpirationDateTime();
        // Decrypt each password of the array
        return passwordModels.map((passwordModel: PasswordModel) => {
          let passwordModelDecrypted = this.decryptPassword(passwordModel, decryptNamesOnly);
          return passwordModelDecrypted;
        });
      })
    );
  }

  get(id: string): Observable<PasswordModel> {
    return this.http.get<PasswordModel>(`${this.passwordsPath}/${id}`).pipe(
      map((passwordModel: PasswordModel) => {
        this.authService.refreshTokenExpirationDateTime();
        let passwordModelDecrypted = this.decryptPassword(passwordModel);
        return passwordModelDecrypted;
      })
    );
  }

  create(data: PasswordModel): Observable<PasswordModel> {
    let passwordModelEncrypted = this.encryptPassword(data);
    return this.http.post<PasswordModel>(this.passwordsPath, passwordModelEncrypted).pipe(
      map((passwordModel: PasswordModel) => {
        this.authService.refreshTokenExpirationDateTime();
        return passwordModel;
      })
    );
  }

  update(id:string, data: PasswordModel): Observable<PasswordModel> {
    let passwordModelEncrypted = this.encryptPassword(data);
    return this.http.put<PasswordModel>(`${this.passwordsPath}/${id}`, passwordModelEncrypted).pipe(
      map((passwordModel: PasswordModel) => {
        this.authService.refreshTokenExpirationDateTime();
        return passwordModel;
      })
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.passwordsPath}/${id}`).pipe(
      map(() => {
        this.authService.refreshTokenExpirationDateTime();
      })
    );
  }

  deleteMany(guids: Array<string>): Observable<void> {
    return this.http.delete<void>(`${this.passwordsPath}`, { params: {guids} }).pipe(
      map(() => {
        this.authService.refreshTokenExpirationDateTime();
      })
    );
  }

  // helper methods

  encryptPassword(passwordModelDecrypted: PasswordModel, encryptNamesOnly: boolean = false): PasswordModel {
    let passwordModelEncrypted = {...passwordModelDecrypted};

    passwordModelEncrypted.passwordName = this.encryptionService.encryptUsingAES(passwordModelDecrypted.passwordName, this.authService.cipherKey);
    passwordModelEncrypted.userName = this.encryptionService.encryptUsingAES(passwordModelDecrypted.userName, this.authService.cipherKey);
    passwordModelEncrypted.passwordDecrypted = null;
    
    if(!encryptNamesOnly) {
      passwordModelEncrypted.passwordEncrypted = this.encryptionService.encryptUsingAES(passwordModelDecrypted.passwordDecrypted, this.authService.cipherKey);
      passwordModelEncrypted.url = this.encryptionService.encryptUsingAES(passwordModelDecrypted.url, this.authService.cipherKey);
      passwordModelEncrypted.notes = this.encryptionService.encryptUsingAES(passwordModelDecrypted.notes, this.authService.cipherKey);
    }

    return passwordModelEncrypted;
  }

  decryptPassword(passwordModelEncrypted: PasswordModel, decryptNamesOnly: boolean = false): PasswordModel {
    let passwordModelDecrypted = {...passwordModelEncrypted};

    passwordModelDecrypted.passwordName = this.encryptionService.decryptUsingAES(passwordModelEncrypted.passwordName, this.authService.cipherKey);
    passwordModelDecrypted.userName = this.encryptionService.decryptUsingAES(passwordModelEncrypted.userName, this.authService.cipherKey);
    
    if (!decryptNamesOnly) {
        passwordModelDecrypted.passwordDecrypted = this.encryptionService.decryptUsingAES(passwordModelEncrypted.passwordEncrypted, this.authService.cipherKey);
        passwordModelDecrypted.url = this.encryptionService.decryptUsingAES(passwordModelEncrypted.url, this.authService.cipherKey);
        passwordModelDecrypted.notes = this.encryptionService.decryptUsingAES(passwordModelEncrypted.notes, this.authService.cipherKey);
    }

    return passwordModelDecrypted;
  }

}
