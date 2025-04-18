import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentDetailComponent } from './components/appointment-detail/appointment-detail.component';
import { AppointmentFormComponent } from './components/appointment-form/appointment-form.component';
import { AppointmentListComponent } from './components/appointment-list/appointment-list.component';

const routes: Routes = [
  { path: '', component: AppointmentListComponent },
  { path: 'detail/:id', component: AppointmentDetailComponent },
  { path: 'add', component: AppointmentFormComponent },
  { path: 'edit/:id', component: AppointmentFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentsRoutingModule { }
