import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdmissionListComponent } from './admission-list/admission-list.component';
import { AdmissionDetailComponent } from './admission-detail/admission-detail.component';
import { SurgeryFormComponent } from './surgery-form/surgery-form.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: '', component: AdmissionListComponent,canActivate:[authGuard] },
  { path: 'admission/detail/:id', component: AdmissionDetailComponent,canActivate:[authGuard] },
  { path: 'surgery/add', component: SurgeryFormComponent,canActivate:[authGuard] },
  { path: 'surgery/edit/:id', component: SurgeryFormComponent,canActivate:[authGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InpatientRoutingModule { }
