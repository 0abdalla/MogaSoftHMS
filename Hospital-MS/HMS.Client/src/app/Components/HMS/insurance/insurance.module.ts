import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InsuranceRoutingModule } from './insurance-routing.module';
import { InsuranceListComponent } from './insurance-list/insurance-list.component';
import { InsuranceFormComponent } from './insurance-form/insurance-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { NgxPaginationModule } from 'ngx-pagination';
import { InsuranceEditComponent } from './insurance-edit/insurance-edit.component';
import { SharedModule } from '../../../Shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    InsuranceListComponent,
    InsuranceFormComponent,
    InsuranceEditComponent
  ],
  imports: [
    CommonModule,
    InsuranceRoutingModule,
    ReactiveFormsModule,
    ToastModule,
    NgxPaginationModule,
    SharedModule,
    NgbModule
  ]
})
export class InsuranceModule { }
