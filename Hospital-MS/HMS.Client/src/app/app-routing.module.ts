import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { LoginComponent } from './Auth/login/login.component';
import { RegisterComponent } from './Auth/register/register.component';
import { HomeComponent } from './Components/HMS/dashboard/home/home.component';
import { authGuard } from './Auth/auth.guard';
import { loadingInterceptor } from './Security/loading.interceptor';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {path:"login" , component : LoginComponent},
  {path:"register" , component:RegisterComponent},
  { path: 'home', component: HomeComponent, canActivate: [authGuard], },
  // {
  //   path: 'patients',
  //   loadChildren: () => import('./features/patients/patients.module').then(m => m.PatientsModule),
  //   canActivate: [authGuard],
  // },
  // {
  //   path: 'staff',
  //   loadChildren: () => import('./features/staff/staff.module').then(m => m.StaffModule),
  //   canActivate: [authGuard],
  // },
  // {
  //   path: 'appointments',
  //   loadChildren: () => import('./features/appointments/appointments.module').then(m => m.AppointmentsModule),
  //   canActivate: [authGuard],
  // },
  // { 
  //   path:"emergency" , 
  // loadChildren: () => import('./features/emergency/emergency.module').then(m => m.EmergencyModule),
  // canActivate: [authGuard]
  // },
  // { 
  //   path:"insurance",
  //   loadChildren: () => import('./features/insurance/insurance.module').then(m => m.InsuranceModule),
  //   canActivate: [authGuard],
  // },
  // {
  //   path: 'inpatient',
  //   loadChildren: () => import('./features/inpatient/inpatient.module').then(m => m.InpatientModule),
  //   canActivate: [authGuard],
  // },
  // {
  //   path: 'pharmacy',
  //   loadChildren: () => import('./features/pharmacy/pharmacy.module').then(m => m.PharmacyModule),
  //   canActivate: [authGuard],
  // },
  // {
  //   path: 'financial',
  //   loadChildren: () => import('./features/financial/financial.module').then(m => m.FinancialModule),
  //   canActivate: [authGuard],
  // },
  // {path:"reports" , loadChildren: () => import('./features/reports/reports.module').then(m => m.ReportsModule), canActivate: [authGuard]},
  // {path:"settings" , loadChildren:()=> import('./features/settings/settings.module').then(m => m.SettingsModule) , canActivate:[authGuard]},
  { path: '**', redirectTo: '/dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules, initialNavigation: 'enabledBlocking', scrollPositionRestoration: "enabled" })],
  exports: [RouterModule],
  providers: [
    provideHttpClient(withInterceptors([loadingInterceptor]))
  ]
})
export class AppRoutingModule { }
