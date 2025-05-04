import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorsSettingsComponent } from './doctors-settings/doctors-settings.component';
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { DoctorsSettingsEditComponent } from './doctors-settings-edit/doctors-settings-edit.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { AppsManagmementComponent } from './apps-managmement/apps-managmement.component';

const routes: Routes = [
  {path:"doctors" , component:DoctorsSettingsComponent},
  {path:"doctors-list" , component : DoctorsListComponent },
  {path:"doctors/:id" , component : DoctorsSettingsEditComponent},
  {path:"permissions" , component : PermissionsComponent},
  {path:"apps-managmement" , component : AppsManagmementComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
