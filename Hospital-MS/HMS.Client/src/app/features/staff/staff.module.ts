import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffRoutingModule } from './staff-routing.module';
import { StaffListComponent } from './components/staff-list/staff-list.component';
import { StaffDetailComponent } from './components/staff-detail/staff-detail.component';
import { StaffFormComponent } from './components/staff-form/staff-form.component';


@NgModule({
  declarations: [
    StaffListComponent,
    StaffDetailComponent,
    StaffFormComponent
  ],
  imports: [
    CommonModule,
    StaffRoutingModule
  ]
})
export class StaffModule { }
