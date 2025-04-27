import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffRoutingModule } from './staff-routing.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { StaffListComponent } from './staff-list/staff-list.component';
import { StaffDetailComponent } from './staff-detail/staff-detail.component';
import { StaffFormComponent } from './staff-form/staff-form.component';


@NgModule({
  declarations: [
    StaffListComponent,
    StaffDetailComponent,
    StaffFormComponent
  ],
  imports: [
    CommonModule,
    StaffRoutingModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    ToastModule
  ]
})
export class StaffModule { }
