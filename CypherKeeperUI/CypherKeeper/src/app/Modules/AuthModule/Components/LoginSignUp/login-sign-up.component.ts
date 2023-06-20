import { Component } from '@angular/core';
import { RegisterModel } from 'src/app/Models/RegisterModel';
import { CommonService } from 'src/app/Modules/SharedModule/Services/OtherServices/common.service';
import { FormsService } from 'src/app/Modules/SharedModule/Services/OtherServices/forms.service';
import * as CONSTANTS from 'src/app/Modules/SharedModule/Constants/CONSTANTS';
import { AdminControllerService } from 'src/app/Modules/SharedModule/Services/APIServices/admin-controller.service';
import { LoginModel } from 'src/app/Models/LoginModel';
import { AuthService } from 'src/app/Modules/SharedModule/Services/OtherServices/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-sign-up',
  templateUrl: './login-sign-up.component.html',
  styleUrls: ['./login-sign-up.component.css']
})
export class LoginSignUpComponent {
  constructor(
    public _FormsService:FormsService,
    public _CommonService:CommonService,
    public _AuthService:AuthService,
    public _AdminControllerService:AdminControllerService,
    public _Router: Router,
  ) {}

  ngOnInit(): void {
    this._AuthService.RemoveJWTUserToken();
    this._AuthService.RemoveJWTSelectedServerToken();
    this._FormsService.SetupRegisterForm();
    this._FormsService.SetupLoginForm();
  }

  RegisterSubmit(){
    if (this._FormsService.RegisterForm.valid) {
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
    } else {
      Object.values(this._FormsService.RegisterForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  LoginSubmit(){
    this._FormsService.LoginForm.markAllAsTouched();
    console.log(this._FormsService.LoginForm);
    var RSAKeyPair = this._CommonService.GenerateRSAPairKeys();
    if(this._FormsService.LoginForm.status == "VALID"){
      var LoginData:LoginModel = {
        username: this._FormsService.LoginForm.value['Username'],
        password: this._CommonService.RsaEncrypt(this._FormsService.LoginForm.value['Password'], CONSTANTS.PublicKeyForRSA),
        newPublicKey: this._CommonService.EncodeBase64(RSAKeyPair.PublicKey),
      };

      this._AdminControllerService.Login(LoginData).subscribe((response:any) => {
        console.log(response);
        if(response.code == 1){
          this._AuthService.AddJWTUserTokenToLocal(response.document);
          this._AuthService.SetRSAPrivateKeyForAPI(RSAKeyPair.PrivateKey);
          this._Router.navigateByUrl("/Server");
        }
        else{
          console.log("Invalid Login");
        }
      })
      console.log("LoginData", LoginData);
    }
  }
}
