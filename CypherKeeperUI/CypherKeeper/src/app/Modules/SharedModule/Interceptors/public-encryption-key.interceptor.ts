import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';
import { AuthService } from '../Services/OtherServices/auth.service';
import { AppInitializerService } from '../Services/OtherServices/app-initializer.service';
import { CommonService } from '../Services/OtherServices/common.service';

@Injectable()
export class PublicEncryptionKeyInterceptor implements HttpInterceptor {

  constructor(
    private _AuthService:AuthService,
    private _AppInitializerService:AppInitializerService,
    private _CommonService:CommonService,
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.headers.has(CONSTANTS.InterceptorSkipPublicKeyHeader)) {
      const headers = req.headers.delete(CONSTANTS.InterceptorSkipPublicKeyHeader);
      return next.handle(req.clone({ headers }));
    }

    let tokenizedReq = req.clone({
      setHeaders: {
        'PublicEncryptionKey': this._CommonService.EncodeBase64(this._AppInitializerService.publicKey)
      }
    });

    return next.handle(tokenizedReq);
  }
}
