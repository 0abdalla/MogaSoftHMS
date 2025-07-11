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

const routes: Routes = [
  { path: "medical-service", component: DoctorsMedicalServiceComponent, canActivate: [authGuard], data: { pageName: 'doctors' } },
  { path: "doctors-list", component: DoctorsListComponent, canActivate: [authGuard], data: { pageName: 'doctors-list' } },
  { path: "doctors", component: DoctorsSettingsEditComponent, canActivate: [authGuard], data: { pageName: 'doctors-list' } },
  { path: "permissions", component: PermissionsComponent, canActivate: [authGuard], data: { pageName: 'permissions' } },
  { path: "apps-managmement", component: AppsManagmementComponent, canActivate: [authGuard], data: { pageName: 'apps-managmement' } },
  { path: "medical-services-list", component: MedicalServicesListComponent, canActivate: [authGuard], data: { pageName: 'medical-services-list' } },
  { path: 'account-tree', component: AccountTreeContainerComponent, canActivate: [authGuard] },
  { path: 'cost-center-tree', component: CostCenterTreeContainerComponent, canActivate: [authGuard] },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
