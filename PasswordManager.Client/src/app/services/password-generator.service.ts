import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AbstractControl, FormArray } from '@angular/forms';
import { Observable } from 'rxjs';
import { PasswordGeneratorSettingsResponse } from '../models/password-generator-settings-response.model';
import { ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PasswordGeneratorService {

  private passwordsGeneratorPath = `${environment.apiUrl}/passwords/generator`;

  constructor(private http: HttpClient, private route: ActivatedRoute) { }

  generatePassword(data: AbstractControl<any, any>): Observable<PasswordGeneratorSettingsResponse> {
    // TODO: consider using get method. But the following returns 415 - bad media type
    //let queryParams = new HttpParams({ fromObject: data as any });
    //return this.http.get<PasswordGeneratorSettingsResponse>(this.passwordsGeneratorPath, { params: queryParams });

    return this.http.post<PasswordGeneratorSettingsResponse>(this.passwordsGeneratorPath, data);
  }
}
