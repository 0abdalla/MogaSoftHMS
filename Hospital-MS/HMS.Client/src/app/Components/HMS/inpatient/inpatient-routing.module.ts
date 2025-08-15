import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdmissionListComponent } from './admission-list/admission-list.component';
import { AdmissionDetailComponent } from './admission-detail/admission-detail.component';
import { SurgeryFormComponent } from './surgery-form/surgery-form.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: '', component: AdmissionListComponent,canActivate:[authGuard], data: { pageName: 'AdmissionList' } },
  { path: 'admission/detail/:id', component: AdmissionDetailComponent,canActivate:[authGuard], data: { pageName: 'AdmissionList' } },
  { path: 'surgery/add', component: SurgeryFormComponent,canActivate:[authGuard], data: { pageName: 'Surgery' } },
  { path: 'surgery/edit/:id', component: SurgeryFormComponent,canActivate:[authGuard], data: { pageName: 'Surgery' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InpatientRoutingModule { }
