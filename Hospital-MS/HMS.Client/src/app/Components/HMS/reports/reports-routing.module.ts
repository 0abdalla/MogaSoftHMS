import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FinReportsComponent } from './fin-reports/fin-reports.component';
import { MedReportsComponent } from './med-reports/med-reports.component';

const routes: Routes = [
  {path:"financial" , component : FinReportsComponent},
  {path:"medical" , component : MedReportsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
