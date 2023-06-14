import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SelectServerModel, Server, ServerViewModel } from 'src/app/Models/ServerModels';
import { AdminControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/admin-controller.service';
import { ImageControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/image-controller.service';
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

  ShowImageSelector:boolean = false;
  SelectedImageLink:string = "";
  AllowedDatabaseTypes:Array<string> = [];
  AllServers:Array<Server> = [];
  CurrentModalKey:string = "";
  ShowServerKeyModal:boolean = false;
  CurrentServer:Server | undefined;

  constructor(
    public _FormsService:FormsService,
    public _CommonService:CommonService,
    public _AuthService:AuthService,
    public _AdminControllerService:AdminControllerService,
    public _Router: Router,
    public _ImageControllerService:ImageControllerService,
  ) {}

  ngOnInit(): void {
    this._FormsService.SetupAddServerForm();
    this.AllowedDatabaseTypes = CONSTANTS.AllowedDatabaseTypes;
    this.UpdateAllServers();
  }

  GetServerImage(Server:Server){
    if(Server.imageLink){
      return Server.imageLink;
    }
    else{
      return "https://i.imgur.com/hvWGBUt.png";
    }
  }

  UpdateAllServers(){
    this._AdminControllerService.GetServers().subscribe((response:any) => {
      console.log(response);
      if(response.code == 1){
        this.AllServers = response.document;
        console.log(this.AllServers);
      }
    });
  }

  OnHideImageSelector(event:any){
    this.ShowImageSelector = false;
  }

  OpenImageSelector(){
    this.ShowImageSelector = true;
  }

  OnHideServerKeyModal(event:any){
    this.ShowServerKeyModal = false;
  }

  CancelServerKeyModal(){
    this.CurrentServer = undefined;
    this.CurrentModalKey = "";
    this.ShowServerKeyModal = false;
  }

  OkServerKeyModal(){
    if(this.CurrentServer){
      let model:SelectServerModel = {
        guidServer: this.CurrentServer.guidServer,
        key: this._CommonService.RsaEncrypt(this.CurrentModalKey, CONSTANTS.PublicKeyForRSA),
      };
      console.log(model);
      this._AdminControllerService.SelectServer(model).subscribe((response:any) => {
        if(response.code == 1){
          this._AuthService.AddJWTSelectedServerToken(response.document);
          this._Router.navigateByUrl("/Home");
        }
      })
    }
  }

  OpenServerKeyModal(ServerData:Server){
    this.CurrentServer = ServerData;
    this.ShowServerKeyModal = true;
  }

  ImageChanged(NewImageLink:any){
    this.SelectedImageLink = NewImageLink;
    this.ShowImageSelector = false;
  }

  ServerAddSubmit(){
    this._FormsService.AddServerForm.markAllAsTouched();
    console.log(this._FormsService.AddServerForm);
    if(this._FormsService.AddServerForm.status == "VALID"){
      var ServerData:ServerViewModel = {
        serverName: this._FormsService.AddServerForm.value['ServerName'],
        databaseType: this._FormsService.AddServerForm.value['DatabaseType'],
        connectionString: this._CommonService.RsaEncrypt(this._FormsService.AddServerForm.value['ConnectionString'], CONSTANTS.PublicKeyForRSA),
        key: this._CommonService.RsaEncrypt(this._FormsService.AddServerForm.value['Key'], CONSTANTS.PublicKeyForRSA),
        imageLink: this.SelectedImageLink
      };

      console.log(ServerData);

      this._AdminControllerService.AddServer(ServerData).subscribe((response:any) => {
        console.log(response);
      })
    }
  }
}
