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

const routes: Routes = [
  { path: '', redirectTo : 'list' , pathMatch:"full" },
  { path: 'list' , component : StaffListComponent, canActivate: [authGuard]},
  { path: 'detail/:id', component: StaffDetailComponent, canActivate: [authGuard] },
  { path: 'add', component: StaffFormComponent, canActivate: [authGuard] },
  { path: 'edit/:id', component: StaffFormComponent, canActivate: [authGuard] },
  { path: 'progression' , component:StaffProgressionManagmentComponent , canActivate: [authGuard] },
  { path: 'classification' , component : StaffClassManagmentComponent , canActivate:[authGuard]},
  { path: 'department-admin' , component:StaffDepManagmentComponent , canActivate:[authGuard]},
  { path: 'job-management' , component:StaffJobManagementComponent , canActivate:[authGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffRoutingModule { }
