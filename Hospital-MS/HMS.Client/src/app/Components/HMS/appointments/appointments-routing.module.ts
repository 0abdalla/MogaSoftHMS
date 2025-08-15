import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentListComponent } from './appointment-list/appointment-list.component';
import { AppointmentDetailComponent } from './appointment-detail/appointment-detail.component';
import { AppointmentFormComponent } from './appointment-form/appointment-form.component';
import { AppointmetSettingsComponent } from './appointmet-settings/appointmet-settings.component';
import { AppointmentEditComponent } from './appointment-edit/appointment-edit.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: 'list', component: AppointmentListComponent, canActivate:[authGuard],data: { pageName: 'AppointmentList' } },
  { path: 'detail/:id', component: AppointmentDetailComponent, canActivate:[authGuard],data: { pageName: 'AppointmentList' } },
  { path: 'add', component: AppointmentFormComponent, canActivate:[authGuard],data: { pageName: 'AppointmentList' } },
  { path: 'edit/:id', component: AppointmentEditComponent, canActivate:[authGuard],data: { pageName: 'AppointmentList' } },
  { path: "settings", component: AppointmetSettingsComponent, canActivate:[authGuard],data: { pageName: 'AppointmentList' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentsRoutingModule { }
