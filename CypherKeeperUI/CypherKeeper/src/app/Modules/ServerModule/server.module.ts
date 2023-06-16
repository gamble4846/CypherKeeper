import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ServerRoutingModule } from './server-routing.module';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';
import { SharedModule } from "../SharedModule/shared.module";
import { AbModalModule } from "node_modules/ab-modals";

import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzModalModule } from 'ng-zorro-antd/modal';

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
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzSelectModule,
    NzCardModule,
    NzModalModule
  ]
})
export class ServerModule { }
