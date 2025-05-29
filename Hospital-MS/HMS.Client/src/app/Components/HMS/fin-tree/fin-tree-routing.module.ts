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

const routes: Routes = [
  {path:"items" , component : ItemsComponent},
  {path:"items-group" , component : ItemsGroupComponent},
  {path:"main-groups" , component : MainGroupComponent},
  {path:"providers" , component : ProvidersComponent},
  {path:"clients" , component : ClientsComponent},
  {path:"boxes" , component : BoxsComponent},
  {path:"banks" , component : BanksComponent},
  {path:"accounts" , component : AccountsComponent},
  // actions
  {path:"add-items" , component : AddItemsComponent},
  {path:"issue-items" , component : IssueItemsComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinTreeRoutingModule { }
