import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FinancialRoutingModule } from './financial-routing.module';
import { BillListComponent } from './components/bill-list/bill-list.component';
import { PaymentFormComponent } from './components/payment-form/payment-form.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';


@NgModule({
  declarations: [
    BillListComponent,
    PaymentFormComponent,
    ExpenseListComponent
  ],
  imports: [
    CommonModule,
    FinancialRoutingModule
  ]
})
export class FinancialModule { }
