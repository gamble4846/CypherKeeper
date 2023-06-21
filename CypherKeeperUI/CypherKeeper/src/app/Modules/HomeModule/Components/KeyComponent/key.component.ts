import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SavedKeyModel, ToSaveStringKeyField } from 'src/app/Models/SavedKeyModel';
import { tbGroupsModel } from 'src/app/Models/tbGroupsModel';
import { tbKeysHistoryModel } from 'src/app/Models/tbKeysHistoryModel';
import { tbKeysModel } from 'src/app/Models/tbKeysModel';
import { tbStringKeyFieldsModel } from 'src/app/Models/tbStringKeyFieldsModel';
import { MixedControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/mixed-controller.service';
import { TbStringKeyFieldsControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-string-key-fields-controller.service';
import { AppInitializerService } from 'src/app/Modules/SharedModule/Services/OtherServices/app-initializer.service';
import { FormsService } from 'src/app/Modules/SharedModule/Services/OtherServices/forms.service';

@Component({
  selector: 'key-component',
  templateUrl: './key.component.html',
  styleUrls: ['./key.component.css']
})
export class KeyComponent {
  @Input() CurrentKey: tbKeysModel | null = {
    Id: '',
    ParentGroupId: '',
    Name: '',
    UserName: '',
    Password: '',
    WebsiteId: null,
    Notes: '',
    isDeleted: false,
    DeletedDate: null,
    UpdatedDate: null,
    CreatedDate: ''
  };
  @Input() CurrentGroup: tbGroupsModel | null = {
    Id: '',
    Name: '',
    ParentGroupId: null,
    IconId: null,
    isDeleted: false,
    CreatedDate: '',
    UpdatedDate: null,
    DeletedDate: null
  };

  @Output() OnBack = new EventEmitter<any>();

  CurrentTbStringKeyFields: Array<tbStringKeyFieldsModel> = [];
  ShowHistoryDrawer: boolean = false;
  KeyHistories: Array<tbKeysHistoryModel> =[];

  constructor(
    private _AppInitializerService: AppInitializerService,
    private _TbStringKeyFieldsControllerService: TbStringKeyFieldsControllerService,
    public _FormsService: FormsService,
    public _MixedControllerService: MixedControllerService,
  ) { }

  ngOnInit(): void {
    this._FormsService.SetupKeyForm();
    if (this.CurrentKey == null) {
      this.CurrentKey = {
        Id: '',
        ParentGroupId: '',
        Name: '',
        UserName: '',
        Password: '',
        WebsiteId: null,
        Notes: '',
        isDeleted: false,
        DeletedDate: null,
        UpdatedDate: null,
        CreatedDate: ''
      };
    }
    else{
      this._FormsService.KeyForm.patchValue({
        Name: this.CurrentKey.Name,
        UserName: this.CurrentKey.UserName,
        Password: this.CurrentKey.Password,
        WebsiteId: this.CurrentKey.WebsiteId,
        Notes: this.CurrentKey.Notes,
      })

      this.GetKeyHistory();
    }
    if (this.CurrentGroup == null) {
      this.CurrentGroup = {
        Id: '',
        Name: '',
        ParentGroupId: null,
        IconId: null,
        isDeleted: false,
        CreatedDate: '',
        UpdatedDate: null,
        DeletedDate: null
      };
    }

    console.log(this.CurrentKey);
    console.log(this.CurrentGroup);
    this.GetOldTbStringKeyFields();
  }

  GetKeyHistory(){
    if(this.CurrentKey){
      this._MixedControllerService.GetKeyHistory(this.CurrentKey.Id).subscribe((response:any) => {
        console.log(response);
        if(response.code == 1){
          this.KeyHistories = response.document;
          this.KeyHistories.reverse();
        }
      })
    }
  }

  BackClicked() {
    this.OnBack.emit(null);
  }

  GetOldTbStringKeyFields() {
    if (this.CurrentKey && this.CurrentKey.Id) {
      this._TbStringKeyFieldsControllerService.GetByKeyId(this.CurrentKey.Id).subscribe((response: any) => {
        if (response.code == 1) {
          this.CurrentTbStringKeyFields = response.document;
          this.CurrentTbStringKeyFields = [...this.CurrentTbStringKeyFields]
        }
      })
    }
  }

  KeySave() {
    console.log(this._FormsService.KeyForm);
    let toSaveModel: SavedKeyModel = {
      key: {
        id: null,
        parentGroupId: '',
        name: '',
        userName: '',
        password: '',
        websiteId: null,
        notes: ''
      },
      stringKeyFields: []
    };

    let KeyID: string | null = null;
    if (this.CurrentKey && this.CurrentKey.Id) {
      KeyID = this.CurrentKey.Id;
    }

    if (this._FormsService.KeyForm.valid && this.CurrentGroup) {
      let WebsiteId_: string | null = null;
      if (this._FormsService.KeyForm.value['WebsiteId']) {
        WebsiteId_ = this._FormsService.KeyForm.value['WebsiteId'];
      }

      toSaveModel.key = {
        id: KeyID,
        parentGroupId: this.CurrentGroup.Id,
        name: this._FormsService.KeyForm.value['Name'],
        userName: this._FormsService.KeyForm.value['UserName'],
        password: this._FormsService.KeyForm.value['Password'],
        websiteId: WebsiteId_,
        notes: this._FormsService.KeyForm.value['Notes']
      };

      this.CurrentTbStringKeyFields.forEach((StringKeyField: tbStringKeyFieldsModel) => {
        let KeyID_STRFLD: string | null = null;
        if (StringKeyField.Id) {
          KeyID_STRFLD = StringKeyField.Id;
        }

        let CurrentKeyField: ToSaveStringKeyField = {
          id: KeyID_STRFLD,
          name: StringKeyField.Name,
          value: StringKeyField.Value
        };

        toSaveModel.stringKeyFields.push(CurrentKeyField);
      });

      this._MixedControllerService.SaveKey(toSaveModel).subscribe((response: any) => {
        console.log(response);
        if(response.code == 1){
          if(!toSaveModel.key.id){
            this.BackClicked();
          }
        }
      })
    } else {
      Object.values(this._FormsService.KeyForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  AddCustomFieldEmpty() {
    console.log("here");
    let newCustomField: tbStringKeyFieldsModel = {
      Id: '',
      Name: '',
      Value: '',
      ParentKeyId: '',
      isDeleted: false,
      DeletedDate: null,
      CreatedDate: '',
      UpdatedDate: null
    };

    this.CurrentTbStringKeyFields.push(newCustomField);
    this.CurrentTbStringKeyFields = [...this.CurrentTbStringKeyFields];
    console.log(this.CurrentTbStringKeyFields);
  }

  ShowHistory(){
    this.ShowHistoryDrawer = true;
  }

  OnCloseHistoryDrawer(){
    this.ShowHistoryDrawer = false;
  }

  JsonObjectHistory(JsonString:string){
    return JSON.parse(JsonString);
  }
}
