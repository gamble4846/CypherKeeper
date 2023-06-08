import { Component } from '@angular/core';
import { FormService } from 'src/app/MyCommonModule/Services/OtherServices/form.service';

@Component({
  selector: 'app-login-sign-up',
  templateUrl: './login-sign-up.component.html',
  styleUrls: ['./login-sign-up.component.css']
})
export class LoginSignUpComponent {
  constructor(
    public _FormService:FormService,
  ) {}

  ngOnInit(): void {
    this._FormService.SetupRegisterForm();
    this._FormService.SetupLoginForm();
  }

  RegisterSubmit(){
    
  }
}
