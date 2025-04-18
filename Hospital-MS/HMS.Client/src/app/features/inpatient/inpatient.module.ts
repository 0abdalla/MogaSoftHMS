import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InpatientRoutingModule } from './inpatient-routing.module';
import { AdmissionListComponent } from './components/admission-list/admission-list.component';
import { AdmissionDetailComponent } from './components/admission-detail/admission-detail.component';
import { SurgeryFormComponent } from './components/surgery-form/surgery-form.component';


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
