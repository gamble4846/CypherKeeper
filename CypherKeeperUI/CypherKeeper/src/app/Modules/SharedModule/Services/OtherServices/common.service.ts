import { Injectable } from '@angular/core';
import * as JsEncryptModule from 'jsencrypt';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  encryptMod: any;

  constructor(

  ) {
    this.encryptMod = new JsEncryptModule.JSEncrypt();
  }

  showMessage(type: string, message: string): void {
    console.log(type, message);
  }

  ParseJwt(token: string) {
    try {
      var base64Url = token.split('.')[1];
      var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));

      return JSON.parse(jsonPayload);
    }
    catch {
      return null;
    }

  }

  RsaEncrypt(data: string, key: string) {
    this.encryptMod.setPublicKey(key);
    var cypherText = this.encryptMod.encrypt(data);
    return cypherText;
  }

  RsaDecrypt(data: string, key: string) {
    this.encryptMod.setPrivateKey(key);
    var decypherText = this.encryptMod.decrypt(data);
    return decypherText;
  }

  GenerateRSAPairKeys() {
    var encryptMod = new JsEncryptModule.JSEncrypt();
    var PrivateKey = encryptMod.getPrivateKey();
    var PublicKey = encryptMod.getPublicKey();
    return {
      "PrivateKey": PrivateKey,
      "PublicKey": PublicKey
    }
  }

  EncodeBase64(data: string) {
    return btoa(data).toString();
  }

  DecodeBase64(data: string) {
    return atob(data).toString();
  }

  LogSomething(data: any) {
    console.log(data);
  }

  GetDecryptedStringFromEncryptedArrayRSA(EncryptedArray: Array<string>, PrivateKey: string) {
    var DecryptedString: string = "";
    EncryptedArray.forEach(EncryptedString => {
      DecryptedString += this.RsaDecrypt(EncryptedString, PrivateKey);
    });
    return DecryptedString;
  }

  GroupBy(xs: any, key: any) {
    return xs.reduce((rv: any, x: any) => {
      (rv[x[key]] = rv[x[key]] || []).push(x);
      return rv;
    }, {});
  }

  GenerateUUID() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16;//random number between 0 and 16
      if (d > 0) {//Use timestamp until depleted
        r = (d + r) % 16 | 0;
        d = Math.floor(d / 16);
      } else {//Use microseconds since page-load if supported
        r = (d2 + r) % 16 | 0;
        d2 = Math.floor(d2 / 16);
      }
      return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
  }
}
