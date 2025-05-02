import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppointmentsRoutingModule } from './appointments-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { ToastModule } from 'primeng/toast';
import { AppointmentListComponent } from './appointment-list/appointment-list.component';
import { AppointmentDetailComponent } from './appointment-detail/appointment-detail.component';
import { AppointmentFormComponent } from './appointment-form/appointment-form.component';
import { AppointmetSettingsComponent } from './appointmet-settings/appointmet-settings.component';
import { MessageService } from 'primeng/api';
import { PrintInvoiceComponent } from './print-invoice/print-invoice.component';

@NgModule({
  declarations: [
    AppointmentListComponent,
    AppointmentDetailComponent,
    AppointmentFormComponent,
    AppointmetSettingsComponent,
    PrintInvoiceComponent
  ],
  imports: [
    CommonModule,
    AppointmentsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    ToastModule
  ],
})
export class AppointmentsModule { }
