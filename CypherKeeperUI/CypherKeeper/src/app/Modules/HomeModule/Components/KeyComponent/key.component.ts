import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SavedKeyModel, ToSaveStringKeyField } from 'src/app/Models/SavedKeyModel';
import { tbGroupsModel } from 'src/app/Models/tbGroupsModel';
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
    id: '',
    parentGroupId: '',
    name: '',
    userName: '',
    password: '',
    websiteId: null,
    notes: '',
    isDeleted: false,
    deletedDate: null,
    updatedDate: null,
    createdDate: ''
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

  constructor(
    private _AppInitializerService: AppInitializerService,
    private _TbStringKeyFieldsControllerService: TbStringKeyFieldsControllerService,
    public _FormsService: FormsService,
    public _MixedControllerService: MixedControllerService,
  ) { }

  ngOnInit(): void {
    if (this.CurrentKey == null) {
      this.CurrentKey = {
        id: '',
        parentGroupId: '',
        name: '',
        userName: '',
        password: '',
        websiteId: null,
        notes: '',
        isDeleted: false,
        deletedDate: null,
        updatedDate: null,
        createdDate: ''
      };
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
    this._FormsService.SetupKeyForm();
  }

  BackClicked() {
    this.OnBack.emit(null);
  }

  GetOldTbStringKeyFields() {
    if (this.CurrentKey && this.CurrentKey.id) {
      this._TbStringKeyFieldsControllerService.GetByKeyId(this.CurrentKey.id).subscribe((response: any) => {
        console.log(response);
        if (response.code == 1) {
          this.CurrentTbStringKeyFields = response.document;
        }
      })
    }
  }

  KeySave() {
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
    if (this.CurrentKey && this.CurrentKey.id) {
      KeyID = this.CurrentKey.id;
    }

    if (this._FormsService.KeyForm.valid && this.CurrentGroup) {
      let WebsiteId_:string | null = null;
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
        if (StringKeyField.id) {
          KeyID_STRFLD = StringKeyField.id;
        }

        let CurrentKeyField: ToSaveStringKeyField = {
          id: KeyID_STRFLD,
          name: StringKeyField.name,
          value: StringKeyField.value
        };

        toSaveModel.stringKeyFields.push(CurrentKeyField);
      });

      this._MixedControllerService.SaveKey(toSaveModel).subscribe((response:any) => {
        console.log(response);
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
      id: '',
      name: '',
      value: '',
      parentKeyId: '',
      isDeleted: false,
      deletedDate: null,
      createdDate: '',
      updatedDate: null
    };

    this.CurrentTbStringKeyFields.push(newCustomField);
    this.CurrentTbStringKeyFields = [...this.CurrentTbStringKeyFields];
    console.log(this.CurrentTbStringKeyFields);
  }
}
