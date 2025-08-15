import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MedicationListComponent } from './medication-list/medication-list.component';
import { PrescriptionFormComponent } from './prescription-form/prescription-form.component';
import { InventoryListComponent } from './inventory-list/inventory-list.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: '', component: MedicationListComponent,canActivate:[authGuard], data: { pageName: 'PrescriptionList' } },
  { path: 'prescription/add', component: PrescriptionFormComponent,canActivate:[authGuard], data: { pageName: 'PrescriptionList' } },
  { path: 'prescription/edit/:id', component: PrescriptionFormComponent,canActivate:[authGuard], data: { pageName: 'PrescriptionList' } },
  { path: 'inventory', component: InventoryListComponent,canActivate:[authGuard], data: { pageName: 'Inventory' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PharmacyRoutingModule { }
