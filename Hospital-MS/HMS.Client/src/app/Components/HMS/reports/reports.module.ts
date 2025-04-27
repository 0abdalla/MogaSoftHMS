import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportsRoutingModule } from './reports-routing.module';
import { MedReportsComponent } from './med-reports/med-reports.component';
import { FinReportsComponent } from './fin-reports/fin-reports.component';


@NgModule({
  declarations: [
    MedReportsComponent,
    FinReportsComponent
  ],
  imports: [
    CommonModule,
    ReportsRoutingModule
  ]
})
export class ReportsModule { }
