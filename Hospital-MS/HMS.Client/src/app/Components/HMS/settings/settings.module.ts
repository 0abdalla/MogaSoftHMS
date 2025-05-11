import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { DoctorsSettingsComponent } from './doctors-settings/doctors-settings.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { DoctorsSettingsEditComponent } from './doctors-settings-edit/doctors-settings-edit.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { AppsManagmementComponent } from './apps-managmement/apps-managmement.component';
import { MedicalServicesListComponent } from './medical-services-list/medical-services-list.component';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [
    DoctorsSettingsComponent,
    DoctorsListComponent,
    DoctorsSettingsEditComponent,
    PermissionsComponent,
    AppsManagmementComponent,
    MedicalServicesListComponent,
    
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    ReactiveFormsModule,
    ToastModule,
    NgxPaginationModule,
    FormsModule,
    NgSelectModule
  ]
})
export class SettingsModule { }
