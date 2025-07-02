import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffRoutingModule } from './staff-routing.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { StaffListComponent } from './staff-list/staff-list.component';
import { StaffDetailComponent } from './staff-detail/staff-detail.component';
import { StaffFormComponent } from './staff-form/staff-form.component';
import { StaffClassManagmentComponent } from './staff-class-managment/staff-class-managment.component';
import { StaffDepManagmentComponent } from './staff-dep-managment/staff-dep-managment.component';
import { StaffProgressionManagmentComponent } from './staff-progression-managment/staff-progression-managment.component';
import { StaffJobManagementComponent } from './staff-job-management/staff-job-management.component';
import { StaffLevelsComponent } from './staff-levels/staff-levels.component';
import { AttendanceFormComponent } from './attendance-form/attendance-form.component';
import { TableModule } from 'primeng/table';
import { SharedModule } from '../../../Shared/shared.module';
import { HrPenaltyComponent } from './hr-penalty/hr-penalty.component';
import { HrVacationComponent } from './hr-vacation/hr-vacation.component';
import { HrAdvancesComponent } from './hr-advances/hr-advances.component';
import { HrEmployeesSalariesComponent } from './hr-employees-salaries/hr-employees-salaries.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    StaffListComponent,
    StaffDetailComponent,
    StaffFormComponent,
    StaffClassManagmentComponent,
    StaffDepManagmentComponent,
    StaffProgressionManagmentComponent,
    StaffJobManagementComponent,
    StaffLevelsComponent,
    AttendanceFormComponent,
    HrPenaltyComponent,
    HrVacationComponent,
    HrAdvancesComponent,
    HrEmployeesSalariesComponent
  ],
  imports: [
    CommonModule,
    StaffRoutingModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    ToastModule,
    TableModule,
    FormsModule,
    SharedModule,
    NgbModule
  ]
})
export class StaffModule { }
