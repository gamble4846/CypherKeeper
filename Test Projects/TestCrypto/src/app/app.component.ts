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

    console.log(this.parseJwt("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJMb2dpbkRhdGEiOiJ7XCJVc2VybmFtZVwiOlwiZ2FtYmxlNDg0NlwiLFwiUGFzc3dvcmRcIjpcIkIzV2JXUElqcERvbjV5b25lbjltYmZLVjdhaWJ6Z2pMYXVtcFFsVnp1V289XCJ9IiwiZXhwIjoxNzE3NzM3MDc5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3Ni8iLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzYvIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzYvIl0sIlNlcnZlciBEYXRhIjoiMnpQREkxT1d6eHpUd2tMTU9NY0t4Nk1GejdhdHNKYnJPcXZiRFduSFdTUnVrUFdyN24wM2FocUR6bUtoL2F1VFJvNFBDcmpDcW51YzE5Wk1BSGF6ejY5ZHFpMXNmcmdwZmtpNFVwaXBQcG11Ty9xSlpJL3NqTHJmOTFBWjlPVjlnYVR2YXdzYkt5eHYzbVA4alVQNFFSNWVZT1dxbDYzWTFhcFVxYWlvUDZ0QWRCczBBOTFZK2VmbXVVTjZjajduQi9aNlhxSHZ6VGFJUGtFMHgwRVhrN1RhT3pjS2Q4dW1xaDM3TjY3UFUxcUdpcWhGemlSTzBpUGFlSlpDb2p4UEMzbE5Ia0tjclRZbWhPa0RGTVBIcTQ4OXppMExISUdjMkM2bmlZK3hIL1ZnUXpEWnFmeXB6ZXFIUTdEcmloeStCcklvZUhuQWN3WU5aUHljbVBGbnhoclIzbXZTYTAzVFE1alpDdXczNE1wUmMrWHdUYkFwNyt5UWdueERxckNUemY4ZU1sUkV4WkdJK05rZGg2MnFmY2tkdTAxOWU1VlVGaTVZS3hjbUVCVlMrMnc0a3JCeTNqUnBKeUlaV2RHdEJlcWtFMmRyYWFWTkdzaEF2ZExZb3BNK3hqV28yMkJuUk95SUJlZkFWbm1Yd25RR0tMLzQ2b0JWNG96TU0zYTdXN0ZBVjJIRjMvY3B6bnFicGlSbk1FdkxocE1FYTZxMXFOeFVneDJWVDVGSGhPU0Mwb2RCTlhVYVA1RjBsSzE3VHAzb3ZOSFVSSHRsKzRFTzJYeHJGa0ZON2VZM1RuODdVRUFmaW1RWCtQNUJIQnB6TXZZY3VTQncxSzhmekN6diJ9.sc-L6kRCrNrYvuBHnAZx8SAlqpRH6mI69WIWHJHXm7Q"));
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
