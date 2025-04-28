import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentListComponent } from './appointment-list/appointment-list.component';
import { AppointmentDetailComponent } from './appointment-detail/appointment-detail.component';
import { AppointmentFormComponent } from './appointment-form/appointment-form.component';
import { AppointmetSettingsComponent } from './appointmet-settings/appointmet-settings.component';

const routes: Routes = [
  { path: 'list', component: AppointmentListComponent },
  { path: 'detail/:id', component: AppointmentDetailComponent },
  { path: 'add', component: AppointmentFormComponent },
  { path: 'edit/:id', component: AppointmentFormComponent },
  { path: "settings", component: AppointmetSettingsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentsRoutingModule { }
