import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MenuData } from 'infinite-menu';
import { tbGroupsModel } from 'src/app/Models/tbGroupsModel';
import { AdminControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/admin-controller.service';
import { ImageControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/image-controller.service';
import { TbGroupsService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-groups.service';
import { AuthService } from 'src/app/Modules/SharedModule/Services/OtherServices/auth.service';
import { CommonService } from 'src/app/Modules/SharedModule/Services/OtherServices/common.service';
import { FormsService } from 'src/app/Modules/SharedModule/Services/OtherServices/forms.service';

@Component({
  selector: 'app-opener',
  templateUrl: './opener.component.html',
  styleUrls: ['./opener.component.css']
})
export class OpenerComponent {
  AllGroups:Array<tbGroupsModel> = [];
  AllGroups_IM_Data:Array<MenuData> =[];

  constructor(
    public _FormsService:FormsService,
    public _CommonService:CommonService,
    public _AuthService:AuthService,
    public _AdminControllerService:AdminControllerService,
    public _Router: Router,
    public _ImageControllerService:ImageControllerService,
    public _TbGroupsService:TbGroupsService,
  ) { }

  ngOnInit(): void {
    this.UpdateGroups();
  }

  UpdateGroups(){
    this._TbGroupsService.Get().subscribe((response:any) => {
      if(response.code == 1){
        this.AllGroups = response.document.records;
        this.SetupGroupsMenuData();
      }
      console.log(this.AllGroups);
    })
  }

  SetupGroupsMenuData(){
    let AllGroupLoc = structuredClone(this.AllGroups);
  }
}
