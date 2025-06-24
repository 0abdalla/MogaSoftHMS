import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StaffListComponent } from './staff-list/staff-list.component';
import { StaffDetailComponent } from './staff-detail/staff-detail.component';
import { StaffFormComponent } from './staff-form/staff-form.component';
import { authGuard } from '../../../Auth/auth.guard';
import { StaffProgressionManagmentComponent } from './staff-progression-managment/staff-progression-managment.component';
import { StaffClassManagmentComponent } from './staff-class-managment/staff-class-managment.component';
import { StaffDepManagmentComponent } from './staff-dep-managment/staff-dep-managment.component';
import { StaffJobManagementComponent } from './staff-job-management/staff-job-management.component';
import { StaffLevelsComponent } from './staff-levels/staff-levels.component';
import { AttendanceFormComponent } from './attendance-form/attendance-form.component';
import { HrPenaltyComponent } from './hr-penalty/hr-penalty.component';
import { HrVacationComponent } from './hr-vacation/hr-vacation.component';

const routes: Routes = [
  { path: '', redirectTo : 'list' , pathMatch:"full" },
  { path: 'list' , component : StaffListComponent, canActivate: [authGuard], data: { pageName: 'staff-list' } },
  { path: 'detail/:id', component: StaffDetailComponent, canActivate: [authGuard], data: { pageName: 'staff-list' } },
  { path: 'add', component: StaffFormComponent, canActivate: [authGuard], data: { pageName: 'staff-add' } },
  { path: 'edit/:id', component: StaffFormComponent, canActivate: [authGuard], data: { pageName: 'staff-add' } },
  { path: 'progression' , component:StaffProgressionManagmentComponent , canActivate: [authGuard] },
  { path: 'classification' , component : StaffClassManagmentComponent , canActivate:[authGuard], data: { pageName: 'classification' } },
  { path: 'department-admin' , component:StaffDepManagmentComponent , canActivate:[authGuard], data: { pageName: 'department-admin' } },
  { path: 'job-management' , component:StaffJobManagementComponent , canActivate:[authGuard], data: { pageName: 'job-management' } },
  { path: 'job-levels' , component:StaffLevelsComponent , canActivate:[authGuard] },
  { path:"attendance", component : AttendanceFormComponent , data: { pageName: 'attendance' } },
  { path:"penalty", component : HrPenaltyComponent , data: { pageName: 'penalty' } },
  { path:"vacation", component : HrVacationComponent , data: { pageName: 'penalty' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffRoutingModule { }
