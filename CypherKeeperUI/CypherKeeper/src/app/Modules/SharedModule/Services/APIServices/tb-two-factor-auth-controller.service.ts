import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class TbTwoFactorAuthControllerService {

  constructor(private http: HttpClient) { }

  Get(Page:number = -1, ItemsPerPage:number = -1, OnlyNonDeleted:boolean = true){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbTwoFactorAuth/Get?page=${Page}&itemsPerPage=${ItemsPerPage}&onlyNonDeleted=${OnlyNonDeleted}`;
    return this.http.get(apiLink);
  }
}
