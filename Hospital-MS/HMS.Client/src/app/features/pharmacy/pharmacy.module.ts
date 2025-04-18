import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PharmacyRoutingModule } from './pharmacy-routing.module';
import { MedicationListComponent } from './components/medication-list/medication-list.component';
import { PrescriptionFormComponent } from './components/prescription-form/prescription-form.component';
import { InventoryListComponent } from './components/inventory-list/inventory-list.component';


@NgModule({
  declarations: [
    MedicationListComponent,
    PrescriptionFormComponent,
    InventoryListComponent
  ],
  imports: [
    CommonModule,
    PharmacyRoutingModule
  ]
})
export class PharmacyModule { }
