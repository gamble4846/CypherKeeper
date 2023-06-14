import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonService } from '../Services/OtherServices/common.service';
import { AuthService } from '../Services/OtherServices/auth.service';
import { TokenModel } from 'src/app/Models/TokenModel';

@Injectable({
  providedIn: 'root'
})
export class ServerGuard implements CanActivate {

  constructor(
    private _CommonService:CommonService,
    private _AuthService:AuthService,
    public _Router: Router,
  ){}
  
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    
    var TokenData:TokenModel | null = this._AuthService.GetJWTData();

    if(TokenData){
      if(TokenData.ServerData){
        return true;
      }
      else{
        return false;
      }
    }
    else{
      this._Router.navigateByUrl("/Auth/Login");
      return false;
    }
  }
  
}
