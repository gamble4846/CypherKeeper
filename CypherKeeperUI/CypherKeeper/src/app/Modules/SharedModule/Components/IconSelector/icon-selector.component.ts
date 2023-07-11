import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormsService } from '../../Services/OtherServices/forms.service';
import { CommonService } from '../../Services/OtherServices/common.service';
import { AuthService } from '../../Services/OtherServices/auth.service';
import { AdminControllerService } from '../../Services/APIServices/admin-controller.service';
import { Router } from '@angular/router';
import { ImageControllerService } from '../../Services/APIServices/image-controller.service';
import { ImagesModel } from 'src/app/Models/ImagesModel';
import { tbIconsModel, tbIconsModel_NUll } from 'src/app/Models/tbIconsModel';
import { TbIconsControllerService } from '../../Services/APIServices/tb-icons-controller.service';

@Component({
  selector: 'icon-selector',
  templateUrl: './icon-selector.component.html',
  styleUrls: ['./icon-selector.component.css']
})
export class IconSelectorComponent {

  @ViewChild("HiddenFileSelector") HiddenFileSelector!: any;
  @Output() OnIconSelect = new EventEmitter<tbIconsModel>();

  CurrentUserIcons:Array<tbIconsModel> = [];

  constructor(
    public _FormsService:FormsService,
    public _CommonService:CommonService,
    public _AuthService:AuthService,
    public _AdminControllerService:AdminControllerService,
    public _Router: Router,
    public _ImageControllerService:ImageControllerService,
    public _TbIconsControllerService:TbIconsControllerService,
  ) { }

  ngOnInit(): void {
    this.SetCurrentuserIcons();
  }

  SetCurrentuserIcons(){
    this._TbIconsControllerService.Get().subscribe((response:any) => {
      if(response.code == 1){
        this.CurrentUserIcons = response.document.records;
        this.CurrentUserIcons.reverse();
      }
    })
  }

  SelectFile(){
    this.HiddenFileSelector.nativeElement.value = null;
    this.HiddenFileSelector.nativeElement.click();
  }

  UploadFile(event:any){
    const file = event.target.files[0];
    this._ImageControllerService.UploadImage(file).subscribe((response:any) => {
      if(response.code == 1){
        let NewIconModel:tbIconsModel = {
          id: this._CommonService.GenerateUUID(),
          link: response.document.newImageLink,
          isDeleted: false,
          createdDate: new Date().toISOString(),
          updatedDate: null,
          deletedDate: null
        };
        this._TbIconsControllerService.Add(NewIconModel).subscribe((response:any) => {
          if(response.code == 1){
            this.CurrentUserIcons.unshift(response.document);
          }
        })
      }
    });
  }

  IconSelected(data:tbIconsModel | undefined){
    this.OnIconSelect.emit(data);
  }
  
}
