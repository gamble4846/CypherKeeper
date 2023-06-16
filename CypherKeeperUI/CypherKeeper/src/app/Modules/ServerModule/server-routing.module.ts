import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';
import { LoginGuard } from '../SharedModule/Guards/login.guard';

const routes: Routes = [
  { path: '', component: OpenerComponent, canActivate:[LoginGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ServerRoutingModule { }
