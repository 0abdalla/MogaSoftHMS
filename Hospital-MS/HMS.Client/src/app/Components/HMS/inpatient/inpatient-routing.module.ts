import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdmissionListComponent } from './admission-list/admission-list.component';
import { AdmissionDetailComponent } from './admission-detail/admission-detail.component';
import { SurgeryFormComponent } from './surgery-form/surgery-form.component';

const routes: Routes = [
  { path: '', component: AdmissionListComponent },
  { path: 'admission/detail/:id', component: AdmissionDetailComponent },
  { path: 'surgery/add', component: SurgeryFormComponent },
  { path: 'surgery/edit/:id', component: SurgeryFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InpatientRoutingModule { }
