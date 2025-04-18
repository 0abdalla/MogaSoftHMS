import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientDetailComponent } from './components/patient-detail/patient-detail.component';
import { PatientFormComponent } from './components/patient-form/patient-form.component';
import { PatientListComponent } from './components/patient-list/patient-list.component';
import { AppointmentListComponent } from '../appointments/components/appointment-list/appointment-list.component';
import { AffairsComponent } from './components/affairs/affairs.component';

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
