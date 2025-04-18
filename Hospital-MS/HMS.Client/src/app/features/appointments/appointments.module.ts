import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppointmentsRoutingModule } from './appointments-routing.module';
import { AppointmentListComponent } from './components/appointment-list/appointment-list.component';
import { AppointmentDetailComponent } from './components/appointment-detail/appointment-detail.component';
import { AppointmentFormComponent } from './components/appointment-form/appointment-form.component';


@NgModule({
  declarations: [
    AppointmentListComponent,
    AppointmentDetailComponent,
    AppointmentFormComponent
  ],
  imports: [
    CommonModule,
    AppointmentsRoutingModule
  ]
})
export class AppointmentsModule { }
