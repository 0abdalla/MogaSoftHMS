import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FinTreeRoutingModule } from './fin-tree-routing.module';
import { ItemsComponent } from './inputs/items/items.component';
import { MainGroupComponent } from './inputs/main-group/main-group.component';
import { ItemsGroupComponent } from './inputs/items-group/items-group.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../Shared/shared.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { ToastModule } from 'primeng/toast';
import { ProvidersComponent } from './inputs/providers/providers.component';
import { ClientsComponent } from './inputs/clients/clients.component';
import { BoxsComponent } from './inputs/boxs/boxs.component';
import { BanksComponent } from './inputs/banks/banks.component';
import { AccountsComponent } from './inputs/accounts/accounts.component';
import { AddItemsComponent } from './action/add-items/add-items.component';
import { IssueItemsComponent } from './action/issue-items/issue-items.component';


@NgModule({
  declarations: [
    ItemsComponent,
    MainGroupComponent,
    ItemsGroupComponent,
    ProvidersComponent,
    ClientsComponent,
    BoxsComponent,
    BanksComponent,
    AccountsComponent,
    AddItemsComponent,
    IssueItemsComponent,
  ],
  imports: [
    CommonModule,
    FinTreeRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    NgSelectModule,
    NgxPaginationModule,
    ToastModule,
  ]
})
export class FinTreeModule { }
