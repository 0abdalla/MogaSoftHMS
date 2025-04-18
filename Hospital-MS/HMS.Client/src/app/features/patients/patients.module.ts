import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientsRoutingModule } from './patients-routing.module';
import { PatientListComponent } from './components/patient-list/patient-list.component';
import { PatientDetailComponent } from './components/patient-detail/patient-detail.component';
import { PatientFormComponent } from './components/patient-form/patient-form.component';


@NgModule({
  declarations: [
    PatientListComponent,
    PatientDetailComponent,
    PatientFormComponent
  ],
  imports: [
    CommonModule,
    PatientsRoutingModule
  ]
})
export class PatientsModule { }
