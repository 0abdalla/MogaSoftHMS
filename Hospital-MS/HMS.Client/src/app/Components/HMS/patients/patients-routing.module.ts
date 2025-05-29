import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientListComponent } from './patient-list/patient-list.component';
import { PatientDetailComponent } from './patient-detail/patient-detail.component';
import { PatientFormComponent } from './patient-form/patient-form.component';
import { AppointmentListComponent } from '../appointments/appointment-list/appointment-list.component';
import { AffairsComponent } from './affairs/affairs.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: 'list', component: PatientListComponent,canActivate:[authGuard],data: { pageName: 'patients-list' } },
  { path: 'detail/:id', component: PatientDetailComponent,canActivate:[authGuard],data: { pageName: 'patients-list' } },
  { path: 'add', component: PatientFormComponent,canActivate:[authGuard],data: { pageName: 'patients-add' } },
  { path: 'edit/:id', component: PatientFormComponent,canActivate:[authGuard],data: { pageName: 'patients-add' } },
  { path: "appointments", component: AppointmentListComponent,canActivate:[authGuard],data: { pageName: 'appointments-list' } },
  { path: "affairs", component: AffairsComponent,canActivate:[authGuard],data: { pageName: 'Affairs' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientsRoutingModule { }
