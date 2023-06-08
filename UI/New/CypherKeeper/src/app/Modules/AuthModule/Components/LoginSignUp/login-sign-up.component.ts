import { Component } from '@angular/core';
import { RegisterModel } from 'src/app/Models/RegisterModel';
import { FormsService } from 'src/app/Modules/SharedModule/Services/OtherServices/forms.service';

@Component({
  selector: 'app-login-sign-up',
  templateUrl: './login-sign-up.component.html',
  styleUrls: ['./login-sign-up.component.css']
})
export class LoginSignUpComponent {
  constructor(
    public _FormsService:FormsService,
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
        password: this._FormsService.RegisterForm.value['Password'],
        email: this._FormsService.RegisterForm.value['Email'],
        firstName: this._FormsService.RegisterForm.value['FirstName'],
        lastName: this._FormsService.RegisterForm.value['LastName']
      };

      console.log("RegisterData", RegisterData);
    }
  }
}
