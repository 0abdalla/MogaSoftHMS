import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientsRoutingModule } from './patients-routing.module';
import { PatientListComponent } from './components/patient-list/patient-list.component';
import { PatientDetailComponent } from './components/patient-detail/patient-detail.component';
import { PatientFormComponent } from './components/patient-form/patient-form.component';
import { AppointmentsComponent } from './components/appointments/appointments.component';
import { AffairsComponent } from './components/affairs/affairs.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { AgePipe } from '../../core/pipes/age.pipe';
import { ToastModule } from 'primeng/toast';


@NgModule({
  declarations: [
    PatientListComponent,
    PatientDetailComponent,
    PatientFormComponent,
    AppointmentsComponent,
    AffairsComponent,
    AgePipe
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
