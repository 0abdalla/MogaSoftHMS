import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffRoutingModule } from './staff-routing.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { StaffListComponent } from './staff-list/staff-list.component';
import { StaffDetailComponent } from './staff-detail/staff-detail.component';
import { StaffFormComponent } from './staff-form/staff-form.component';
import { StaffClassManagmentComponent } from './staff-class-managment/staff-class-managment.component';
import { StaffDepManagmentComponent } from './staff-dep-managment/staff-dep-managment.component';
import { StaffProgressionManagmentComponent } from './staff-progression-managment/staff-progression-managment.component';
import { StaffJobManagementComponent } from './staff-job-management/staff-job-management.component';
import { StaffLevelsComponent } from './staff-levels/staff-levels.component';


@NgModule({
  declarations: [
    StaffListComponent,
    StaffDetailComponent,
    StaffFormComponent,
    StaffClassManagmentComponent,
    StaffDepManagmentComponent,
    StaffProgressionManagmentComponent,
    StaffJobManagementComponent,
    StaffLevelsComponent
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
