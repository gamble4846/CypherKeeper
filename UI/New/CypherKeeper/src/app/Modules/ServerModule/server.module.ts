import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServerRoutingModule } from './server-routing.module';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';


@NgModule({
  declarations: [
    OpenerComponent
  ],
  imports: [
    CommonModule,
    ServerRoutingModule
  ]
})
export class ServerModule { }
