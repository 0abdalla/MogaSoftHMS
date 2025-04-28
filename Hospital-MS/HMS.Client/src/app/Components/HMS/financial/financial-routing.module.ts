import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BillListComponent } from './bill-list/bill-list.component';
import { PaymentFormComponent } from './payment-form/payment-form.component';
import { ExpenseListComponent } from './expense-list/expense-list.component';

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
