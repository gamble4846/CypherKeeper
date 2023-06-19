import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfiniteMenuModule } from 'infinite-menu';
import { HomeRoutingModule } from './home-routing.module';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from "../SharedModule/shared.module";

import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';


@NgModule({
  declarations: [
    OpenerComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    InfiniteMenuModule,
    NzDropDownModule,
    NzDrawerModule,
    NzButtonModule,
    FormsModule,
    ReactiveFormsModule,
    NzInputModule,
    NzModalModule,
    SharedModule,
    NzIconModule,
    NzGridModule,
    NzCardModule
  ]
})
export class HomeModule { }
