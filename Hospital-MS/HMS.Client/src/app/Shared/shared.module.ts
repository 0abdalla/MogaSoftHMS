import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AgePipe } from '../Pipes/age.pipe';
import { SearchArryPipe } from '../Pipes/search-arry.pipe';
import { RoleCheckerDirective } from '../Directives/role-checker.directive';
import { InterceptorComponent } from '../Security/interceptor/interceptor.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { PaginationComponent } from './pagination/pagination.component';
import { FiltersComponent } from './filters/filters.component';
import { FormsModule } from '@angular/forms';
import { SpinnerComponent } from './spinner/spinner.component';
import { EmptyDataComponent } from './empty-data/empty-data.component';
import { GeneralSelectorComponent } from './general-selector/general-selector.component';
import { GoToAccountReportDirective } from '../Directives/go-to-account-report.directive';
import { DropDownFormControlComponent } from './drop-down-form-control/drop-down-form-control.component';
import { ColorWithStatusDirectiveDirective } from '../Directives/color-with-status.directive.directive';
import { WorkflowStatusDirective } from '../Directives/workflow-status.directive';
import { NgbModule, NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { TagComponent } from './tag/tag.component';
import { DayToArabicPipe } from '../Pipes/day-to-arabic.pipe';
import { MedicalTypeToArabicPipe } from '../Pipes/medical-type-to-arabic.pipe';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';


@NgModule({
  declarations: [
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent,
    HeaderComponent,
    FooterComponent,
    BreadcrumbComponent,
    PaginationComponent,
    FiltersComponent,
    SpinnerComponent,
    EmptyDataComponent,
    GeneralSelectorComponent,
    GoToAccountReportDirective,
    DropDownFormControlComponent,
    ColorWithStatusDirectiveDirective,
    WorkflowStatusDirective,
    TagComponent,
    DayToArabicPipe,
    MedicalTypeToArabicPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    NgbPopoverModule,
    NgxDaterangepickerMd.forRoot()
  ],
  exports: [
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent,
    HeaderComponent,
    FooterComponent,
    BreadcrumbComponent,
    PaginationComponent,
    FiltersComponent,
    EmptyDataComponent,
    GeneralSelectorComponent,
    GoToAccountReportDirective,
    DropDownFormControlComponent,
    ColorWithStatusDirectiveDirective,
    WorkflowStatusDirective,
    TagComponent,
    DayToArabicPipe,
    MedicalTypeToArabicPipe
  ],
  providers: [DatePipe]
})
export class SharedModule { }
