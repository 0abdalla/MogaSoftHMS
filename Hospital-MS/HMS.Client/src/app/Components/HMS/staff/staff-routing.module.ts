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
import { HrAdvancesComponent } from './hr-advances/hr-advances.component';
import { HrSalariesComponent } from './hr-salaries/hr-salaries.component';

const routes: Routes = [
  { path: '', redirectTo : 'list' , pathMatch:"full" },
  { path: 'list' , component : StaffListComponent, canActivate: [authGuard], data: { pageName: 'StaffList' } },
  { path: 'detail/:id', component: StaffDetailComponent, canActivate: [authGuard], data: { pageName: 'StaffList' } },
  { path: 'add', component: StaffFormComponent, canActivate: [authGuard], data: { pageName: 'StaffList' } },
  { path: 'edit/:id', component: StaffFormComponent, canActivate: [authGuard], data: { pageName: 'StaffList' } },
  { path: 'progression' , component:StaffProgressionManagmentComponent , canActivate: [authGuard], data: { pageName: 'Progression' } },
  { path: 'classification' , component : StaffClassManagmentComponent , canActivate:[authGuard], data: { pageName: 'Classification' } },
  { path: 'department-admin' , component:StaffDepManagmentComponent , canActivate:[authGuard], data: { pageName: 'DepartmentAdmin' } },
  { path: 'job-management' , component:StaffJobManagementComponent , canActivate:[authGuard], data: { pageName: 'JobManagement' } },
  { path: 'job-levels' , component:StaffLevelsComponent , canActivate:[authGuard] },
  { path:"attendance", component : AttendanceFormComponent , data: { pageName: 'Attendance' } },
  { path:"penalty", component : HrPenaltyComponent , data: { pageName: 'Penalty' } },
  { path:"vacation", component : HrVacationComponent , data: { pageName: 'Penalty' } },
  { path:"advances", component : HrAdvancesComponent , data: { pageName: 'Penalty' } },
  { path:"salaries", component : HrSalariesComponent , data: { pageName: 'Penalty' } },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffRoutingModule { }
