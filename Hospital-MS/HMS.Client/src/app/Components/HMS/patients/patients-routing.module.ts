import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientListComponent } from './patient-list/patient-list.component';
import { PatientDetailComponent } from './patient-detail/patient-detail.component';
import { PatientFormComponent } from './patient-form/patient-form.component';
import { AppointmentListComponent } from '../appointments/appointment-list/appointment-list.component';
import { AffairsComponent } from './affairs/affairs.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: 'list', component: PatientListComponent,canActivate:[authGuard],data: { pageName: 'PatientList' } },
  { path: 'detail/:id', component: PatientDetailComponent,canActivate:[authGuard],data: { pageName: 'PatientList' } },
  { path: 'add', component: PatientFormComponent,canActivate:[authGuard],data: { pageName: 'PatientList' } },
  { path: 'edit/:id', component: PatientFormComponent,canActivate:[authGuard],data: { pageName: 'PatientList' } },
  { path: "appointments", component: AppointmentListComponent,canActivate:[authGuard],data: { pageName: 'AppointmentList' } },
  { path: "affairs", component: AffairsComponent,canActivate:[authGuard],data: { pageName: 'PatientList' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientsRoutingModule { }
