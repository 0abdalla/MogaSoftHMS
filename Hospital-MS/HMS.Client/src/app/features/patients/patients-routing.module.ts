import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientDetailComponent } from './components/patient-detail/patient-detail.component';
import { PatientFormComponent } from './components/patient-form/patient-form.component';
import { PatientListComponent } from './components/patient-list/patient-list.component';

const routes: Routes = [
  { path: '', component: PatientListComponent },
  { path: 'detail/:id', component: PatientDetailComponent },
  { path: 'add', component: PatientFormComponent },
  { path: 'edit/:id', component: PatientFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientsRoutingModule { }
