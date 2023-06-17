import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tbIconsModel } from 'src/app/Models/tbIconsModel';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class TbIconsControllerService {
  constructor(private http: HttpClient) { }

  Get(Page:number = -1, ItemsPerPage:number = -1, OnlyNonDeleted:boolean = true){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbIcons/Get?page=${Page}&itemsPerPage=${ItemsPerPage}&onlyNonDeleted=${OnlyNonDeleted}`;
    return this.http.get(apiLink);
  }

  Add(model:tbIconsModel){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbIcons/Add`;
    return this.http.post(apiLink, model);
  }
}
