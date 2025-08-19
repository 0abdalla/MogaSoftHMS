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
import { PrintInvoiceComponent } from './print-invoice/print-invoice.component';
import { AppointmentEditComponent } from './appointment-edit/appointment-edit.component';
import { SharedModule } from '../../../Shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Time12PipePipe } from '../../../Pipes/time12-pipe.pipe';

@NgModule({
  declarations: [
    AppointmentListComponent,
    AppointmentDetailComponent,
    AppointmentFormComponent,
    AppointmetSettingsComponent,
    PrintInvoiceComponent,
    AppointmentEditComponent,
    // Time12PipePipe
  ],
  imports: [
    CommonModule,
    AppointmentsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    ToastModule,
    SharedModule,
    NgbModule
],
})
export class AppointmentsModule { }
