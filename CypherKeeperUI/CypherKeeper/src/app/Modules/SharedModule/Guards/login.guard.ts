import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonService } from '../Services/OtherServices/common.service';
import { TokenModel } from 'src/app/Models/TokenModel';
import { AuthService } from '../Services/OtherServices/auth.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {
  
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
      return true;
    }
    else{
      this._Router.navigateByUrl("/Auth/Login");
      return false;
    }
  }
  
}
