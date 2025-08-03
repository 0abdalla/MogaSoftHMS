import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportsRoutingModule } from './reports-routing.module';
import { MedReportsComponent } from './med-reports/med-reports.component';
import { FinReportsComponent } from './fin-reports/fin-reports.component';
import { LedgerReportComponent } from './ledger-report/ledger-report.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [
    MedReportsComponent,
    FinReportsComponent,
    LedgerReportComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    ReportsRoutingModule
  ]
})
export class ReportsModule { }
