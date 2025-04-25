import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorsSettingsComponent } from './doctors-settings/doctors-settings.component';
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { DoctorsSettingsEditComponent } from './doctors-settings-edit/doctors-settings-edit.component';

const routes: Routes = [
  {path:"doctors" , component:DoctorsSettingsComponent},
  {path:"doctors-list" , component : DoctorsListComponent },
  {path:"doctors/:id" , component : DoctorsSettingsEditComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
