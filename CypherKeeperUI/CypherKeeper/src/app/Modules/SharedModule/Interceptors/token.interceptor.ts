import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, tap, throwError } from 'rxjs';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';
import { AuthService } from '../Services/OtherServices/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private _AuthService:AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.headers.has(CONSTANTS.InterceptorSkipTokkenHeader)) {
      const headers = req.headers.delete(CONSTANTS.InterceptorSkipTokkenHeader);
      return next.handle(req.clone({ headers }));
    }

    let tokenizedReq = req.clone({
      setHeaders: {
        Authorization: 'Bearer ' + this._AuthService.GetJWTToken()
      }
    });

    console.log(tokenizedReq);
    return next.handle(tokenizedReq);
  }
}
