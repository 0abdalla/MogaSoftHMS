import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AgePipe } from '../Pipes/age.pipe';
import { SearchArryPipe } from '../Pipes/search-arry.pipe';
import { RoleCheckerDirective } from '../Directives/role-checker.directive';
import { InterceptorComponent } from '../Security/interceptor/interceptor.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { MessageService } from 'primeng/api';



@NgModule({
  declarations: [
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent,
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    AgePipe,
    SearchArryPipe,
    RoleCheckerDirective,
    InterceptorComponent,
    HeaderComponent,
    FooterComponent
  ],
  providers: [DatePipe]
})
export class SharedModule { }
