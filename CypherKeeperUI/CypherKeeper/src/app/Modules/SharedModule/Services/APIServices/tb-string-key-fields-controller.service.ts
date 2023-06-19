import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class TbStringKeyFieldsControllerService {

  constructor(private http: HttpClient) { }

  GetByKeyId(KeyId:string){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbStringKeyFields/GetByKeyId/${KeyId}`;
    return this.http.get(apiLink);
  }
}
