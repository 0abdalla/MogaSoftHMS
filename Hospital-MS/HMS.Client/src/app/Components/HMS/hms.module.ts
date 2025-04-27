import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HmsRoutingModule } from './hms-routing.module';
import { HmsLayoutComponent } from './hms-layout.component';
import { SharedModule } from '../../Shared/shared.module';


@NgModule({
  declarations: [
    HmsLayoutComponent
  ],
  imports: [
    CommonModule,
    HmsRoutingModule,
    SharedModule
  ]
})
export class HmsModule { }
