import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tbGroupsAddModel } from 'src/app/Models/tbGroupsAddModel';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Injectable({
  providedIn: 'root'
})
export class TbGroupsService {

  constructor(private http: HttpClient) { }

  Get(Page:number = -1, ItemsPerPage:number = -1, OnlyNonDeleted:boolean = true){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbGroups/Get?page=${Page}&itemsPerPage=${ItemsPerPage}&onlyNonDeleted=${OnlyNonDeleted}`;
    return this.http.get(apiLink);
  }

  Add(model:tbGroupsAddModel){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbGroups/Add`;
    return this.http.post(apiLink,model);
  }

  Delete(Id:string){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbGroups/Delete/${Id}`;
    return this.http.delete(apiLink);
  }

  Rename(Id:string, NewName:string){
    let apiLink = `${CONSTANTS.APIUrl}/api/TbGroups/Rename/${Id}/${NewName}`;
    return this.http.patch(apiLink, {});
  }
}
