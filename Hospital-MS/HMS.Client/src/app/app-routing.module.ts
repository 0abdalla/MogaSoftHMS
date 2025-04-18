import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { HomeComponent } from './features/dashboard/home/home.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: HomeComponent, canActivate: [authGuard], },
  {
    path: 'patients',
    loadChildren: () => import('./features/patients/patients.module').then(m => m.PatientsModule),
    canActivate: [authGuard],
  },
  {
    path: 'staff',
    loadChildren: () => import('./features/staff/staff.module').then(m => m.StaffModule),
    canActivate: [authGuard],
  },
  {
    path: 'appointments',
    loadChildren: () => import('./features/appointments/appointments.module').then(m => m.AppointmentsModule),
    canActivate: [authGuard],
  },
  {
    path: 'inpatient',
    loadChildren: () => import('./features/inpatient/inpatient.module').then(m => m.InpatientModule),
    canActivate: [authGuard],
  },
  {
    path: 'pharmacy',
    loadChildren: () => import('./features/pharmacy/pharmacy.module').then(m => m.PharmacyModule),
    canActivate: [authGuard],
  },
  {
    path: 'financial',
    loadChildren: () => import('./features/financial/financial.module').then(m => m.FinancialModule),
    canActivate: [authGuard],
  },
  { path: '**', redirectTo: '/dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
