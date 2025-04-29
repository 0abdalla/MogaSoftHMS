import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InsuranceListComponent } from './insurance-list/insurance-list.component';
import { InsuranceFormComponent } from './insurance-form/insurance-form.component';
import { InsuranceEditComponent } from './insurance-edit/insurance-edit.component';

const routes: Routes = [
  {path:"insurance-list", component:InsuranceListComponent},
  {path:"add-insurance" , component:InsuranceFormComponent},
  {path:"add-insurance/:id", component:InsuranceEditComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InsuranceRoutingModule { }
