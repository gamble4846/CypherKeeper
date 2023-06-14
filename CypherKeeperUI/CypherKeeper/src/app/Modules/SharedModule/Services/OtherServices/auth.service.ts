import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { TokenModel } from 'src/app/Models/TokenModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _CS:CommonService) { }

  AddJWTUserTokenToLocal(token:string){
    localStorage.setItem("UserToken", token);
  }

  AddJWTSelectedServerToken(token:string){
    sessionStorage.setItem("SelectedServerToken", token);
  }

  SetRSAPrivateKeyForAPI(PrivateKey:string){
    var Base64String = this._CS.EncodeBase64(PrivateKey);
    localStorage.setItem("RSAPrivateKeyForAPI", Base64String);
  }

  GetRSAPrivateKeyForAPI(){
    var Base64String = localStorage.getItem("RSAPrivateKeyForAPI") || "";
    return this._CS.DecodeBase64(Base64String);
  }

  GetJWTToken(){
    if(sessionStorage.getItem("SelectedServerToken")){
      return sessionStorage.getItem("SelectedServerToken");
    }
    return localStorage.getItem("UserToken");
  }

  GetJWTData(){
    var UserToken = this.GetJWTToken();
    if(UserToken){
      var DeryptedToken = this._CS.ParseJwt(UserToken);
      var TokenData:TokenModel = {
        LoginData: DeryptedToken.LoginData,
        exp: DeryptedToken.exp,
        iss: DeryptedToken.iss,
        aud: DeryptedToken.aud,
        ServerData: DeryptedToken.ServerData
      };

      return TokenData;
    }
    return null;
  }
}
