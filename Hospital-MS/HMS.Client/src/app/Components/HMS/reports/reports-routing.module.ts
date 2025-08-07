import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FinReportsComponent } from './fin-reports/fin-reports.component';
import { MedReportsComponent } from './med-reports/med-reports.component';
import { authGuard } from '../../../Auth/auth.guard';
import { LedgerReportComponent } from './ledger-report/ledger-report.component';
import { StoreMovementComponent } from './store-movement/store-movement.component';
import { ItemMovementComponent } from './item-movement/item-movement.component';

const routes: Routes = [
  {path:"financial" , component : FinReportsComponent,canActivate:[authGuard], data: { pageName: 'reports-financial' } },
  {path:"medical" , component : MedReportsComponent,canActivate:[authGuard], data: { pageName: 'reports-medical' } },
  {path:"ledger-report" , component : LedgerReportComponent},
  {path:"store-movement" , component : StoreMovementComponent},
  {path:"item-movement" , component : ItemMovementComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
