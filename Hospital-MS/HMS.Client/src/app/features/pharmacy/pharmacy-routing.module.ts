import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InventoryListComponent } from './components/inventory-list/inventory-list.component';
import { MedicationListComponent } from './components/medication-list/medication-list.component';
import { PrescriptionFormComponent } from './components/prescription-form/prescription-form.component';

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
