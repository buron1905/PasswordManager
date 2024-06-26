import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AbstractControl, FormArray } from '@angular/forms';
import { Observable } from 'rxjs';
import { PasswordGeneratorSettingsResponse } from '../models/password-generator-settings-response.model';
import { ActivatedRoute } from '@angular/router';
import { PasswordGeneratorSettings } from '../models/password-generator-settings.model';

@Injectable({
  providedIn: 'root'
})
export class PasswordGeneratorService {

  private passwordsGeneratorPath = `${environment.apiUrl}/passwords/generator`;

  private readonly letters: string = "abcdefghijklmnopqrstuvwxyz";
  private readonly numbers: string = "0123456789";
  private readonly specialChars: string = "!@#$%^&*";

  constructor(private http: HttpClient, private route: ActivatedRoute) { }

  generatePassword(data: AbstractControl<any, any>): Observable<PasswordGeneratorSettingsResponse> {
    return this.http.post<PasswordGeneratorSettingsResponse>(this.passwordsGeneratorPath, data);
  }

  public generatePasswordFromModel(settings: PasswordGeneratorSettings): string {
    return this.generatePasswordWithOptions(settings.passwordLength, settings.useNumbers, settings.useSpecialChars, settings.useUppercase, settings.useLowercase);
  }

  public generatePasswordWithOptions(length: number = 8, useNumbers: boolean = true, useSpecialChars: boolean = true, useUppercase: boolean = true, useLowercase: boolean = true): string {
    if (length < 1) {
      return "";
    }

    let sb: string = "";

    if (useNumbers) {
      sb += this.numbers;
    }

    if (useSpecialChars) {
      sb += this.specialChars;
    }

    if (useUppercase) {
      sb += this.letters.toUpperCase();
    }

    if (useLowercase) {
      sb += this.letters;
    }

    const alphabet: string = sb.toString();
    if (alphabet.length === 0) {
      return "";
    }

    let password: string;
    do {
      sb = "";
      for (let i = 0; i < length; i++) {
        const next: number = this.getRandomInt(alphabet.length);
        sb += alphabet[next];
      }
      password = sb.toString();
    } while (!this.checkConstraints(password, useNumbers, useSpecialChars, useUppercase, useLowercase));

    return password;
  }

  private checkConstraints(text: string, useNumbers: boolean = true, useSpecialChars: boolean = true, useUppercase: boolean = true, useLowercase: boolean = true): boolean {
    if (useNumbers && !/\d/.test(text)) {
      return false;
    } else if (!useNumbers && /\d/.test(text)) {
      return false;
    }

    if (useUppercase && !/[A-Z]/.test(text)) {
      return false;
    } else if (!useUppercase && /[A-Z]/.test(text)) {
      return false;
    }

    if (useLowercase && !/[a-z]/.test(text)) {
      return false;
    } else if (!useLowercase && /[a-z]/.test(text)) {
      return false;
    }

    if (useSpecialChars) {
      if (!/[!@#$%^&*]/.test(text)) {
        return false;
      }
    } else if (!useSpecialChars) {
      if (/[!@#$%^&*]/.test(text)) {
        return false;
      }
    }

    return true;
  }

  private getRandomInt(max: number): number {
    const randomBytes: Uint8Array = new Uint8Array(1);
    window.crypto.getRandomValues(randomBytes);
    return randomBytes[0] % max;
  }

}
