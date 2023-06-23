import { Injectable } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormsService {
  RegisterForm!: FormGroup;
  LoginForm!: FormGroup;
  AddServerForm!:FormGroup;
  KeyForm!:FormGroup;
  TwoFAForm!:FormGroup;

  constructor(private fb: FormBuilder) { }

  SetupRegisterForm(){
    this.RegisterForm = this.fb.group({
      Username: [null, [Validators.required]],
      Password: [null, [Validators.required]],
      Email: [null, [Validators.required, Validators.email]],
      FirstName: [null, [Validators.required]],
      LastName: [null, [Validators.required]],
    });
  }

  SetupLoginForm(){
    this.LoginForm = this.fb.group({
      Username: [null, [Validators.required]],
      Password: [null, [Validators.required]]
    });
  }

  SetupAddServerForm(){
    this.AddServerForm = this.fb.group({
      ServerName: [null, [Validators.required]],
      DatabaseType: [null, [Validators.required]],
      ConnectionString: [null, [Validators.required]],
      Key: [null, [Validators.required]]
    });
  }

  SetupKeyForm(){
    this.KeyForm = this.fb.group({
      Name: [null, [Validators.required]],
      UserName: [null],
      Password: [null],
      WebsiteId: [null],
      Notes: [null],
    });
  }

  SetupTwoFAForm(){
    this.TwoFAForm = this.fb.group({
      Name: [null, [Validators.required]],
      SecretKey: [null, [Validators.required]],
      Mode: ['Sha1', [Validators.required]],
      CodeSize: [6, [Validators.required]],
      Type: ['TOTP', [Validators.required]],
      Step: [30, [Validators.required]],
      Id: [null]
    });
  }
}
