import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientsRoutingModule } from './patients-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { ToastModule } from 'primeng/toast';
import { PatientListComponent } from './patient-list/patient-list.component';
import { PatientDetailComponent } from './patient-detail/patient-detail.component';
import { PatientFormComponent } from './patient-form/patient-form.component';
import { AppointmentsComponent } from './appointments/appointments.component';
import { AffairsComponent } from './affairs/affairs.component';


@NgModule({
  declarations: [
    PatientListComponent,
    PatientDetailComponent,
    PatientFormComponent,
    AppointmentsComponent,
    AffairsComponent
  ],
  imports: [
    CommonModule,
    PatientsRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    NgSelectModule,
    NgxPaginationModule,
    ToastModule
  ]
})
export class PatientsModule { }
