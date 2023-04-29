import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';
import { environment } from 'src/environments/environment';
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

  constructor() { }

  // AES

  encryptUsingAES(plainText: string, plainTextKey: string): string | null {
    if (!plainText || !plainText.trim() || !plainTextKey || !plainTextKey.trim()) {
      return null;
    }

    var ivWords = CryptoJS.lib.WordArray.random(128 / 8);
    var saltWords = CryptoJS.lib.WordArray.random(32);
    var key = this.generateKey(plainTextKey, saltWords);
    
    //will attach link where you can find these
    var cipherText = CryptoJS.AES.encrypt(plainText, key, {
      padding: CryptoJS.pad.Pkcs7,
      mode: CryptoJS.mode.CBC,
      iv: ivWords
    });
    
    //Convert Lib.WordArray to ByteArray so we can combine them with concat
    var saltBytes = this.wordArrayToByteArray(saltWords);
    var ivBytes = this.wordArrayToByteArray(ivWords);
    var cryptedTextBytes = this.wordArrayToByteArray(cipherText.ciphertext);
    // combine everything together in ByteArray.
    var headerBytes = saltBytes.concat(ivBytes).concat(cryptedTextBytes);
    //Now convert to WordArray.
    var cipherWords = this.byteArrayToWordArray(headerBytes);
    //Encode this to sent to server
    var encryptedString = CryptoJS.enc.Base64.stringify(cipherWords);
    return encryptedString;
  }

  decryptUsingAES(cipherText: string, plainTextKey: string): string | null {
    if (!cipherText || !cipherText.trim() || !plainTextKey || !plainTextKey.trim()) {
      return null;
    }

    var decodedWords = CryptoJS.enc.Base64.parse(cipherText);
    // Convert byteArray so it can be split
    var decodedBytes = this.wordArrayToByteArray(decodedWords);
    // Split the bytes into salt, iv and ciphertext
    var saltBytes = decodedBytes.slice(0, 32);
    var ivBytes = this.byteArrayToWordArray(decodedBytes.slice(32, 32 + 16));
    var cipherBytes = decodedBytes.slice(32 + 16)

    // Generate key from password and salt
    var key = this.generateKey(plainTextKey, this.byteArrayToWordArray(saltBytes));

    var cryptedWordsString = CryptoJS.enc.Base64.stringify(this.byteArrayToWordArray(cipherBytes));

    var decrypted = CryptoJS.AES.decrypt(cryptedWordsString, key, {
      padding: CryptoJS.pad.Pkcs7,
      mode: CryptoJS.mode.CBC,
      iv: ivBytes
    });

    return decrypted.toString(CryptoJS.enc.Utf8);
  }

  // RSA

  encryptUsingRSA(value: string): string | null {
    if (!value || !value.trim()) {
      return null;
    }
    const rsa = forge.pki.publicKeyFromPem(this.publicKey);
    return window.btoa(rsa.encrypt(value, 'RSA-OAEP')); // TOTO try CryptoJS.enc.Base64.stringify
  }

  // SHA256

  hashUsingSHA256(value: string): string | null {
    if (!value || !value.trim()) {
      return null;
    }

    return CryptoJS.enc.Base64.stringify(CryptoJS.SHA512(value));
  }

  // helping methods

  generateKey(password: string, salt: CryptoJS.lib.WordArray): string {
    return CryptoJS.PBKDF2(password, salt, {
      keySize: 256 / 32,
      iterations: 10000,
    });
  }

  //conversion methods

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

// aes and conversion methods from inspired from https://www.appsloveworld.com/csharp/100/310/encrypting-in-angular-and-decrypt-on-c
