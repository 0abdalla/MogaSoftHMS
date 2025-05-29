import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorsSettingsComponent } from './doctors-settings/doctors-settings.component';
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { DoctorsSettingsEditComponent } from './doctors-settings-edit/doctors-settings-edit.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { AppsManagmementComponent } from './apps-managmement/apps-managmement.component';
import { MedicalServicesListComponent } from './medical-services-list/medical-services-list.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  {path:"doctors" , component:DoctorsSettingsComponent,canActivate:[authGuard], data: { pageName: 'doctors' } },
  {path:"doctors-list" , component : DoctorsListComponent,canActivate:[authGuard], data: { pageName: 'doctors-list' } },
  {path:"doctors/:id" , component : DoctorsSettingsEditComponent,canActivate:[authGuard], data: { pageName: 'doctors-list' } },
  {path:"permissions" , component : PermissionsComponent,canActivate:[authGuard], data: { pageName: 'permissions' } },
  {path:"apps-managmement" , component : AppsManagmementComponent,canActivate:[authGuard], data: { pageName: 'apps-managmement' } },
  {path:"medical-services-list" , component : MedicalServicesListComponent,canActivate:[authGuard], data: { pageName: 'medical-services-list' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
