import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginSignUpComponent } from './Components/LoginSignUp/login-sign-up.component';

const routes: Routes = [
  { path: 'Login', component: LoginSignUpComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
