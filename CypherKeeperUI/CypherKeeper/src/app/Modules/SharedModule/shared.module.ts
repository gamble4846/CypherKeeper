import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { ImageSelectorComponent } from './Components/ImageSelector/image-selector.component';
import { IconSelectorComponent } from './Components/IconSelector/icon-selector.component';


@NgModule({
  declarations: [
    ImageSelectorComponent,
    IconSelectorComponent
  ],
  imports: [
    CommonModule,
    SharedRoutingModule
  ],
  exports: [
    ImageSelectorComponent,
    IconSelectorComponent
  ]
})
export class SharedModule { }
