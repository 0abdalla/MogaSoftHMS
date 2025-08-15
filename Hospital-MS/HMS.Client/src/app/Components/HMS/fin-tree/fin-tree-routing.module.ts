import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ItemsComponent } from './inputs/items/items.component';
import { ItemsGroupComponent } from './inputs/items-group/items-group.component';
import { MainGroupComponent } from './inputs/main-group/main-group.component';
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
import { BankIndexComponent } from './action/bankActions/bank-index/bank-index.component';
import { AddNoticeComponent } from './action/bankActions/add-notice/add-notice.component';
import { DiscountNoticeComponent } from './action/bankActions/discount-notice/discount-notice.component';
import { RestrictionsComponent } from './action/restrictions/restrictions.component';
import { PurchaseRequestComponent } from './inputs/purchase-request/purchase-request.component';
import { PurchaseOrderComponent } from './inputs/purchase-order/purchase-order.component';
import { OffersComponent } from './inputs/offers/offers.component';
import { TreasuriesComponent } from './inputs/treasuries/treasuries.component';
import { StoresComponent } from './inputs/stores/stores.component';
import { StoresTypesComponent } from './inputs/stores-types/stores-types.component';
import { YearFinSettingsComponent } from './action/year-fin-settings/year-fin-settings.component';
import { IssueRequestComponent } from './action/issue-request/issue-request.component';
import { UnitsComponent } from './inputs/units/units.component';
import { authGuard } from '../../../Auth/auth.guard';

const routes: Routes = [
  {path:"items" , component : ItemsComponent, canActivate: [authGuard], data: { pageName: 'Items' }},
  {path:"items-group" , component : ItemsGroupComponent, canActivate: [authGuard], data: { pageName: 'ItemsGroup' }},
  {path:"main-groups" , component : MainGroupComponent, canActivate: [authGuard], data: { pageName: 'MainGroups' }},
  {path:"units" , component:UnitsComponent, canActivate: [authGuard], data: { pageName: 'Units' }},
  {path:"providers" , component : ProvidersComponent, canActivate: [authGuard], data: { pageName: 'Providers' }},
  {path:"purchase-request" , component : PurchaseRequestComponent, canActivate: [authGuard], data: { pageName: 'PurchaseRequest' }},
  {path:"purchase-order" , component : PurchaseOrderComponent, canActivate: [authGuard], data: { pageName: 'PurchaseOrder' }},
  {path:"offers" , component : OffersComponent, canActivate: [authGuard], data: { pageName: 'Offers' }},
  {path:"clients" , component : ClientsComponent, canActivate: [authGuard], data: { pageName: 'Clients' }},
  {path:"boxes" , component : BoxsComponent, canActivate: [authGuard], data: { pageName: 'Boxes' }},
  {path:"stores" , component : StoresComponent, canActivate: [authGuard], data: { pageName: 'Stores' }},
  {path:"stores-types" , component : StoresTypesComponent, canActivate: [authGuard], data: { pageName: 'StoreTypes' }},
  {path:"banks" , component : BanksComponent, canActivate: [authGuard], data: { pageName: 'Banks' }},
  {path:"accounts" , component : AccountsComponent, canActivate: [authGuard], data: { pageName: 'Accounts' }},
  {path:"treasuries" , component : TreasuriesComponent, canActivate: [authGuard], data: { pageName: 'Treasuries' }},
  // actions
  {path:"add-items" , component : AddItemsComponent, canActivate: [authGuard], data: { pageName: 'AddItems' }},
  {path:"issue-request" , component : IssueRequestComponent, canActivate: [authGuard], data: { pageName: 'IssueRequest' }},
  {path:"issue-items" , component : IssueItemsComponent, canActivate: [authGuard], data: { pageName: 'IssueItems' }},
  {path:"fetch-inventory" , component : FetchInventoryComponent, canActivate: [authGuard], data: { pageName: 'FetchInventory' }},
  // treasury
  {path:"treasury" , component : TreasuryIndexComponent, canActivate: [authGuard], data: { pageName: 'Treasury' }},
  {path:"treasury/supply-receipt" , component : SupplyReceiptComponent, canActivate: [authGuard], data: { pageName: 'Treasury' }},
  {path:"treasury/exchange-permission" , component : ExchangePremssionComponent, canActivate: [authGuard], data: { pageName: 'Treasury' }},
  // bank
  {path:'bank',component:BankIndexComponent, canActivate: [authGuard], data: { pageName: 'Bank' }},
  {path:'bank/add-notice',component:AddNoticeComponent, canActivate: [authGuard], data: { pageName: 'AddNotice' }},
  {path:'bank/discount-notice',component:DiscountNoticeComponent, canActivate: [authGuard], data: { pageName: 'DiscountNotice' }},
  {path:'restrictions',component:RestrictionsComponent, canActivate: [authGuard], data: { pageName: 'Restrictions' }},
  // 
  {path:'year-fin-settings',component:YearFinSettingsComponent, canActivate: [authGuard], data: { pageName: 'YeaFinSettings' }}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinTreeRoutingModule { }
