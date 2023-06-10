import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServerRoutingModule } from './server-routing.module';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';
import { SharedModule } from "../SharedModule/shared.module";


@NgModule({
  declarations: [
    OpenerComponent
  ],
  imports: [
    CommonModule,
    ServerRoutingModule,
    SharedModule
  ]
})
export class ServerModule { }
