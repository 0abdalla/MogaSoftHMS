import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StaffDetailComponent } from './components/staff-detail/staff-detail.component';
import { StaffFormComponent } from './components/staff-form/staff-form.component';
import { StaffListComponent } from './components/staff-list/staff-list.component';

const routes: Routes = [
  { path: '', component: StaffListComponent },
  { path: 'detail/:id', component: StaffDetailComponent },
  { path: 'add', component: StaffFormComponent },
  { path: 'edit/:id', component: StaffFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffRoutingModule { }
