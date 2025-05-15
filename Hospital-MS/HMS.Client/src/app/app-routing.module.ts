import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { LoginComponent } from './Auth/login/login.component';
import { RegisterComponent } from './Auth/register/register.component';
import { loadingInterceptor } from './Security/loading.interceptor';
import { authGuard } from './Auth/auth.guard';
import { NotAuthorizedComponent } from './Auth/not-authorized/not-authorized.component';

const routes: Routes = [
  { path: "login", component: LoginComponent },
  { path: "register", component: RegisterComponent },
  { path: "register", component: RegisterComponent },
  { path: 'not-authorized', component: NotAuthorizedComponent },
  {
    path: 'hms',
    loadChildren: () => import('./Components/HMS/hms.module').then(m => m.HmsModule),
    canActivate: [authGuard],
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login', pathMatch: 'full' }
];
//
@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules, initialNavigation: 'enabledBlocking', scrollPositionRestoration: "enabled" })],
  exports: [RouterModule],
  providers: [
    provideHttpClient(withInterceptors([loadingInterceptor]))
  ]
})
export class AppRoutingModule { }
