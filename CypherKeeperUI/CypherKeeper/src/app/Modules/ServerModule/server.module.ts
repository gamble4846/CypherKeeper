import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
    AbModalModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class ServerModule { }
