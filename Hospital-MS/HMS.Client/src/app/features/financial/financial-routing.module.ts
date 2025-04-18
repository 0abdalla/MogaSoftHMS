import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BillListComponent } from './components/bill-list/bill-list.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';
import { PaymentFormComponent } from './components/payment-form/payment-form.component';

const routes: Routes = [
  { path: '', component: BillListComponent },
  { path: 'payment/add', component: PaymentFormComponent },
  { path: 'payment/edit/:id', component: PaymentFormComponent },
  { path: 'expenses', component: ExpenseListComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinancialRoutingModule { }
