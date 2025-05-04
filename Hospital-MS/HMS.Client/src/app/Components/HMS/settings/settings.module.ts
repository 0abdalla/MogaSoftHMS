import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { DoctorsSettingsComponent } from './doctors-settings/doctors-settings.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { DoctorsSettingsEditComponent } from './doctors-settings-edit/doctors-settings-edit.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { AppsManagmementComponent } from './apps-managmement/apps-managmement.component';


@NgModule({
  declarations: [
    DoctorsSettingsComponent,
    DoctorsListComponent,
    DoctorsSettingsEditComponent,
    PermissionsComponent,
    AppsManagmementComponent
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    ReactiveFormsModule,
    ToastModule,
    NgxPaginationModule
  ]
})
export class SettingsModule { }
