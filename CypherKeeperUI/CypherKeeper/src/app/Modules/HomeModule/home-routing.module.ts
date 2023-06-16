import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';
import { ServerGuard } from '../SharedModule/Guards/server.guard';

const routes: Routes = [
  { path: '', component: OpenerComponent, canActivate:[ServerGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
