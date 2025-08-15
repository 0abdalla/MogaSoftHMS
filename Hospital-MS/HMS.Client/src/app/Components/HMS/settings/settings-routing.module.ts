import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorsMedicalServiceComponent } from './doctors-settings/doctors-medical-service.component';
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { DoctorsSettingsEditComponent } from './doctors-settings-edit/doctors-settings-edit.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { AppsManagmementComponent } from './apps-managmement/apps-managmement.component';
import { MedicalServicesListComponent } from './medical-services-list/medical-services-list.component';
import { authGuard } from '../../../Auth/auth.guard';
import { AccountTreeContainerComponent } from './account-tree-container/account-tree-container.component';
import { CostCenterTreeContainerComponent } from './cost-center-tree-container/cost-center-tree-container.component';
import { DoctorsDepartmentsListComponent } from './doctors-departments-list/doctors-departments-list.component';

const routes: Routes = [
  { path: "medical-service", component: DoctorsMedicalServiceComponent, canActivate: [authGuard], data: { pageName: 'MedicalService' } },
  { path: "doctors-list", component: DoctorsListComponent, canActivate: [authGuard], data: { pageName: 'DoctorList' } },
  { path: "doctors", component: DoctorsSettingsEditComponent, canActivate: [authGuard], data: { pageName: 'DoctorList' } },
  { path: "permissions", component: PermissionsComponent, canActivate: [authGuard], data: { pageName: 'Permissions' } },
  { path: "apps-managmement", component: AppsManagmementComponent, canActivate: [authGuard], data: { pageName: 'AppsManagmement' } },
  { path: "medical-services-list", component: MedicalServicesListComponent, canActivate: [authGuard], data: { pageName: 'MedicalServicesList' } },
  { path: 'account-tree', component: AccountTreeContainerComponent, canActivate: [authGuard], data: { pageName: 'AccountTree' } },
  { path: 'cost-center-tree', component: CostCenterTreeContainerComponent, canActivate: [authGuard], data: { pageName: 'CostCenterTree' } },
  { path: 'medical-departments-list', component: DoctorsDepartmentsListComponent, canActivate: [authGuard], data: { pageName: 'DoctorsDepartmentsList' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
