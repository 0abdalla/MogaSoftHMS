import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { DoctorsMedicalServiceComponent } from './doctors-settings/doctors-medical-service.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { DoctorsSettingsEditComponent } from './doctors-settings-edit/doctors-settings-edit.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { AppsManagmementComponent } from './apps-managmement/apps-managmement.component';
import { MedicalServicesListComponent } from './medical-services-list/medical-services-list.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { AccountTreeContainerComponent } from './account-tree-container/account-tree-container.component';
import { AccountTreeItemComponent } from './account-tree-container/account-tree-item/account-tree-item.component';
import { AddEditAccountTreeComponent } from './account-tree-container/add-edit-account-tree/add-edit-account-tree.component';
import { SharedModule } from '../../../Shared/shared.module';
import { CostCenterTreeContainerComponent } from './cost-center-tree-container/cost-center-tree-container.component';
import { AddEditCostCenterTreeComponent } from './cost-center-tree-container/add-edit-cost-center-tree/add-edit-cost-center-tree.component';
import { CostCenterTreeComponent } from './cost-center-tree-container/cost-center-tree/cost-center-tree.component';
import { CostCenterTreeItemComponent } from './cost-center-tree-container/cost-center-tree-item/cost-center-tree-item.component';
import { AccountTreeComponent } from './account-tree-container/account-tree/account-tree.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DoctorsDepartmentsListComponent } from './doctors-departments-list/doctors-departments-list.component';


@NgModule({
  declarations: [
    DoctorsMedicalServiceComponent,
    DoctorsListComponent,
    DoctorsSettingsEditComponent,
    PermissionsComponent,
    AppsManagmementComponent,
    MedicalServicesListComponent,
    AccountTreeContainerComponent,
    AccountTreeComponent,
    AccountTreeItemComponent,
    AddEditAccountTreeComponent,
    CostCenterTreeContainerComponent,
    AddEditCostCenterTreeComponent,
    CostCenterTreeComponent,
    CostCenterTreeItemComponent,
    DoctorsDepartmentsListComponent,
    
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    ReactiveFormsModule,
    ToastModule,
    NgxPaginationModule,
    FormsModule,
    NgSelectModule,
    SharedModule,
    NgbModule
  ]
})
export class SettingsModule { }
