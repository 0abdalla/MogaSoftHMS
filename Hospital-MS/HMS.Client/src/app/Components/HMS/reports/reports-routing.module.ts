import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FinReportsComponent } from './fin-reports/fin-reports.component';
import { MedReportsComponent } from './med-reports/med-reports.component';
import { authGuard } from '../../../Auth/auth.guard';
import { LedgerReportComponent } from './ledger-report/ledger-report.component';

const routes: Routes = [
  {path:"financial" , component : FinReportsComponent,canActivate:[authGuard], data: { pageName: 'reports-financial' } },
  {path:"medical" , component : MedReportsComponent,canActivate:[authGuard], data: { pageName: 'reports-medical' } },
  {path:"ledger-report" , component : LedgerReportComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
