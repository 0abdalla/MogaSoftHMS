import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InpatientRoutingModule } from './inpatient-routing.module';
import { AdmissionListComponent } from './admission-list/admission-list.component';
import { AdmissionDetailComponent } from './admission-detail/admission-detail.component';
import { SurgeryFormComponent } from './surgery-form/surgery-form.component';


@NgModule({
  declarations: [
    AdmissionListComponent,
    AdmissionDetailComponent,
    SurgeryFormComponent
  ],
  imports: [
    CommonModule,
    InpatientRoutingModule
  ]
})
export class InpatientModule { }
