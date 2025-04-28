import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StaffListComponent } from './staff-list/staff-list.component';
import { StaffDetailComponent } from './staff-detail/staff-detail.component';
import { StaffFormComponent } from './staff-form/staff-form.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  { path: '', redirectTo : 'list' , pathMatch:"full" },
  { path: 'list' , component : StaffListComponent, canActivate: [authGuard]},
  { path: 'detail/:id', component: StaffDetailComponent, canActivate: [authGuard] },
  { path: 'add', component: StaffFormComponent, canActivate: [authGuard] },
  { path: 'edit/:id', component: StaffFormComponent, canActivate: [authGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffRoutingModule { }
