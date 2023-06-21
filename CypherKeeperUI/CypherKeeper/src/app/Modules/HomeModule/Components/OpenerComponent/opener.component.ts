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
import { NzContextMenuService, NzDropdownMenuComponent } from 'ng-zorro-antd/dropdown';
import { tbGroupsAddModel } from 'src/app/Models/tbGroupsAddModel';
import { tbIconsModel } from 'src/app/Models/tbIconsModel';
import { TbIconsControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-icons-controller.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { tbKeysModel } from 'src/app/Models/tbKeysModel';
import { TbKeysControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-keys-controller.service';
import { tbWebsitesModel } from 'src/app/Models/tbWebsitesModel';
import { TbWebsitesControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-websites-controller.service';
import { MixedControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/mixed-controller.service';

@Component({
  selector: 'app-opener',
  templateUrl: './opener.component.html',
  styleUrls: ['./opener.component.css']
})
export class OpenerComponent {
  AllGroups: Array<tbGroupsModel> = [];
  AllGroups_IM_Data: Array<MenuData> = [];
  MenuStylesConstant: Array<MenuStyles> = [];
  CurrentContextGroupMenu: MenuData | undefined;
  ShowCreateGroupDrawer: boolean = false;
  tbGroupsAddForm: tbGroupsAddModel = {
    name: '',
    parentGroupId: null,
    iconId: null
  };
  SelectIconModalIsVisible: boolean = false;
  SelectedIcon: tbIconsModel | undefined;
  AllIcons: Array<tbIconsModel> = [];
  RenameGroupModalIsVisible: boolean = false;
  RenamedGroupName: string = "";
  AllIcons_Completed: boolean = false;
  AllGroups_Completed: boolean = false;
  IsEditingIcon: boolean = false;
  IsKeyOpen: boolean = false;
  OpenedKey: tbKeysModel | null = null;
  SelectedGroup:tbGroupsModel | undefined;
  CurrentGroupKeys: Array<tbKeysModel> = [];
  AllWebsites:Array<tbWebsitesModel> = [];
  CurrentKeyContext:tbKeysModel | undefined;

  constructor(
    public _FormsService: FormsService,
    public _CommonService: CommonService,
    public _AuthService: AuthService,
    public _AdminControllerService: AdminControllerService,
    public _Router: Router,
    public _ImageControllerService: ImageControllerService,
    public _TbGroupsService: TbGroupsService,
    private nzContextMenuService: NzContextMenuService,
    public _TbIconsControllerService: TbIconsControllerService,
    private _NzModalService: NzModalService,
    private _TbKeysControllerService:TbKeysControllerService,
    private _TbWebsitesControllerService:TbWebsitesControllerService,
    private _MixedControllerService:MixedControllerService,
  ) { }

  ngOnInit(): void {
    this.MenuStylesConstant = CONSTANTS.MenuStylesConstant;
    this.UpdateGroups();
    this.UpdateAllIcons();
    this.UpdateAllWebsites();
  }

  UpdateAllWebsites(){
    this._TbWebsitesControllerService.Get().subscribe((response:any) => {
      if(response.code == 1){
        this.AllWebsites = response.document.records;
      }
    })
  }

  UpdateGroups() {
    this._TbGroupsService.Get().subscribe((response: any) => {
      console.log(response);
      if (response.code == 1) {
        this.AllGroups = response.document.records;
        this.AllGroups_Completed = true;
        if (this.AllGroups_Completed && this.AllIcons_Completed) { this.SetupGroupsMenuData(); }
      }
    })
  }

  UpdateAllIcons() {
    this._TbIconsControllerService.Get().subscribe((response: any) => {
      console.log(response);
      if (response.code == 1) {
        this.AllIcons = response.document.records;
        this.AllIcons_Completed = true;
        if (this.AllGroups_Completed && this.AllIcons_Completed) { this.SetupGroupsMenuData(); }
      }
    })
  }

  Get_Icon_By_WebSiteId(WebsiteId:string | null){
    if(WebsiteId){
      let Website:any = this.AllWebsites.find((x:tbWebsitesModel) => x.id == WebsiteId);
      if(Website){
        let Icon:any = this.AllIcons.find((x:tbIconsModel) => x.id == Website.iconId);
        if(Icon && Icon.link){
          return Icon.link;
        }
      }
    }
    return "https://i.imgur.com/inF7171.jpeg";
  }

  SetupGroupsMenuData() {
    let Old_AllGroups_IM_Data = structuredClone(this.AllGroups_IM_Data);

    let MenuGroupWithoutChildren: Array<MenuData> = [];

    this.AllGroups.forEach((Group: tbGroupsModel) => {
      let Menu: MenuData = {
        "Title": Group.Name,
        "IconType": IconType.Image,
        "Icon": this.GetIconLink(Group.IconId),
        "Children": [],
        "Id": Group.Id,
        "CustomData": Group,
        "IsOpen": false,
      };

      MenuGroupWithoutChildren.push(Menu);
    });

    this.AllGroups_IM_Data = MenuGroupWithoutChildren.filter((x: MenuData) => x.CustomData.ParentGroupId == null);
    MenuGroupWithoutChildren = MenuGroupWithoutChildren.filter((x: MenuData) => x.CustomData.ParentGroupId != null);

    this.AllGroups_IM_Data.sort((a, b) => (a.CustomData.ArrangePosition > b.CustomData.ArrangePosition) ? 1 : ((b.CustomData.ArrangePosition > a.CustomData.ArrangePosition) ? -1 : 0))

    this.AllGroups_IM_Data.forEach((GroupIM: MenuData) => {
      GroupIM.Children = this.GetChildrenForMenu(GroupIM, MenuGroupWithoutChildren);
      GroupIM.Children.sort((a, b) => (a.CustomData.ArrangePosition > b.CustomData.ArrangePosition) ? 1 : ((b.CustomData.ArrangePosition > a.CustomData.ArrangePosition) ? -1 : 0))
    });

    this.ReOpenMenuFromOldMenu(Old_AllGroups_IM_Data);
    console.log(this.AllGroups_IM_Data);
  }

  ReOpenMenuFromOldMenu(Old_AllGroups_IM_Data: Array<MenuData>) {
    let IdsAndOpen = this.GetIdsAndOpen(Old_AllGroups_IM_Data);
    IdsAndOpen = IdsAndOpen.filter((x: any) => x.isOpen);
    this._ReOpenMenuFromOldMenu(IdsAndOpen, this.AllGroups_IM_Data)
    console.log(IdsAndOpen);
  }

  _ReOpenMenuFromOldMenu(IdsAndOpen: Array<any>, AllGroups_IM_Data: Array<MenuData>) {
    AllGroups_IM_Data.forEach((group: MenuData) => {
      let currentIdOpen = IdsAndOpen.find((x: any) => x.id == group.Id);
      if (currentIdOpen) {
        group.IsOpen = true;
      }
      this._ReOpenMenuFromOldMenu(IdsAndOpen, group.Children);
    });
  }

  GetIdsAndOpen(Old_AllGroups_IM_Data: Array<MenuData>) {
    let toReturn: Array<any> = [];
    Old_AllGroups_IM_Data.forEach((GroupData: MenuData) => {
      toReturn.push({
        id: GroupData.Id,
        isOpen: GroupData.IsOpen
      });
      toReturn = toReturn.concat(this.GetIdsAndOpen(GroupData.Children));
    });
    return toReturn;
  }

  GetIconLink(IconId: any) {
    if (IconId) {
      var CurrentIconData = this.AllIcons.find(x => x.id == IconId);
      if (CurrentIconData) {
        return CurrentIconData.link;
      }
    }
    return "";
  }

  GetChildrenForMenu(GroupIM: MenuData, MenuGroupWithoutChildren: Array<MenuData>) {
    var CurrentChildren = MenuGroupWithoutChildren.filter((x: MenuData) => x.CustomData.ParentGroupId == GroupIM.Id);
    CurrentChildren.forEach((Child: MenuData) => {
      Child.Children = this.GetChildrenForMenu(Child, MenuGroupWithoutChildren);
      Child.Children.sort((a, b) => (a.CustomData.ArrangePosition > b.CustomData.ArrangePosition) ? 1 : ((b.CustomData.ArrangePosition > a.CustomData.ArrangePosition) ? -1 : 0))
    });
    return CurrentChildren;
  }

  _MenuItemOnContextMenu(event: any, ContectMenuGroup: NzDropdownMenuComponent) {
    this.CurrentContextGroupMenu = event.MenuModel;
    let _MouseEvents: MouseEvent = event.EventData;
    this.nzContextMenuService.create(_MouseEvents, ContectMenuGroup);
  }

  ResetGroupsAddForm() {
    this.tbGroupsAddForm = {
      name: '',
      parentGroupId: null,
      iconId: null
    };
    this.SelectedIcon = undefined;
  }

  _ShowCreateGroupDrawer() {
    this.ShowCreateGroupDrawer = true;
    this.nzContextMenuService.close();
  }

  _ShowCreateGroupDrawerForNullParent() {
    let Empty: MenuData = {
      Title: 'NULL',
      Children: [],
      CustomData: {
        Id: null,
      }
    }
    this.CurrentContextGroupMenu = Empty;
    this.ShowCreateGroupDrawer = true;

  }

  OnCloseCreateGroupDrawer() {
    this.ShowCreateGroupDrawer = false;
    this.ResetGroupsAddForm();
  }

  OpenSelectIconModal() {
    this.SelectIconModalIsVisible = true;
  }

  SelectIconModalIsVisibleHandleCancel() {
    this.SelectIconModalIsVisible = false;
  }

  IconSelected(IconsModel: tbIconsModel | undefined) {
    let IconId: string | null;
    if (IconsModel == undefined) {
      IconId = null;
    }
    else {
      IconId = IconsModel.id;
    }

    if (this.IsEditingIcon) {
      this._TbGroupsService.ChangeIcon(this.CurrentContextGroupMenu?.CustomData.Id, IconId).subscribe((response: any) => {
        if (response.code == 1) {
          this.SelectIconModalIsVisibleHandleCancel();
          this.AllIcons_Completed = true;
          this.AllGroups_Completed = false;
          this.UpdateGroups();
          this.IsEditingIcon = false;
        }
      })
    }
    else {
      this.SelectedIcon = IconsModel;
      this.tbGroupsAddForm.iconId = IconId;
      this.SelectIconModalIsVisibleHandleCancel();
    }
  }

  SubmitCreateGroupDrawer() {
    this.tbGroupsAddForm.parentGroupId = this.CurrentContextGroupMenu?.CustomData.Id;
    this._TbGroupsService.Add(this.tbGroupsAddForm).subscribe((response: any) => {
      if (response.code == 1) {
        this.AllIcons_Completed = false;
        this.AllGroups_Completed = false;

        this.UpdateGroups();
        this.UpdateAllIcons();
        this.OnCloseCreateGroupDrawer();
      }
    })
  }

  DeleteGroup() {
    this._NzModalService.confirm({
      nzTitle: 'Are you sure delete this Group (' + this.CurrentContextGroupMenu?.Title + ')?',
      nzContent: '<b style="color: red;">Deleting this group makes all the children group inaccessible, you can restore this group to access all the childrens</b>',
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => {
        this._TbGroupsService.Delete(this.CurrentContextGroupMenu?.CustomData.Id).subscribe((response: any) => {
          if (response.code == 1) {
            this.AllIcons_Completed = true;
            this.AllGroups_Completed = false;
            this.UpdateGroups();
          }
        })
      },
      nzCancelText: 'No',
      nzOnCancel: () => console.log('Cancel')
    });
  }

  RenameGroupShowModal() {
    this.RenamedGroupName = this.CurrentContextGroupMenu?.Title || "";
    this.RenameGroupModalIsVisible = true;
  }

  RenameGroupModalHandleCancel() {
    this.RenameGroupModalIsVisible = false;
    this.CurrentContextGroupMenu = undefined;
  }

  RenameGroupModalHandleOk() {
    if (this.RenamedGroupName && this.CurrentContextGroupMenu?.Id) {
      this._TbGroupsService.Rename(this.CurrentContextGroupMenu?.Id, this.RenamedGroupName).subscribe((response: any) => {
        if (response.code == 1) {
          this.RenameGroupModalIsVisible = false;
          this.CurrentContextGroupMenu = undefined;
          this.AllIcons_Completed = true;
          this.AllGroups_Completed = false;
          this.UpdateGroups();
        }
      })
    }
  }

  SelectIconForAdd() {
    this.IsEditingIcon = false;
    this.OpenSelectIconModal();
  }

  SelectIconForEdit() {
    this.IsEditingIcon = true;
    this.OpenSelectIconModal();
  }

  GroupSelected(SelectedMenuData: MenuData) {
    this.SelectedGroup = SelectedMenuData.CustomData;
    this.GetKeysForGroup();
  }

  OpenKey(Key:tbKeysModel | null){
    this.OpenedKey = Key;
    this.IsKeyOpen = true;
  }

  CloseKey(){
    this.OpenedKey = null;
    this.IsKeyOpen = false;
    this.GetKeysForGroup();
  }

  GetKeysForGroup(){
    if(this.SelectedGroup){
      this._TbKeysControllerService.GetByGroupId(this.SelectedGroup.Id).subscribe((response:any) => {
        if(response.code == 1){
          this.CurrentGroupKeys = response.document;
        }
        console.log(this.CurrentGroupKeys);
      })
    }
  }

  KeyContextMenu(event:MouseEvent, key:tbKeysModel, Menu:NzDropdownMenuComponent){
    this.CurrentKeyContext = key;
    this.nzContextMenuService.create(event, Menu);
  }

  DublicateKey_Context(){
    if(this.CurrentKeyContext){
      this._MixedControllerService.DublicateKey(this.CurrentKeyContext.Id).subscribe((response:any) => {
        if(response.code == 1){
          this.GetKeysForGroup();
        }
      })
    }
  }
}
