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

  generateKey(password: string, salt: CryptoJS.lib.WordArray): string {
    return CryptoJS.PBKDF2(password, salt, {
      keySize: 256 / 32,
      iterations: 10000,
    });
  }

  encrypt(plainTextData: string, password: string): string {
    var iv = CryptoJS.lib.WordArray.random(128 / 8);
    var salt = CryptoJS.lib.WordArray.random(32);
    var key = this.generateKey(password, salt);
    
    //will attach link where you can find these
    var encrypted = CryptoJS.AES.encrypt(plainTextData, key, {
      padding: CryptoJS.pad.Pkcs7,
      mode: CryptoJS.mode.CBC,
      iv: iv
    });
    
    //Convert Lib.WordArray to ByteArray so we can combine them like Concat
    var saltwords = this.wordArrayToByteArray(salt);
    var ivwords = this.wordArrayToByteArray(iv);
    var cryptedText = this.wordArrayToByteArray(encrypted.ciphertext);
    // combine everything together in ByteArray.
    var header = saltwords.concat(ivwords).concat(cryptedText);
    //Now convert to WordArray.
    var headerWords = this.byteArrayToWordArray(header);
    //Encode this to sent to server
    var encodedString = CryptoJS.enc.Base64.stringify(headerWords);
    return encodedString;
  }

  encryptUsingAes(plainTextData: string, password: string): string {
    var hash = CryptoJS.SHA256(password);

    //var hash1 = CryptoJS.enc.Utf8.parse(CryptoJS.SHA256(plainTextKey));
    //var hash2 = CryptoJS.enc.Base64.parse(CryptoJS.SHA256(plainTextKey));
    //var hash3 = CryptoJS.SHA256(plainTextKey).toString();
    //var hash5 = CryptoJS.SHA256(plainTextKey).toString(CryptoJS.enc.Utf8);
    //var hash6 = CryptoJS.SHA256(plainTextKey).toString(CryptoJS.enc.Hex);
    const iv = CryptoJS.enc.Utf8.parse("1203199320052021");
    const encrypted = CryptoJS.AES.encrypt(plainTextData, hash, {
      keySize: 256 / 32,
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });

    var usedKey = encrypted.key.toString(CryptoJS.enc.Base64);
    var usedIV = encrypted.iv.toString(CryptoJS.enc.Base64);

    var cipherText2 = encrypted.toString();
    var cipherText = CryptoJS.enc.Base64.stringify(encrypted.ciphertext);

    return encrypted.toString();
  }
   //const ivAndSalt = IV.concat(salt);
   //const ivAndEncryptedData = ivAndSalt.concat(encrypted.ciphertext);

    //const base64Data = encrypted.toString();
   //const base64Data = ivAndEncryptedData.toString(CryptoJS.enc.Base64);
   //const base64Data = window.btoa(ivAndEncryptedData);

  decryptUsingAes(cipherText: string, plainTextKey: string): string {

    const bytes = CryptoJS.AES.decrypt(cipherText, plainTextKey);
    const plainText = bytes.toString(CryptoJS.enc.Utf8);

    return plainText;
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

  wordArrayToByteArray(wordArray) {
    if (wordArray.hasOwnProperty("sigBytes") && wordArray.hasOwnProperty("words")) {
      length = wordArray.sigBytes;
      wordArray = wordArray.words;
    }

    var result = [],
      bytes,
      i = 0;
    while (length > 0) {
      bytes = this.wordToByteArray(wordArray[i], Math.min(4, length));
      length -= bytes.length;
      result.push(bytes);
      i++;
    }
    return [].concat.apply([], result);
  }
  byteArrayToWordArray(ba) {
    var wa = [],
      i;
    for (i = 0; i < ba.length; i++) {
      wa[(i / 4) | 0] |= ba[i] << (24 - 8 * i);
    }

    return CryptoJS.lib.WordArray.create(wa);
  }
  
  wordToByteArray(word, length) {
    var ba = [],
      xFF = 0xff;
    if (length > 0) ba.push(word >>> 24);
    if (length > 1) ba.push((word >>> 16) & xFF);
    if (length > 2) ba.push((word >>> 8) & xFF);
    if (length > 3) ba.push(word & xFF);

    return ba;
  }

}
