import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HmsRoutingModule } from './hms-routing.module';
import { HmsLayoutComponent } from './hms-layout.component';
import { SharedModule } from '../../Shared/shared.module';
import { HomeComponent } from './dashboard/home/home.component';
import { ChartModule } from 'angular-highcharts';
import { HMSSideMenueComponent } from './dashboard/hms-side-menue/hms-side-menue.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    HmsLayoutComponent,
    HMSSideMenueComponent,
    HomeComponent
  ],
  imports: [
    CommonModule,
    HmsRoutingModule,
    ChartModule,
    SharedModule,
    FormsModule
  ]
})
export class HmsModule { }
