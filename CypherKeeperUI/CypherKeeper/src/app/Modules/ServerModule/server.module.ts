import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServerRoutingModule } from './server-routing.module';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';
import { SharedModule } from "../SharedModule/shared.module";

import { AbModalModule } from "node_modules/ab-modals";


@NgModule({
  declarations: [
    OpenerComponent
  ],
  imports: [
    CommonModule,
    ServerRoutingModule,
    SharedModule,
    AbModalModule
  ]
})
export class ServerModule { }
