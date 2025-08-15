import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmergencyReceptionComponent } from './emergency-reception/emergency-reception.component';
import { UpdateStatusComponent } from './update-status/update-status.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  {path:"emergency-reception" , component : EmergencyReceptionComponent, canActivate:[authGuard],data: { pageName: 'EmergencyReception' } },
  {path:"update-emergency-reception" , component : UpdateStatusComponent, canActivate:[authGuard],data: { pageName: 'EmergencyReception' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmergencyRoutingModule { }
