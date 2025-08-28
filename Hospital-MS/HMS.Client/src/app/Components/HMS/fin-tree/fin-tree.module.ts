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
import { FetchInventoryComponent } from './action/fetch-inventory/fetch-inventory.component';
import { TreasuryIndexComponent } from './action/treasury/treasury-index/treasury-index.component';
import { SupplyReceiptComponent } from './action/treasury/supply-receipt/supply-receipt.component';
import { ExchangePremssionComponent } from './action/treasury/exchange-premssion/exchange-premssion.component';
import { AddNoticeComponent } from './action/bankActions/add-notice/add-notice.component';
import { DiscountNoticeComponent } from './action/bankActions/discount-notice/discount-notice.component';
import { BankIndexComponent } from './action/bankActions/bank-index/bank-index.component';
import { RestrictionsComponent } from './action/restrictions/restrictions.component';
import { PurchaseRequestComponent } from './inputs/purchase-request/purchase-request.component';
import { OffersComponent } from './inputs/offers/offers.component';
import { PurchaseOrderComponent } from './inputs/purchase-order/purchase-order.component';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { TreasuriesComponent } from './inputs/treasuries/treasuries.component';
import { StoresComponent } from './inputs/stores/stores.component';
import { StoresTypesComponent } from './inputs/stores-types/stores-types.component';
import { YearFinSettingsComponent } from './action/year-fin-settings/year-fin-settings.component';
import { IssueRequestComponent } from './action/issue-request/issue-request.component';
import { IsTodayPipe } from '../../../Pipes/is-today.pipe';
import { UnitsComponent } from './inputs/units/units.component';
import { AccountGuidanceComponent } from './inputs/account-guidance/account-guidance.component';


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
    FetchInventoryComponent,
    TreasuryIndexComponent,
    SupplyReceiptComponent,
    ExchangePremssionComponent,
    AddNoticeComponent,
    DiscountNoticeComponent,
    BankIndexComponent,
    RestrictionsComponent,
    PurchaseRequestComponent,
    PurchaseOrderComponent,
    OffersComponent,
    TreasuriesComponent,
    StoresComponent,
    StoresTypesComponent,
    YearFinSettingsComponent,
    IssueRequestComponent,
    IsTodayPipe,
    UnitsComponent,
    AccountGuidanceComponent
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
    // NgModule
    NgbCollapseModule
  ]
})
export class FinTreeModule { }
