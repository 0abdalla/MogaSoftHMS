import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AgePipe } from '../Pipes/age.pipe';
import { SearchArryPipe } from '../Pipes/search-arry.pipe';
import { RoleCheckerDirective } from '../Directives/role-checker.directive';
import { InterceptorComponent } from '../Security/interceptor/interceptor.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { ArabicDayDatePipe } from '../Pipes/arabic-day-date.pipe';
import { OverviewCardComponent } from './overview-card/overview-card.component';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { PaginationComponent } from './pagination/pagination.component';
import { EmptyDataComponent } from './empty-data/empty-data.component';
import { SpinnerComponent } from './spinner/spinner.component';



@NgModule({
  declarations: [
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    ArabicDayDatePipe,
    OverviewCardComponent,
    BreadcrumbComponent,
    PaginationComponent,
    SpinnerComponent,
    EmptyDataComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    RouterModule
  ],
  exports: [
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent,
    HeaderComponent,
    FooterComponent,
    ArabicDayDatePipe,
    OverviewCardComponent,
    BreadcrumbComponent,
    PaginationComponent,
    EmptyDataComponent
  ],
  providers: [DatePipe]
})
export class SharedModule { }
