import { Component } from '@angular/core';
import * as JsEncryptModule from 'jsencrypt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TestCrypto';
  publicKey = `-----BEGIN PUBLIC KEY-----
  MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC4ZpfUUHimSe1Z+9BVBIN1CG9i
  LozZJt27tZMkmFeXqdxHcfKPBamD++pSVi9WpBL/hyuWpxNpPWFbRB6uexCQfQfp
  2xyWPFkanczRiv4nuDsduPUve+RNhQtg8hG+mOt+3NIqW7gOK5/u57XQKv3wJiVM
  KkzsaePTT5K+inuJvQIDAQAB
  -----END PUBLIC KEY-----`;
  encryptMod: any;

  constructor() {
    this.encryptMod = new JsEncryptModule.JSEncrypt();
  }

  ngOnInit(): void {
    var x = "PublicMSI";
    console.log(this.publicKey);
    var y = this.RsaEncrypt(x, this.publicKey);
    console.log(y);

    console.log(this.parseJwt("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJMb2dpbkRhdGEiOiJ7XCJVc2VybmFtZVwiOlwiZ2FtYmxlNDg0NlwiLFwiUGFzc3dvcmRcIjpcIjE4MTU4MTE0XCJ9IiwiZXhwIjoxNzE3Njc3MTI3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3Ni8iLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzYvIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzYvIl0sIlNlcnZlciBEYXRhIjoie1wiR1VJRFNlcnZlclwiOlwiYmRiN2JhNjgtMzlmMi00ZTVmLTk0MjAtNzllNWE4MjljYjgzXCIsXCJTZXJ2ZXJOYW1lXCI6XCJQdWJsaWNNU0lcIixcIkRhdGFiYXNlVHlwZVwiOlwiU1FMU2VydmVyXCIsXCJDb25uZWN0aW9uU3RyaW5nXCI6XCJTZXJ2ZXI9dGNwOmxvbmctY29pbi5hdC5wbHkuZ2csMTA3MDM7RGF0YWJhc2U9Q3lwaGVyS2VlcGVyO1VzZXIgSWQ9c2E7UGFzc3dvcmQ9MTgxNTgxMTQ7XCIsXCJLZXlcIjpcIlB1YmxpY01TSVwifSJ9.Zy68lNbdrOi__oeO_N98ZwKlQFHPeRnC5TqUgQySPag"));
  }

  RsaEncrypt(data:string, key:string)
  {
    this.encryptMod.setPublicKey(key);
    var cypherText = this.encryptMod.encrypt(data);
    console.log("encryptedText-",cypherText);
  }

  parseJwt (token:string) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

}
