import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { tbIconsModel } from 'src/app/Models/tbIconsModel';
import { tbWebsitesModel, tbWebsitesModel_ToAdd } from 'src/app/Models/tbWebsitesModel';
import { TbWebsitesControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-websites-controller.service';
import { AppInitializerService } from 'src/app/Modules/SharedModule/Services/OtherServices/app-initializer.service';

@Component({
  selector: 'web-site-selector',
  templateUrl: './web-site-selector.component.html',
  styleUrls: ['./web-site-selector.component.css']
})
export class WebSiteSelectorComponent {
  @Input() _FormGroup:FormGroup | undefined;

  SelectedWebsiteID:string = "";
  AllWebsites:Array<tbWebsitesModel> = [];
  IsAddWebsiteModalVisible:boolean = false;
  NewWebsite:tbWebsitesModel_ToAdd = {
    Name: '',
    Link: null,
    IconId: null
  };
  SelectedIcon: tbIconsModel | undefined;
  SelectIconModalIsVisible: boolean = false;

  constructor(
    private _AppInitializerService:AppInitializerService,
    private _TbWebsitesControllerService:TbWebsitesControllerService,
  ) {}

  ngOnInit(): void {
    this.SelectedWebsiteID = this._FormGroup?.value['WebsiteId'];
    this._TbWebsitesControllerService.Get().subscribe((response:any) => {
      if(response.code == 1){
        this.AllWebsites = response.document.records;
        console.log(this.AllWebsites);
      }
    })
  }

  WebsiteChanged(){
    console.log(this.SelectedWebsiteID);
    if(this._FormGroup){
      this._FormGroup.patchValue({
        "WebsiteId": this.SelectedWebsiteID,
      })
    }
  }

  AddWebsiteModalHandleCancel(){
    this.IsAddWebsiteModalVisible = false;
  }

  AddWebsiteModalHandleOk(){
    
    console.log(this.NewWebsite);
    this._TbWebsitesControllerService.Add(this.NewWebsite).subscribe((response:any) => {
      if(response.code == 1){
        console.log(response);
        this.IsAddWebsiteModalVisible = false;
      }
    })
  }

  ShowAddWebsiteModal(){
    this.IsAddWebsiteModalVisible = true;
    this.NewWebsite = {
      Name: '',
      Link: null,
      IconId: null
    };
    this.SelectedIcon = undefined;
  }

  OpenSelectIconModal() {
    this.SelectIconModalIsVisible = true;
  }

  SelectIconModalIsVisibleHandleCancel(){
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

    this.SelectedIcon = IconsModel;
    this.NewWebsite.IconId = IconId;
    this.SelectIconModalIsVisibleHandleCancel();
  }
}
