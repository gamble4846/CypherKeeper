import { Component } from '@angular/core';
import { RegisterModel } from 'src/app/Models/RegisterModel';
import { CommonService } from 'src/app/Modules/SharedModule/Services/OtherServices/common.service';
import { FormsService } from 'src/app/Modules/SharedModule/Services/OtherServices/forms.service';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';
import { AdminControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/admin-controller.service';

@Component({
  selector: 'app-login-sign-up',
  templateUrl: './login-sign-up.component.html',
  styleUrls: ['./login-sign-up.component.css']
})
export class LoginSignUpComponent {
  constructor(
    public _FormsService:FormsService,
    public _CommonService:CommonService,
    public _AdminControllerService:AdminControllerService,
  ) {}

  ngOnInit(): void {
    this._FormsService.SetupRegisterForm();
    this._FormsService.SetupLoginForm();
  }

  RegisterSubmit(){
    this._FormsService.RegisterForm.markAllAsTouched();
    console.log(this._FormsService.RegisterForm);
    if(this._FormsService.RegisterForm.status == "VALID"){
      var RegisterData:RegisterModel = {
        username: this._FormsService.RegisterForm.value['Username'],
        password: this._CommonService.RsaEncrypt(this._FormsService.RegisterForm.value['Password'], CONSTANTS.PublicKeyForRSA),
        email: this._FormsService.RegisterForm.value['Email'],
        firstName: this._FormsService.RegisterForm.value['FirstName'],
        lastName: this._FormsService.RegisterForm.value['LastName']
      };

      this._AdminControllerService.Register(RegisterData).subscribe((response:any) => {
        console.log(response);
      })
      console.log("RegisterData", RegisterData);
    }
  }
}
