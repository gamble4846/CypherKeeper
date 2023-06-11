import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormsService } from '../../Services/OtherServices/forms.service';
import { CommonService } from '../../Services/OtherServices/common.service';
import { AuthService } from '../../Services/OtherServices/auth.service';
import { AdminControllerService } from '../../Services/APIServices/admin-controller.service';
import { Router } from '@angular/router';
import { ImageControllerService } from '../../Services/APIServices/image-controller.service';
import { ImagesModel } from 'src/app/Models/ImagesModel';

@Component({
  selector: 'image-selector',
  templateUrl: './image-selector.component.html',
  styleUrls: ['./image-selector.component.css']
})
export class ImageSelectorComponent {

  @ViewChild("HiddenFileSelector") HiddenFileSelector!: any;
  @Output() OnImageSelect = new EventEmitter<string>();

  CurrentUserImages:Array<ImagesModel> = [];

  constructor(
    public _FormsService:FormsService,
    public _CommonService:CommonService,
    public _AuthService:AuthService,
    public _AdminControllerService:AdminControllerService,
    public _Router: Router,
    public _ImageControllerService:ImageControllerService,
  ) { }

  ngOnInit(): void {
    this.SetCurrentuserImages();
  }

  SetCurrentuserImages(){
    this._AdminControllerService.GetImages().subscribe((response:any) => {
      if(response.code == 1){
        this.CurrentUserImages = response.document;
        this.CurrentUserImages.reverse();
      }
      console.log(this.CurrentUserImages);
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
        this.CurrentUserImages = response.document;
        this.CurrentUserImages.reverse();
      }
    });
  }

  ImageSelected(link:string){
    this.OnImageSelect.emit(link);
  }
  
}
