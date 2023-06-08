import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { TokkenModel } from 'src/app/Models/TokkenModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _CS:CommonService) { }

  AddJWTUserTokenToLocal(token:string){
    localStorage.setItem("UserToken", token);
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
      var TokenData:TokkenModel = {
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

  AddJWTSelectedServerToken(token:string){
    sessionStorage.setItem("SelectedServerToken", token);
  }
}
