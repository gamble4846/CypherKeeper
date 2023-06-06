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
    var x = "rohan";
    console.log(this.publicKey);
    var y = this.RsaEncrypt(x, this.publicKey);
    console.log(y);
  }

  RsaEncrypt(data:string, key:string)
  {
    this.encryptMod.setPublicKey(key);
    var cypherText = this.encryptMod.encrypt(data);
    console.log("encryptedText-",cypherText);
  }
}
