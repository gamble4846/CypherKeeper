import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tbWebsitesModel_ToAdd } from 'src/app/Models/tbWebsitesModel';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class TbWebsitesControllerService {
  constructor(private http: HttpClient) { }

  Get(Page:number = -1, ItemsPerPage:number = -1, OnlyNonDeleted:boolean = true, OrderBySTR:string = ""){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbWebsites/Get?page=${Page}&itemsPerPage=${ItemsPerPage}&orderBy=${OrderBySTR}&onlyNonDeleted=${OnlyNonDeleted}`;
    return this.http.get(apiLink);
  }

  Add(model:tbWebsitesModel_ToAdd){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbWebsites/Add`;
    return this.http.post(apiLink, model);
  }
}
