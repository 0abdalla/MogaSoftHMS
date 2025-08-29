import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FinReportsComponent } from './fin-reports/fin-reports.component';
import { MedReportsComponent } from './med-reports/med-reports.component';
import { authGuard } from '../../../Auth/auth.guard';
import { LedgerReportComponent } from './ledger-report/ledger-report.component';
import { StoreMovementComponent } from './store-movement/store-movement.component';
import { ItemMovementComponent } from './item-movement/item-movement.component';
import { ItemOrderLimitComponent } from './item-order-limit/item-order-limit.component';
import { StoreRateComponent } from './store-rate/store-rate.component';

const routes: Routes = [
  {path:"financial" , component : FinReportsComponent,canActivate:[authGuard], data: { pageName: 'FinancialReport' } },
  {path:"medical" , component : MedReportsComponent,canActivate:[authGuard], data: { pageName: 'MedicalReport' } },
  {path:"ledger-report" , component : LedgerReportComponent,canActivate:[authGuard], data: { pageName: 'LedgerReport' } },
  {path:"store-movement" , component : StoreMovementComponent, canActivate:[authGuard], data: { pageName: 'StoreMovement' } },
  {path:"item-movement" , component : ItemMovementComponent, canActivate:[authGuard], data: { pageName: 'ItemMovement' } },
  {path:"item-order-limit" , component : ItemOrderLimitComponent, canActivate:[authGuard], data: { pageName: 'ItemOrderLimit' } },
  {path:"store-rate" , component : StoreRateComponent, canActivate:[authGuard], data: { pageName: 'StoreRate' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
