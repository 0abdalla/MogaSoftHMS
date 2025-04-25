import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmergencyRoutingModule } from './emergency-routing.module';
import { EmergencyReceptionComponent } from './components/emergency-reception/emergency-reception.component';
import { UpdateStatusComponent } from './components/update-status/update-status.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';


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
