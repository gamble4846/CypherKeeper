import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SavedKeyModel, ToSaveStringKeyField } from 'src/app/Models/SavedKeyModel';
import { tbGroupsModel } from 'src/app/Models/tbGroupsModel';
import { tbKeysHistoryModel } from 'src/app/Models/tbKeysHistoryModel';
import { tbKeysModel } from 'src/app/Models/tbKeysModel';
import { tbStringKeyFieldsModel } from 'src/app/Models/tbStringKeyFieldsModel';
import { tbTwoFactorAuthModel, tbTwoFactorAuthModel_ADD } from 'src/app/Models/tbTwoFactorAuthModel';
import { MixedControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/mixed-controller.service';
import { TbStringKeyFieldsControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-string-key-fields-controller.service';
import { TbTwoFactorAuthControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/tb-two-factor-auth-controller.service';
import { AppInitializerService } from 'src/app/Modules/SharedModule/Services/OtherServices/app-initializer.service';
import { FormsService } from 'src/app/Modules/SharedModule/Services/OtherServices/forms.service';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';
import { CommonService } from 'src/app/Modules/SharedModule/Services/OtherServices/common.service';

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
  KeyHistories: Array<tbKeysHistoryModel> = [];
  KeyTwoFactors: Array<tbTwoFactorAuthModel> = [];
  TwoFAModalIsVisible: boolean = false;
  TwoFAModalTitle: string = "Add New 2FA";

  constructor(
    private _AppInitializerService: AppInitializerService,
    private _TbStringKeyFieldsControllerService: TbStringKeyFieldsControllerService,
    public _FormsService: FormsService,
    public _MixedControllerService: MixedControllerService,
    public _TbTwoFactorAuthControllerService: TbTwoFactorAuthControllerService,
    public _CommonService:CommonService,
  ) { }

  ngOnInit(): void {
    this._FormsService.SetupKeyForm();
    this._FormsService.SetupTwoFAForm();
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
    else {
      this._FormsService.KeyForm.patchValue({
        Name: this.CurrentKey.Name,
        UserName: this.CurrentKey.UserName,
        Password: this.CurrentKey.Password,
        WebsiteId: this.CurrentKey.WebsiteId,
        Notes: this.CurrentKey.Notes,
      })

      this.GetKeyHistory();
      this.UpdateTwoFactorAuths();
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

  UpdateTwoFactorAuths() {
    this._TbTwoFactorAuthControllerService.Get().subscribe((response: any) => {
      if (response.code == 1) {
        this.KeyTwoFactors = response.document.records;
      }
      console.log(this.KeyTwoFactors);
    });
  }

  GetKeyHistory() {
    if (this.CurrentKey) {
      this._MixedControllerService.GetKeyHistory(this.CurrentKey.Id).subscribe((response: any) => {
        console.log(response);
        if (response.code == 1) {
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
      stringKeyFields: [],
      twoFactorAuths: [],
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

      this.KeyTwoFactors.forEach((TwoFA: tbTwoFactorAuthModel) => {
        let ID_TWOFA: string | null = null;
        if (TwoFA.Id) {
          ID_TWOFA = TwoFA.Id;
        }

        let CurrentTwoFA: tbTwoFactorAuthModel_ADD = {
          Name: TwoFA.Name,
          SecretKey: TwoFA.SecretKey,
          Mode: TwoFA.Mode,
          CodeSize: TwoFA.CodeSize,
          Type: TwoFA.Type,
          KeyId: null,
          Id: ID_TWOFA,
        };

        toSaveModel.twoFactorAuths.push(CurrentTwoFA);
      })


      //Encrypting
      //----------------------------------------------------
      toSaveModel.key.name = this._CommonService.RsaEncrypt(toSaveModel.key.name, CONSTANTS.PublicKeyForRSA);
      toSaveModel.key.notes = this._CommonService.RsaEncrypt(toSaveModel.key.notes, CONSTANTS.PublicKeyForRSA);
      toSaveModel.key.password = this._CommonService.RsaEncrypt(toSaveModel.key.password, CONSTANTS.PublicKeyForRSA);
      toSaveModel.key.userName = this._CommonService.RsaEncrypt(toSaveModel.key.userName, CONSTANTS.PublicKeyForRSA);

      toSaveModel.stringKeyFields.forEach((SKF:ToSaveStringKeyField) => {
        SKF.name = this._CommonService.RsaEncrypt(SKF.name, CONSTANTS.PublicKeyForRSA);
        SKF.value = this._CommonService.RsaEncrypt(SKF.value, CONSTANTS.PublicKeyForRSA);
      });

      toSaveModel.twoFactorAuths.forEach((TFA: tbTwoFactorAuthModel_ADD) => {
        TFA.SecretKey = this._CommonService.RsaEncrypt(TFA.SecretKey, CONSTANTS.PublicKeyForRSA);
      });
      //----------------------------------------------------

      this._MixedControllerService.SaveKey(toSaveModel).subscribe((response: any) => {
        console.log(response);
        if (response.code == 1) {
          if (!toSaveModel.key.id) {
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

  ShowHistory() {
    this.ShowHistoryDrawer = true;
  }

  OnCloseHistoryDrawer() {
    this.ShowHistoryDrawer = false;
  }

  JsonObjectHistory(JsonString: string) {
    return JSON.parse(JsonString);
  }

  OpenTwoFAModel(data: tbTwoFactorAuthModel | null) {
    if (data == null) {
      this.TwoFAModalTitle = "Add New 2FA";
      // this._FormsService.TwoFAForm.reset();
      this._FormsService.SetupTwoFAForm();
    }
    else {
      this.TwoFAModalTitle = "Edit 2FA - " + data.Name;
      this._FormsService.TwoFAForm.patchValue({
        Name: data.Name,
        SecretKey: data.SecretKey,
        Mode: data.Mode,
        CodeSize: data.CodeSize,
        Type: data.Type,
        Id: data.Id,
      })
    }
    this.TwoFAModalIsVisible = true;
  }

  TwoFAModalHandleCancel() {
    this.TwoFAModalIsVisible = false;
  }

  TwoFASubmit() {
    if (this._FormsService.TwoFAForm.valid) {
      let CKeyId: string | null = null;

      if (this.CurrentKey) {
        CKeyId = this.CurrentKey.Id;
      }

      var TwoFAData_Add: tbTwoFactorAuthModel_ADD = {
        Name: this._FormsService.TwoFAForm.value['Name'],
        SecretKey: this._FormsService.TwoFAForm.value['SecretKey'],
        Mode: this._FormsService.TwoFAForm.value['Mode'],
        CodeSize: this._FormsService.TwoFAForm.value['CodeSize'],
        Type: this._FormsService.TwoFAForm.value['Type'],
        KeyId: CKeyId,
        Id: this._FormsService.TwoFAForm.value['Id'],
      };

      var TwoFAData: tbTwoFactorAuthModel = {
        Id: TwoFAData_Add.Id ?? '',
        Name: TwoFAData_Add.Name,
        SecretKey: TwoFAData_Add.SecretKey,
        Mode: TwoFAData_Add.Mode,
        CodeSize: TwoFAData_Add.CodeSize,
        Type: TwoFAData_Add.Type,
        KeyId: TwoFAData_Add.KeyId ?? '',
        isDeleted: false,
        CreatedDate: '',
        UpdatedDate: null,
        DeletedDate: null,
        ArrangePosition: null
      };

      if (TwoFAData.Id) {
        this.KeyTwoFactors[this.KeyTwoFactors.findIndex((x: any) => x.Id == TwoFAData.Id)] = TwoFAData;
        this.KeyTwoFactors = [...this.KeyTwoFactors];
      }
      else {
        this.KeyTwoFactors.push(TwoFAData);
        this.KeyTwoFactors = [...this.KeyTwoFactors];
      }
      
      this.TwoFAModalIsVisible = false;
    } else {
      Object.values(this._FormsService.TwoFAForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
