import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SavedKeyModel } from 'src/app/Models/SavedKeyModel';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class MixedControllerService {

  constructor(private http: HttpClient) { }

  SaveKey(model:SavedKeyModel){
    let apiLink = `${CONSTANTS.APIUrl}/api/Mixed/SaveKey`;
    return this.http.post(apiLink, model);
  }

  GetKeyHistory(KeyId:string){
    let apiLink = `${CONSTANTS.APIUrl}/api/Mixed/KeyHistory/${KeyId}`;
    return this.http.get(apiLink);
  }

  DublicateKey(KeyId:string){
    let apiLink = `${CONSTANTS.APIUrl}/api/Mixed/DublicateKey/${KeyId}`;
    return this.http.get(apiLink);
  }
}
