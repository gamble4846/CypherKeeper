import { Injectable } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormsService {
  RegisterForm!: FormGroup;
  LoginForm!: FormGroup;

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
}
