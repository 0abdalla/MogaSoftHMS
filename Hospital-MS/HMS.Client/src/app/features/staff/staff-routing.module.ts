import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StaffDetailComponent } from './components/staff-detail/staff-detail.component';
import { StaffFormComponent } from './components/staff-form/staff-form.component';
import { StaffListComponent } from './components/staff-list/staff-list.component';
import { authGuard } from '../../core/auth/auth.guard';

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
