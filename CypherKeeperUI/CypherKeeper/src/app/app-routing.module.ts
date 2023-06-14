import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ServerGuard } from './Modules/SharedModule/Guards/server.guard';
import { LoginGuard } from './Modules/SharedModule/Guards/login.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'Home' },
  { path: 'Auth', loadChildren: () => import('./Modules/AuthModule/auth.module').then(m => m.AuthModule) },
  { path: 'Server', loadChildren: () => import('./Modules/ServerModule/server.module').then(m => m.ServerModule), canActivate:[LoginGuard] },
  { path: 'Home', loadChildren: () => import('./Modules/HomeModule/home.module').then(m => m.HomeModule), canActivate:[ServerGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
