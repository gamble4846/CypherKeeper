import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class TbKeysControllerService {

  constructor(private http: HttpClient) { }

  GetByGroupId(groupId:string){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbKeys/GetByGroupId/${groupId}`;
    return this.http.get(apiLink);
  }
}
