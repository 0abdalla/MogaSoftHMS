import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InsuranceRoutingModule } from './insurance-routing.module';
import { InsuranceListComponent } from './insurance-list/insurance-list.component';
import { InsuranceFormComponent } from './insurance-form/insurance-form.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    InsuranceListComponent,
    InsuranceFormComponent
  ],
  imports: [
    CommonModule,
    InsuranceRoutingModule,
    ReactiveFormsModule
  ]
})
export class InsuranceModule { }
