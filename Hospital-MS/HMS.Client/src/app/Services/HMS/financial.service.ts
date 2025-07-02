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
  addTreasury(treasury:any){
    return this.http.post<any>(this.baseUrl + 'Treasuries', treasury);
  }
  updateTreasury(id:number,treasury:any){
    return this.http.put<any>(this.baseUrl + 'Treasuries/' + id, treasury);
  }
  deleteTreasury(id:number){
    return this.http.delete<any>(this.baseUrl + 'Treasuries/' + id);
  }
}
