import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MedicationListComponent } from './medication-list/medication-list.component';
import { PrescriptionFormComponent } from './prescription-form/prescription-form.component';
import { InventoryListComponent } from './inventory-list/inventory-list.component';

const routes: Routes = [
  { path: '', component: MedicationListComponent },
  { path: 'prescription/add', component: PrescriptionFormComponent },
  { path: 'prescription/edit/:id', component: PrescriptionFormComponent },
  { path: 'inventory', component: InventoryListComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PharmacyRoutingModule { }
