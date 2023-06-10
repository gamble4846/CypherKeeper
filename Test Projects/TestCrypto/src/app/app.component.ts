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
    // var GeneratedPrivateKey = this.encryptMod.getPrivateKey();
    // var GeneratedPublicKey = this.encryptMod.getPublicKey();
    // console.log(GeneratedPrivateKey);
    // console.log(GeneratedPublicKey);

    var newPublicKey = "-----BEGIN PUBLIC KEY-----\nMIGeMA0GCSqGSIb3DQEBAQUAA4GMADCBiAKBgF6/IgmBiOIsvLUtGv85EtfzJaQy\ngu4mXhbnbipWzMpspgJ+ycpXDIjQkVWxVJ+iAkCjl0EZK5DLKT1a4Y7XzfHySImA\nhyXnZTuBZgFPo4lXZX228bg20Us8S45RoAvQJTXMNymYsWBS42YN9zTLvfmbbfH6\nvzqlYq7b3v9tbrzZAgMBAAE=\n-----END PUBLIC KEY-----"
    var newPrivateKey = "-----BEGIN RSA PRIVATE KEY-----\nMIICXAIBAAKBgF6/IgmBiOIsvLUtGv85EtfzJaQygu4mXhbnbipWzMpspgJ+ycpX\nDIjQkVWxVJ+iAkCjl0EZK5DLKT1a4Y7XzfHySImAhyXnZTuBZgFPo4lXZX228bg2\n0Us8S45RoAvQJTXMNymYsWBS42YN9zTLvfmbbfH6vzqlYq7b3v9tbrzZAgMBAAEC\ngYAhRuxDfnV/Sss0rxTuUzNlGYc89mi6EEu3q60rvbTL3AEqmzFwmcZTPz3sZQ4d\nyx8TaOG1AYwlnNVtMUBLlUxFohbPSfjisiSr1H17VrFEAQcZnlggQ6yGQ80W1c1D\n6DRK1GWStAEhpvsiSosrwNCXTm+EzZHDB2+OOL2s2K8t6QJBAK1It9sqI+1jCD8g\nwA53n44qDIqmp0ek841tXMByDdg/H0Xhz/Et/PWypTTmkQ5KFGRmplw86LhsCqTn\nYGjOJjsCQQCL+Scebrpz0iHlY7VGR+ND9ZcmFxOPrFq0XogEfRfSN1T0WFdBup9a\nd86ZE5g9hthvj+xWnKZN08R8lyfmfLP7AkEAhJzxL8YZRrQSfJVoxemgbyEZBgcy\nX+8KAUMfx5vBoqv0F/wPsoi6XaJBMrH9cw0YfBne2Ro4E+ODZd449xxFPwJAHK+Q\nE82PiqL67V70zJV4/vl644SBLsWcxTBGYd0dH6jVUUo1f7P2anz5Kyf34EwdWSe+\nwLVT48lNkFcOB09xNwJBAJURICez1t1Fy2SjF7aZIMLX67DJvzg+t3KEaF6uIdwR\nVqkO5TCznYoE/rmkYFe+I02ZrQW2CNr+X9CcCwaAd6E=\n-----END RSA PRIVATE KEY-----"
    // var newPublicKeyBAS64 = btoa(newPublicKey);
    // console.log("newPublicKeyBAS64", newPublicKeyBAS64);

    // var cypherText = this.RsaEncrypt("rohan", newPublicKey);
    // var decypherText = this.RsaDecrypt(cypherText,newPrivateKey);

    // console.log(cypherText, decypherText);

    // var x = "PublicMSI";
    // console.log(this.publicKey);
    // console.log(this.encryptMod.getPublicKey());

    // var y = this.RsaEncrypt(x, this.publicKey);
    // console.log(y);
    // console.log(this.encryptMod.getPublicKey());
    // console.log(this.parseJwt("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJMb2dpbkRhdGEiOiJ7XCJVc2VybmFtZVwiOlwiZ2FtYmxlNDg0NlwiLFwiUGFzc3dvcmRcIjpcIkIzV2JXUElqcERvbjV5b25lbjltYmZLVjdhaWJ6Z2pMYXVtcFFsVnp1V289XCJ9IiwiZXhwIjoxNzE3NzM3MDc5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3Ni8iLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzYvIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzYvIl0sIlNlcnZlciBEYXRhIjoiMnpQREkxT1d6eHpUd2tMTU9NY0t4Nk1GejdhdHNKYnJPcXZiRFduSFdTUnVrUFdyN24wM2FocUR6bUtoL2F1VFJvNFBDcmpDcW51YzE5Wk1BSGF6ejY5ZHFpMXNmcmdwZmtpNFVwaXBQcG11Ty9xSlpJL3NqTHJmOTFBWjlPVjlnYVR2YXdzYkt5eHYzbVA4alVQNFFSNWVZT1dxbDYzWTFhcFVxYWlvUDZ0QWRCczBBOTFZK2VmbXVVTjZjajduQi9aNlhxSHZ6VGFJUGtFMHgwRVhrN1RhT3pjS2Q4dW1xaDM3TjY3UFUxcUdpcWhGemlSTzBpUGFlSlpDb2p4UEMzbE5Ia0tjclRZbWhPa0RGTVBIcTQ4OXppMExISUdjMkM2bmlZK3hIL1ZnUXpEWnFmeXB6ZXFIUTdEcmloeStCcklvZUhuQWN3WU5aUHljbVBGbnhoclIzbXZTYTAzVFE1alpDdXczNE1wUmMrWHdUYkFwNyt5UWdueERxckNUemY4ZU1sUkV4WkdJK05rZGg2MnFmY2tkdTAxOWU1VlVGaTVZS3hjbUVCVlMrMnc0a3JCeTNqUnBKeUlaV2RHdEJlcWtFMmRyYWFWTkdzaEF2ZExZb3BNK3hqV28yMkJuUk95SUJlZkFWbm1Yd25RR0tMLzQ2b0JWNG96TU0zYTdXN0ZBVjJIRjMvY3B6bnFicGlSbk1FdkxocE1FYTZxMXFOeFVneDJWVDVGSGhPU0Mwb2RCTlhVYVA1RjBsSzE3VHAzb3ZOSFVSSHRsKzRFTzJYeHJGa0ZON2VZM1RuODdVRUFmaW1RWCtQNUJIQnB6TXZZY3VTQncxSzhmekN6diJ9.sc-L6kRCrNrYvuBHnAZx8SAlqpRH6mI69WIWHJHXm7Q"));

    var testArray: Array<string> = [
      "A5bHUhm7I7ciIasHviQw0nXzIUryNTxh0ufS0cUFtsIa1HnG7vf+ycjBgcnoWFKqlmhcTQlTMI9eWsofg4YiOsEK/1W9gZqdXqSxivJ2t63yMiQWyMwvurbbvtFp69s0+RfMqdQSumFTawnQ33Vxar3Ed4vXju88G8OXCs4D1mI=",
      "RY6rp6aZv/0u0olkE3KdsskzlmW5giue0qjPTvfAt+3PuXxXlVQanYwPgrDhxj88eWOqAycrURujUl7ZtTvxMUHouYM2hS5mF6tszF4AzQgsDPRn0A+I58BQrRUce4/teEeNOXSylz7/aKRdQK16qTWc0mjqA/OAMAkL8AQAUdA=",
      "V0znko88ykkGp+lh91VXL3L/LG4D8jYc/H75qewyHHfO9NMHngHuMThbq4Ewg+1d+qWCKuRwjQjbtBkm3KnSDK3peAfCsFXrQ29MMsMa3aFfioNkopJjrl0Vy43kmxVgAdGN7o/BvdacEtsnudBL9ZefwI9tXgYlIU12XiMaQIQ="
    ];

    var DecryptedString = this.GetDecryptedStringFromEncryptedArrayRSA(testArray, newPrivateKey);
    console.log(DecryptedString);
    console.log(JSON.parse(DecryptedString));
  }

  GetDecryptedStringFromEncryptedArrayRSA(EncryptedArray: Array<string>, newPrivateKey: string) {
    var DecryptedString: string = "";
    EncryptedArray.forEach(EncryptedString => {
      DecryptedString += this.RsaDecrypt(EncryptedString, newPrivateKey);
    });
    return DecryptedString;
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

  parseJwt(token: string) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
  }

  EncodeBase64(data:string){
    return btoa(data).toString();
  }

  DecodeBase64(data:string){
    return atob(data).toString();
  }

}
