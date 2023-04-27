import { HttpClient } from '@angular/common/http';
import { Injectable, ResolvedReflectiveFactory } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { lastValueFrom, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from './../models/login.model';
import { AuthResponseModel } from '../models/auth-response.model';
import { TfaSetup } from '../models/tfa-setup.model';
import { EmailConfirmationModel } from '../models/email-confirmation.model';
import { RegisterResponseModel } from '../models/register-response.model';
import * as forge from 'node-forge';

@Injectable({
  providedIn: 'root'
})
export class EncryptionService {
  
  private getPublicKeyPath = `${environment.apiUrl}/auth/public-key`;

  publicKey: string = `-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAieE7DlvEwNgtndPx6krZ
bZxDRRcFxoZ0nkqsRt1t1faQfSLjSt1EyHJnsIoDVSb4xuqnZO9OpquaQtosmjL1
+9ftFR1Y1HggUOaKo+fmBsEvXJYjNfRpNeEEbbtkLWgdpzHg9mDJa7SsuFgWD2k0
JoOa4wOJhfY6PUFtEMIsZklYn9bTwr+LfqxLMW55qK7AXK0JSx8QGQrzLj0BeLLu
4UADlrdeXkY4YMOWTx1St8KdB7GnjU2vXJK5HRSsg+/Fz/ShdZ+IvDfw6FtXTnqr
L+ggb/nQHzGq/yhNFb1gP77CiBL0AmvIs+8luhGuhgA4MNOKQH7+BWfWzEgIcSkQ
NwIDAQAB
-----END PUBLIC KEY-----`;

  constructor(private http: HttpClient ) { }

  encryptRsa(valueToEncrypt: string): string {
    let rsa = forge.pki.publicKeyFromPem(this.publicKey);
    return window.btoa(rsa.encrypt(valueToEncrypt, 'RSA-OAEP'));
  }

  encryptAes(valueToEncrypt: string, key: string): string {
    return '';
  }

  decryptAes(valueToDecrypt: string, key: string): string {
    return '';
  }

}
