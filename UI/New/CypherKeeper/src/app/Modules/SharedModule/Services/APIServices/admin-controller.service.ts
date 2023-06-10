import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginModel } from 'src/app/Models/LoginModel';
import { RegisterModel } from 'src/app/Models/RegisterModel';
import { ServerViewModel } from 'src/app/Models/ServerModels';
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

  Login(LoginData:LoginModel){
    let apiLink = `${CONSTANTS.APIUrl}/api/Admin/Login`;
    return this.http.post(apiLink,LoginData);
  }

  AddServer(model:ServerViewModel){
    let apiLink = `${CONSTANTS.APIUrl}/api/Server/Add`;
    return this.http.post(apiLink,model);
  }

  GetServers(){
    let apiLink = `${CONSTANTS.APIUrl}/api/Servers/Get`;
    return this.http.get(apiLink);
  }

  SelectServer(){
    
  }
}
