import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../Auth/auth.guard';
import { HomeComponent } from './dashboard/home/home.component';
import { HmsLayoutComponent } from './hms-layout.component';

const routes: Routes = [

  {
    path: '',
    component: HmsLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'home',
        component: HomeComponent,
        canActivate: [authGuard],
      },
      {
        path: 'patients',
        loadChildren: () => import('./patients/patients.module').then(m => m.PatientsModule),
        canActivate: [authGuard],
      },
      {
        path: 'staff',
        loadChildren: () => import('./staff/staff.module').then(m => m.StaffModule),
        canActivate: [authGuard],
      },
      {
        path: 'appointments',
        loadChildren: () => import('./appointments/appointments.module').then(m => m.AppointmentsModule),
        canActivate: [authGuard],
      },
      {
        path: "emergency",
        loadChildren: () => import('./emergency/emergency.module').then(m => m.EmergencyModule),
        canActivate: [authGuard]
      },
      {
        path: "insurance",
        loadChildren: () => import('./insurance/insurance.module').then(m => m.InsuranceModule),
        canActivate: [authGuard],
      },
      {
        path: 'inpatient',
        loadChildren: () => import('./inpatient/inpatient.module').then(m => m.InpatientModule),
        canActivate: [authGuard],
      },
      {
        path: 'pharmacy',
        loadChildren: () => import('./pharmacy/pharmacy.module').then(m => m.PharmacyModule),
        canActivate: [authGuard],
      },
      {
        path: 'financial',
        loadChildren: () => import('./financial/financial.module').then(m => m.FinancialModule),
        canActivate: [authGuard],
      },
      { path: "reports", loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule), canActivate: [authGuard] },
      { path: "settings", loadChildren: () => import('./settings/settings.module').then(m => m.SettingsModule), canActivate: [authGuard] },
      { path: '**', redirectTo: 'home' },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
    ]
  },
  { path: '', redirectTo: '', pathMatch: 'full' },
  { path: '**', redirectTo: '', pathMatch: 'full' },



];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HmsRoutingModule { }
