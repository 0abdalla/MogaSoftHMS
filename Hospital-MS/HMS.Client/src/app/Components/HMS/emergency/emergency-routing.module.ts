import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmergencyReceptionComponent } from './emergency-reception/emergency-reception.component';
import { UpdateStatusComponent } from './update-status/update-status.component';

const routes: Routes = [
  {path:"emergency-reception" , component : EmergencyReceptionComponent},
  {path:"update-emergency-reception" , component : UpdateStatusComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmergencyRoutingModule { }
