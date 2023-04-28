import { HttpClient } from '@angular/common/http';
import { Injectable, ResolvedReflectiveFactory } from '@angular/core';
import * as CryptoJS from 'crypto-js';
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
    const rsa = forge.pki.publicKeyFromPem(this.publicKey);
    return window.btoa(rsa.encrypt(valueToEncrypt, 'RSA-OAEP'));
  }

  encryptAes(valueToEncrypt: string, key: string): string {
    return '';
  }

  decryptAes(valueToDecrypt: string, key: string): string {
    return '';
  }

  encryptAesAsync(plainTextData: string, plainTextKey: string): string {
    const IV = CryptoJS.enc.Utf8.parse("1203199320052021");
    const salt = CryptoJS.enc.Utf8.parse("12031993200520211203199320052021");
    //const IV = CryptoJS.lib.WordArray.random(16);
    //const salt = CryptoJS.lib.WordArray.random(32);
    const key = CryptoJS.PBKDF2(plainTextKey, salt, { keySize: 256 / 32, iterations: 10000, hasher: CryptoJS.algo.SHA512 });

    const encrypted = CryptoJS.AES.encrypt(plainTextData, key, { iv: IV, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });

   //const ivAndSalt = IV.concat(salt);
   //const ivAndEncryptedData = ivAndSalt.concat(encrypted.ciphertext);

    const base64Data = encrypted.toString();
   //const base64Data = ivAndEncryptedData.toString(CryptoJS.enc.Base64);
   //const base64Data = window.btoa(ivAndEncryptedData);

    return base64Data;
  }

    // Concatenate IV and encrypted data
    //const ivAndEncryptedData = CryptoJS.lib.WordArray.create(IV.words.length + salt.words.length + encrypted.ciphertext.words.length);

//ivAndEncryptedData.set(IV.words, 0);
    //ivAndEncryptedData.set(salt.words, IV.words.length);
    //ivAndEncryptedData.set(encrypted.ciphertext.words, IV.words.length + salt.words.length);

   //const base64Data = window.btoa(String.fromCharCode.apply(null, ivAndEncryptedData));
   //const base64Data = CryptoJS.enc.Base64.stringify(ivAndEncryptedData);
   //var decoder = new TextDecoder('utf8');
   //var b64encoded = btoa(decoder.decode(u8));





  //encryptUsingAES256(text): any {
  //  const encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(text), this.key, {
  //    keySize: 128 / 8,
  //    iv: this.iv,
  //    mode: CryptoJS.mode.CBC,
  //    padding: CryptoJS.pad.Pkcs7
  //  });
  //  return encrypted.toString();
  //}
  //decryptUsingAES256(decString) {
  //  const decrypted = CryptoJS.AES.decrypt(decString, this.key, {
  //    keySize: 128 / 8,
  //    iv: this.iv,
  //    mode: CryptoJS.mode.CBC,
  //    padding: CryptoJS.pad.Pkcs7
  //  });
  //  return decrypted.toString(CryptoJS.enc.Utf8);
  //}

}
