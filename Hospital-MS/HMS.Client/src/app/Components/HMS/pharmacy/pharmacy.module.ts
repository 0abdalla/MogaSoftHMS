import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PharmacyRoutingModule } from './pharmacy-routing.module';
import { MedicationListComponent } from './medication-list/medication-list.component';
import { PrescriptionFormComponent } from './prescription-form/prescription-form.component';
import { InventoryListComponent } from './inventory-list/inventory-list.component';


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
