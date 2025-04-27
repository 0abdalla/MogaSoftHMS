import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmergencyRoutingModule } from './emergency-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { EmergencyReceptionComponent } from './emergency-reception/emergency-reception.component';
import { UpdateStatusComponent } from './update-status/update-status.component';


@NgModule({
  declarations: [
    EmergencyReceptionComponent,
    UpdateStatusComponent
  ],
  imports: [
    CommonModule,
    EmergencyRoutingModule,
    ReactiveFormsModule,
    ToastModule
  ]
})
export class EmergencyModule { }
