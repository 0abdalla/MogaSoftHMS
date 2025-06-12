import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../../Auth/auth.guard';
import { HmsLayoutComponent } from './hms-layout.component';
import { HmsHomeComponent } from './hms-home/hms-home.component';

const routes: Routes = [
  {
    path: '',
    component: HmsLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'home', component: HmsHomeComponent },
      { path: 'patients', loadChildren: () => import('./patients/patients.module').then(m => m.PatientsModule) },
      { path: 'staff', loadChildren: () => import('./staff/staff.module').then(m => m.StaffModule) },
      { path: 'appointments', loadChildren: () => import('./appointments/appointments.module').then(m => m.AppointmentsModule) },
      { path: 'emergency', loadChildren: () => import('./emergency/emergency.module').then(m => m.EmergencyModule) },
      { path: 'insurance', loadChildren: () => import('./insurance/insurance.module').then(m => m.InsuranceModule) },
      { path: 'inpatient', loadChildren: () => import('./inpatient/inpatient.module').then(m => m.InpatientModule) },
      { path: 'pharmacy', loadChildren: () => import('./pharmacy/pharmacy.module').then(m => m.PharmacyModule) },
      { path: 'financial', loadChildren: () => import('./financial/financial.module').then(m => m.FinancialModule) },
      { path: 'reports', loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule) },
      { path: 'settings', loadChildren: () => import('./settings/settings.module').then(m => m.SettingsModule) },
      { path: 'fin-tree', loadChildren: () => import('./fin-tree/fin-tree.module').then(m => m.FinTreeModule) },
      { path: '**', redirectTo: 'home' },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HmsRoutingModule { }
