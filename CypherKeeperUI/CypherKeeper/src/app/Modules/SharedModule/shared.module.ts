import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { ImageSelectorComponent } from './Components/ImageSelector/image-selector.component';


@NgModule({
  declarations: [
    ImageSelectorComponent
  ],
  imports: [
    CommonModule,
    SharedRoutingModule
  ],
  exports: [
    ImageSelectorComponent
  ]
})
export class SharedModule { }
