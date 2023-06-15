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

@Injectable()
export class DecryptInterceptor implements HttpInterceptor {

  constructor(private _CS:CommonService) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (req.headers.has(CONSTANTS.InterceptorSkipErrorHeader)) {
      const headers = req.headers.delete(CONSTANTS.InterceptorSkipErrorHeader);
      return next.handle(req.clone({ headers }));
    }

    return next.handle(req).pipe(
      tap((data:any) => {
        try{
          console.log(data);
        }
        catch(ex){}
      }),
    );
  }
}
