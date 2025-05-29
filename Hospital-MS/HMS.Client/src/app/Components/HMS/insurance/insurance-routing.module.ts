import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InsuranceListComponent } from './insurance-list/insurance-list.component';
import { InsuranceFormComponent } from './insurance-form/insurance-form.component';
import { InsuranceEditComponent } from './insurance-edit/insurance-edit.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  {path:"insurance-list", canActivate:[authGuard],component:InsuranceListComponent, data: { pageName: 'insurance-list' } },
  {path:"add-insurance" , canActivate:[authGuard],component:InsuranceFormComponent, data: { pageName: 'add-insurance' } },
  {path:"add-insurance/:id", canActivate:[authGuard],component:InsuranceEditComponent, data: { pageName: 'add-insurance' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InsuranceRoutingModule { }
