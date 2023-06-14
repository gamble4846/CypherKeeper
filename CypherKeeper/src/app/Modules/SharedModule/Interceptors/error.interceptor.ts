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
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private _CS:CommonService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.headers.has(CONSTANTS.InterceptorSkipErrorHeader)) {
      const headers = req.headers.delete(CONSTANTS.InterceptorSkipErrorHeader);
      return next.handle(req.clone({ headers }));
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        console.log(error);
        if(error.status == 0 && error.message.includes("Http failure response for")){
          this._CS.showMessage("error","API Offline")
        }
        else if(error.status == 403){
          this._CS.showMessage("error","You are not Authorized to perform this operation")
        }
        else{
          this._CS.showMessage("error","API Error")
        }

        let errorObj:any = {
          message: "Error Occured",
          error: error
        }

        return throwError(errorObj);
      })
    ).pipe(
      tap((data:any) => {
        try{
          if(data.body.code == 0){
            this._CS.showMessage("error",data);
            console.log(data, "API ERROR", req.urlWithParams);
          }
        }
        catch(ex){}
      }),
    );
  }
}
