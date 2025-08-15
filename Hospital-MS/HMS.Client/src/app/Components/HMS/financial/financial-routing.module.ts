import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BillListComponent } from './bill-list/bill-list.component';
import { PaymentFormComponent } from './payment-form/payment-form.component';
import { ExpenseListComponent } from './expense-list/expense-list.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: '', component: BillListComponent,canActivate:[authGuard], data: { pageName: 'PaymentList' } },
  { path: 'payment/add', component: PaymentFormComponent,canActivate:[authGuard], data: { pageName: 'PaymentList' } },
  { path: 'payment/edit/:id', component: PaymentFormComponent,canActivate:[authGuard], data: { pageName: 'PaymentList' } },
  { path: 'expenses', component: ExpenseListComponent,canActivate:[authGuard], data: { pageName: 'Expense' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinancialRoutingModule { }
