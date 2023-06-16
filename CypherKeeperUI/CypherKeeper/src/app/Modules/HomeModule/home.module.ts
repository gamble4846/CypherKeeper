import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfiniteMenuModule } from 'infinite-menu';
import { HomeRoutingModule } from './home-routing.module';
import { OpenerComponent } from './Components/OpenerComponent/opener.component';

import { NzDropDownModule } from 'ng-zorro-antd/dropdown';


@NgModule({
  declarations: [
    OpenerComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    InfiniteMenuModule,
    NzDropDownModule
  ]
})
export class HomeModule { }
