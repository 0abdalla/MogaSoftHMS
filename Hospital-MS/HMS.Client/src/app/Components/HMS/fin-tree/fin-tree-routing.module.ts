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

const routes: Routes = [
  {path:"items" , component : ItemsComponent},
  {path:"items-group" , component : ItemsGroupComponent},
  {path:"main-groups" , component : MainGroupComponent},
  {path:"providers" , component : ProvidersComponent},
  {path:"purchase-request" , component : PurchaseRequestComponent},
  {path:"purchase-order" , component : PurchaseOrderComponent},
  {path:"offers" , component : OffersComponent},
  {path:"clients" , component : ClientsComponent},
  {path:"boxes" , component : BoxsComponent},
  {path:"banks" , component : BanksComponent},
  {path:"accounts" , component : AccountsComponent},
  // actions
  {path:"add-items" , component : AddItemsComponent},
  {path:"issue-items" , component : IssueItemsComponent},
  {path:"fetch-inventory" , component : FetchInventoryComponent},
  // treasury
  {path:"treasury" , component : TreasuryIndexComponent},
  {path:"treasury/supply-receipt" , component : SupplyReceiptComponent},
  {path:"treasury/exchange-permission" , component : ExchangePremssionComponent},
  // bank
  {path:'bank',component:BankIndexComponent},
  {path:'bank/add-notice',component:AddNoticeComponent},
  {path:'bank/discount-notice',component:DiscountNoticeComponent},
  {path:'restrictions',component:RestrictionsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinTreeRoutingModule { }
