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
}
