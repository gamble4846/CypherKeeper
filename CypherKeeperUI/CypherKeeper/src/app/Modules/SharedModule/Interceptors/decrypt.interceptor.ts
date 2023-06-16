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
import { CommonService } from '../Services/OtherServices/common.service';
import { AuthService } from '../Services/OtherServices/auth.service';

@Injectable()
export class DecryptInterceptor implements HttpInterceptor {

  constructor(
    private _CommonService:CommonService,
    private _AuthService:AuthService,
  ) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (req.headers.has(CONSTANTS.InterceptorSkipErrorHeader)) {
      const headers = req.headers.delete(CONSTANTS.InterceptorSkipErrorHeader);
      return next.handle(req.clone({ headers }));
    }

    return next.handle(req).pipe(
      tap((data:any) => {
        try{
          if(data.body.isEncrypted){
            let DecryptKey = this._AuthService.GetRSAPrivateKeyForAPI();
            let DecryptedString = this._CommonService.GetDecryptedStringFromEncryptedArrayRSA(data.body.document,DecryptKey);
            data.body.document = JSON.parse(DecryptedString);
            return data.body;
          }
        }
        catch(ex){}
      }),
    );
  }
}
