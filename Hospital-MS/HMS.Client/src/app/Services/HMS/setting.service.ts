import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AccountTreeModel } from '../../Models/HMS/AccountTree';
import { GeneralSelectorModel } from '../../Models/Generics/GeneralSelectorModel';

@Injectable({
  providedIn: 'root'
})
export class SettingService {
  baseUrl = environment.baseUrl
  constructor(private http: HttpClient) { }

  AddNewAccount(model: AccountTreeModel) {
    return this.http.post<any>(this.baseUrl + 'AccountTree/AddNewAccount', model);
  }

  EditAccountTree(accountId: number, model: AccountTreeModel) {
    return this.http.post<any>(this.baseUrl + 'AccountTree/EditAccountTree?AccountId=' + accountId, model);
  }

  GenerateAccountNumber(parentAccountId: number) {
    return this.http.get<string>(this.baseUrl + 'AccountTree/GenerateAccountNumber?ParentAccountId=' + parentAccountId);
  }

  GetAccountTypes() {
    return this.http.get<GeneralSelectorModel[]>(this.baseUrl + 'AccountTree/GetAccountTypes');
  }

  GetCurrencySelector() {
    return this.http.get<GeneralSelectorModel[]>(this.baseUrl + 'AccountTree/GetCurrencySelector');
  }

  GetAccountsSelector(IsGroup: boolean = null) {
    const param = IsGroup !== null ? `?IsGroup=${IsGroup}` : '';
    return this.http.get<any[]>(this.baseUrl + 'AccountTree/GetAccountsSelector' + param);
  }

  GetCostCenterSelector(IsParent: boolean = false) {
    return this.http.get<any[]>(this.baseUrl + 'AccountTree/GetCostCenterSelector?IsParent=' + IsParent);
  }

  GetAccountTreeHierarchicalData(SearchText: string) {
    return this.http.get<any>(this.baseUrl + 'AccountTree/GetAccountTreeHierarchicalData?SearchText=' + SearchText);
  }

  DeleteAccountTree(accountId: number) {
    return this.http.get<any>(this.baseUrl + 'AccountTree/DeleteAccountTree?AccountId=' + accountId);
  }
}
