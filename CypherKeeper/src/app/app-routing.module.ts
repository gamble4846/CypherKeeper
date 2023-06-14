import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'Server' },
  { path: 'Auth', loadChildren: () => import('./Modules/AuthModule/auth.module').then(m => m.AuthModule) },
  { path: 'Server', loadChildren: () => import('./Modules/ServerModule/server.module').then(m => m.ServerModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
