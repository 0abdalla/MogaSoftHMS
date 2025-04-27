import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from '../Components/HMS/dashboard/home/home.component';
import { HeaderComponent } from './header/header.component';
import { ChartModule } from 'angular-highcharts';
import { AgePipe } from '../Pipes/age.pipe';
import { SearchArryPipe } from '../Pipes/search-arry.pipe';
import { RoleCheckerDirective } from '../Directives/role-checker.directive';
import { InterceptorComponent } from '../Security/interceptor/interceptor.component';



@NgModule({
  declarations: [
    SidebarComponent,
    FooterComponent,
    HomeComponent,
    HeaderComponent,
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent
  ],
  imports: [
    CommonModule,
    ChartModule
  ],
  exports: [
    SidebarComponent,
    FooterComponent,
    HeaderComponent,
    HomeComponent,
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent
  ]
})
export class SharedModule { }
