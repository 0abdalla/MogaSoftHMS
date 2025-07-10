import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { PagingFilterModel } from '../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../Models/Generics/PagedResponseModel';
@Injectable({
  providedIn: 'root'
})
export class FinancialService {
  baseUrl = environment.baseUrl;
  constructor(private http : HttpClient) { }
  getMainGroups(pagingFilter : PagingFilterModel){
    return this.http.get<any>(this.baseUrl + 'MainGroups?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getMainGroupsById(id:number){
    return this.http.get<any>(this.baseUrl + 'MainGroups/' + id);
  }
  addMainGroups(MainGroups:any){
    return this.http.post<any>(this.baseUrl + 'MainGroups', MainGroups);
  }
  updateMainGroups(id:number,MainGroups:any){
    return this.http.put<any>(this.baseUrl + 'MainGroups/' + id, MainGroups);
  }
  deleteMainGroups(id:number){
    return this.http.delete<any>(this.baseUrl + 'MainGroups/' + id);
  }
  // 
  getItemsGroups(pagingFilter : PagingFilterModel){
    return this.http.get<any>(this.baseUrl + 'ItemsGroups?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getItemsGroupsById(id:number){
    return this.http.get<any>(this.baseUrl + 'ItemsGroups/' + id);
  }
  addItemsGroups(ItemsGroups:any){
    return this.http.post<any>(this.baseUrl + 'ItemsGroups', ItemsGroups);
  }
  updateItemsGroups(id:number,ItemsGroups:any){
    return this.http.put<any>(this.baseUrl + 'ItemsGroups/' + id, ItemsGroups);
  }
  deleteItemsGroups(id:number){
    return this.http.delete<any>(this.baseUrl + 'ItemsGroups/' + id);
  }
  // 
  getItems(pagingFilter: PagingFilterModel){
    return this.http.get<any>(this.baseUrl + 'Items?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getItemsById(id:number){
    return this.http.get<any>(this.baseUrl + 'Items/' + id);
  }
  addItem(item:any){
    return this.http.post<any>(this.baseUrl + 'Items', item);
  }
  updateItem(id:number,item:any){
    return this.http.put<any>(this.baseUrl + 'Items/' + id, item);
  }
  deleteItem(id:number){
    return this.http.delete<any>(this.baseUrl + 'Items/' + id);
  }
  // 
  getSuppliers(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'Suppliers?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getSuppliersById(id:number){
    return this.http.get<any>(this.baseUrl + 'Suppliers/' + id);
  }
  addSupplier(supplier:any){
    return this.http.post<any>(this.baseUrl + 'Suppliers', supplier);
  }
  updateSupplier(id:number,supplier:any){
    return this.http.put<any>(this.baseUrl + 'Suppliers/' + id, supplier);
  }
  deleteSupplier(id:number){
    return this.http.delete<any>(this.baseUrl + 'Suppliers/' + id);
  }
  // 
  getStores(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'Stores?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getStoresById(id:number){
    return this.http.get<any>(this.baseUrl + 'Stores/' + id);
  }
  addStore(store:any){
    return this.http.post<any>(this.baseUrl + 'Stores', store);
  }
  updateStore(id:number,store:any){
    return this.http.put<any>(this.baseUrl + 'Stores/' + id, store);
  }
  deleteStore(id:number){
    return this.http.delete<any>(this.baseUrl + 'Stores/' + id);
  }
  // 
  getStoresTypes(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'StoreTypes?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getStoresTypesById(id:number){
    return this.http.get<any>(this.baseUrl + 'StoreTypes/' + id);
  }
  addStoreType(storeType:any){
    return this.http.post<any>(this.baseUrl + 'StoreTypes', storeType);
  }
  updateStoreType(id:number,storeType:any){
    return this.http.put<any>(this.baseUrl + 'StoreTypes/' + id, storeType);
  }
  deleteStoreType(id:number){
    return this.http.delete<any>(this.baseUrl + 'StoreTypes/' + id);
  }
  // 
  getPurchaseRequests(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'PurchaseRequests?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getPurchaseRequestsById(id:number){
    return this.http.get<any>(this.baseUrl + 'PurchaseRequests/' + id);
  }
  addPurchaseRequest(purchaseRequest:any){
    return this.http.post<any>(this.baseUrl + 'PurchaseRequests', purchaseRequest);
  }
  updatePurchaseRequest(id:number,purchaseRequest:any){
    return this.http.put<any>(this.baseUrl + 'PurchaseRequests/' + id, purchaseRequest);
  }
  deletePurchaseRequest(id:number){
    return this.http.delete<any>(this.baseUrl + 'PurchaseRequests/' + id);
  }
  // 
  getapprovedPurchaseRequests(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'PurchaseRequests/approved?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  
  // 
  getPurchaseOrders(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'PurchaseOrders?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getPurchaseOrdersById(id:number){
    return this.http.get<any>(this.baseUrl + 'PurchaseOrders/' + id);
  }
  addPurchaseOrder(purchaseOrder:any){
    return this.http.post<any>(this.baseUrl + 'PurchaseOrders', purchaseOrder);
  }
  updatePurchaseOrder(id:number,purchaseOrder:any){
    return this.http.put<any>(this.baseUrl + 'PurchaseOrders/' + id, purchaseOrder);
  }
  deletePurchaseOrder(id:number){
    return this.http.delete<any>(this.baseUrl + 'PurchaseOrders/' + id);
  }
  // 
  getOffers(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'PriceQuotations?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getOffersById(id:number){
    return this.http.get<any>(this.baseUrl + 'PriceQuotations/' + id);
  }
  getPriceQuotationById(id:number){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + `PriceQuotations/GetByPurchaseRequest/${id}`);
  }
  getApprovedPriceQutations(){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + `PriceQuotations/Approved`);
  }
  putPriceQutataion(id:number , price:any){
    return this.http.put<any>(this.baseUrl + `PriceQuotations/SubmitPriceQuotation/${id}`, price);
  }
  addOffer(offer:any){
    return this.http.post<any>(this.baseUrl + 'PriceQuotations', offer);
  }
  updateOffer(id:number,offer:any){
    return this.http.put<any>(this.baseUrl + 'PriceQuotations/' + id, offer);
  }
  deleteOffer(id:number){
    return this.http.delete<any>(this.baseUrl + 'PriceQuotations/' + id);
  }
  
  // 
  getAdditionNotifications(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'AdditionNotifications?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getAdditionNotificationsById(id:number){
    return this.http.get<any>(this.baseUrl + 'AdditionNotifications/' + id);
  }
  addAdditionNotification(additionNotification:any){
    return this.http.post<any>(this.baseUrl + 'AdditionNotifications', additionNotification);
  }
  updateAdditionNotification(id:number,additionNotification:any){
    return this.http.put<any>(this.baseUrl + 'AdditionNotifications/' + id, additionNotification);
  }
  deleteAdditionNotification(id:number){
    return this.http.delete<any>(this.baseUrl + 'AdditionNotifications/' + id);
  }
  // 
  getDebitNotification(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'DebitNotification?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getDebitNotificationById(id:number){
    return this.http.get<any>(this.baseUrl + 'DebitNotification/' + id);
  }
  addDebitNotification(debitNotification:any){
    return this.http.post<any>(this.baseUrl + 'DebitNotification', debitNotification);
  }
  updateDebitNotification(id:number,debitNotification:any){
    return this.http.put<any>(this.baseUrl + 'DebitNotification/' + id, debitNotification);
  }
  deleteDebitNotification(id:number){
    return this.http.delete<any>(this.baseUrl + 'DebitNotification/' + id);
  }
  // 
  getReceiptPermissions(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'ReceiptPermissions?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getReceiptPermissionsById(id:number){
    return this.http.get<any>(this.baseUrl + 'ReceiptPermissions/' + id);
  }
  addReceiptPermission(receiptPermission:any){
    return this.http.post<any>(this.baseUrl + 'ReceiptPermissions', receiptPermission);
  }
  updateReceiptPermission(id:number,receiptPermission:any){
    return this.http.put<any>(this.baseUrl + 'ReceiptPermissions/' + id, receiptPermission);
  }
  deleteReceiptPermission(id:number){
    return this.http.delete<any>(this.baseUrl + 'ReceiptPermissions/' + id);
  }
  // 
  getMaterialIssuePermissions(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'MaterialIssuePermissions?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getMaterialIssuePermissionsById(id:number){
    return this.http.get<any>(this.baseUrl + 'MaterialIssuePermissions/' + id);
  }
  addMaterialIssuePermission(materialIssuePermission:any){
    return this.http.post<any>(this.baseUrl + 'MaterialIssuePermissions', materialIssuePermission);
  }
  updateMaterialIssuePermission(id:number,materialIssuePermission:any){
    return this.http.put<any>(this.baseUrl + 'MaterialIssuePermissions/' + id, materialIssuePermission);
  }
  deleteMaterialIssuePermission(id:number){
    return this.http.delete<any>(this.baseUrl + 'MaterialIssuePermissions/' + id);
  }
  // 
  getDispensePermissions(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'DispensePermissions?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getDispensePermissionsById(id:number){
    return this.http.get<any>(this.baseUrl + 'DispensePermissions/' + id);
  }
  addDispensePermission(dispensePermission:any){
    return this.http.post<any>(this.baseUrl + 'DispensePermissions', dispensePermission);
  }
  updateDispensePermission(id:number,dispensePermission:any){
    return this.http.put<any>(this.baseUrl + 'DispensePermissions/' + id, dispensePermission);
  }
  deleteDispensePermission(id:number){
    return this.http.delete<any>(this.baseUrl + 'DispensePermissions/' + id);
  }
  // 
  getTreasuries(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'Treasuries?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getEnabledTreasuries(){
    return this.http.get<any>(this.baseUrl + 'Treasuries/Enabled');
  }
  enableTreasury(id:number){
    return this.http.put<any>(this.baseUrl + 'Treasuries/Enable?id=' + id, {});
  }
  getDisabledTreasuries(){
    return this.http.get<any>(this.baseUrl + 'Treasuries/Disabled');
  }
  disableTreasury(id:number){
    return this.http.put<any>(this.baseUrl + 'Treasuries/Disabled?id=' + id, {});
  }
  getTreasuriesById(id:number){
    return this.http.get<any>(this.baseUrl + 'Treasuries/' + id);
  }
  getTransactionsForTreasury(id:any){
    return this.http.get<any>(this.baseUrl + 'Treasuries/transactions/' + id);
  }
  addTreasury(treasury:any){
    return this.http.post<any>(this.baseUrl + 'Treasuries', treasury);
  }
  updateTreasury(id:number,treasury:any){
    return this.http.put<any>(this.baseUrl + 'Treasuries/' + id, treasury);
  }
  deleteTreasury(id:number){
    return this.http.delete<any>(this.baseUrl + 'Treasuries/' + id);
  }
  // 
  getDailyRestrictions(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'DailyRestrictions?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getDailyRestrictionsById(id:number){
    return this.http.get<any>(this.baseUrl + 'DailyRestrictions/' + id);
  }
  addDailyRestriction(dailyRestriction:any){
    return this.http.post<any>(this.baseUrl + 'DailyRestrictions', dailyRestriction);
  }
  updateDailyRestriction(id:number,dailyRestriction:any){
    return this.http.put<any>(this.baseUrl + 'DailyRestrictions/' + id, dailyRestriction);
  }
  deleteDailyRestriction(id:number){
    return this.http.delete<any>(this.baseUrl + 'DailyRestrictions/' + id);
  }
  // 
  getDailyRestrictionsTypes(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'RestrictionTypes?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getDailyRestrictionsTypesById(id:number){
    return this.http.get<any>(this.baseUrl + 'RestrictionTypes/' + id);
  }
  addDailyRestrictionTypes(dailyRestriction:any){
    return this.http.post<any>(this.baseUrl + 'RestrictionTypes', dailyRestriction);
  }
  updateDailyRestrictionTypes(id:number,dailyRestriction:any){
    return this.http.put<any>(this.baseUrl + 'RestrictionTypes/' + id, dailyRestriction);
  }
  deleteDailyRestrictionTypes(id:number){
    return this.http.delete<any>(this.baseUrl + 'RestrictionTypes/' + id);
  }
  // 
  getAccountingGuidance(pagingFilter:PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'AccountingGuidance?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getAccountingGuidanceById(id:number){
    return this.http.get<any>(this.baseUrl + 'AccountingGuidance/' + id);
  }
  addAccountingGuidance(accountingGuidance:any){
    return this.http.post<any>(this.baseUrl + 'AccountingGuidance', accountingGuidance);
  }
  updateAccountingGuidance(id:number,accountingGuidance:any){
    return this.http.put<any>(this.baseUrl + 'AccountingGuidance/' + id, accountingGuidance);
  }
  deleteAccountingGuidance(id:number){
    return this.http.delete<any>(this.baseUrl + 'AccountingGuidance/' + id);
  }
  // 
  getAccounts(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'AccountTree/GetAccountTreeData?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  // 
  getBranches(pagingFilter : PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'Branches?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getBranchesById(id:number){
    return this.http.get<any>(this.baseUrl + 'Branches/' + id);
  }
  addBranch(branch:any){
    return this.http.post<any>(this.baseUrl + 'Branches', branch);
  }
  updateBranch(id:number,branch:any){
    return this.http.put<any>(this.baseUrl + 'Branches/' + id, branch);
  }
  deleteBranch(id:number){
    return this.http.delete<any>(this.baseUrl + 'Branches/' + id);
  }
  // 
  getSupplyReceipts(pagingFilter: PagingFilterModel){
    return this.http.get<PagedResponseModel<any>>(this.baseUrl + 'SupplyReceipts?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getSupplyReceiptsById(id:number){
    return this.http.get<any>(this.baseUrl + 'SupplyReceipts/' + id);
  }
  addSupplyReceipt(supplyReceipt:any){
    return this.http.post<any>(this.baseUrl + 'SupplyReceipts', supplyReceipt);
  }
  updateSupplyReceipt(id:number,supplyReceipt:any){
    return this.http.put<any>(this.baseUrl + 'SupplyReceipts/' + id, supplyReceipt);
  }
  deleteSupplyReceipt(id:number){
    return this.http.delete<any>(this.baseUrl + 'SupplyReceipts/' + id);
  }
  // 
  getFiscalYears(){
    return this.http.get<any>(this.baseUrl + 'FiscalYears');
  }
  postFiscalYear(fiscalYear:any){
    return this.http.post<any>(this.baseUrl + 'FiscalYears', fiscalYear);
  }
}
