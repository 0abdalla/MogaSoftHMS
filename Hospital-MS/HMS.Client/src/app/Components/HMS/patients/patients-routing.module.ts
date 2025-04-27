import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientListComponent } from './patient-list/patient-list.component';
import { PatientDetailComponent } from './patient-detail/patient-detail.component';
import { PatientFormComponent } from './patient-form/patient-form.component';
import { AppointmentListComponent } from '../appointments/appointment-list/appointment-list.component';
import { AffairsComponent } from './affairs/affairs.component';

const routes: Routes = [
  { path: 'list', component: PatientListComponent },
  { path: 'detail/:id', component: PatientDetailComponent },
  { path: 'add', component: PatientFormComponent },
  { path: 'edit/:id', component: PatientFormComponent },
  { path: "appointments", component: AppointmentListComponent },
  { path: "affairs", component: AffairsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientsRoutingModule { }
