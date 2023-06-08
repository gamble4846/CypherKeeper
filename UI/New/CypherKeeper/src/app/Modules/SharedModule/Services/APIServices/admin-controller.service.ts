import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterModel } from 'src/app/Models/RegisterModel';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class AdminControllerService {

  constructor(private http: HttpClient) { }

  Register(RegisterData:RegisterModel){
    let apiLink = `${CONSTANTS.APIUrl}/api/Admin/Register`;
    return this.http.post(apiLink,RegisterData);
  }
}
