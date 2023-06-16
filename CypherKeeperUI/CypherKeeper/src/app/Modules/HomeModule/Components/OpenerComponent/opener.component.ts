import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IconType, MenuData, MenuStyles } from 'infinite-menu';
import { tbGroupsModel } from 'src/app/Models/tbGroupsModel';
import { AdminControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/admin-controller.service';
import { ImageControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/image-controller.service';
import { TbGroupsService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-groups.service';
import { AuthService } from 'src/app/Modules/SharedModule/Services/OtherServices/auth.service';
import { CommonService } from 'src/app/Modules/SharedModule/Services/OtherServices/common.service';
import { FormsService } from 'src/app/Modules/SharedModule/Services/OtherServices/forms.service';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';

@Component({
  selector: 'app-opener',
  templateUrl: './opener.component.html',
  styleUrls: ['./opener.component.css']
})
export class OpenerComponent {
  AllGroups:Array<tbGroupsModel> = [];
  AllGroups_IM_Data:Array<MenuData> = [];
  MenuStylesConstant:Array<MenuStyles>  = [];

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
    this.MenuStylesConstant = CONSTANTS.MenuStylesConstant;
    this.UpdateGroups();
  }

  UpdateGroups(){
    this._TbGroupsService.Get().subscribe((response:any) => {
      if(response.code == 1){
        this.AllGroups = response.document.records;
        this.SetupGroupsMenuData();
      }
    })
  }

  SetupGroupsMenuData(){
    let MenuGroupWithoutChildren:Array<MenuData> = [];

    this.AllGroups.forEach((Group:tbGroupsModel) => {
      let Menu:MenuData = {
        "Title": Group.Name,
        "IconType": IconType.Image,
        "Icon": Group.IconId?.toString(),
        "Children": [],
        "Id": Group.Id,
        "CustomData": Group
      };

      MenuGroupWithoutChildren.push(Menu);
    });

    this.AllGroups_IM_Data = MenuGroupWithoutChildren.filter((x:MenuData) => x.CustomData.ParentGroupId == null);
    MenuGroupWithoutChildren = MenuGroupWithoutChildren.filter((x:MenuData) => x.CustomData.ParentGroupId != null);

    this.AllGroups_IM_Data.forEach((GroupIM:MenuData) => {
      GroupIM.Children = this.GetChildrenForMenu(GroupIM,MenuGroupWithoutChildren);
    });
  }
  
  GetChildrenForMenu(GroupIM:MenuData,MenuGroupWithoutChildren:Array<MenuData>){
    var CurrentChildren = MenuGroupWithoutChildren.filter((x:MenuData) => x.CustomData.ParentGroupId == GroupIM.Id);
    CurrentChildren.forEach((Child:MenuData) => {
      Child.Children = this.GetChildrenForMenu(Child, MenuGroupWithoutChildren);
    });
    return CurrentChildren;
  }

  _MenuItemOnContextMenu(event:any){
    console.log(event);
  }
}
