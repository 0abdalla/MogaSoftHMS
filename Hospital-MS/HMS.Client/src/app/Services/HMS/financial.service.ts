import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
@Injectable({
  providedIn: 'root'
})
export class FinancialService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) { }
  // الخزائن
  getAllTreasuries(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = [
      
    ]
  ) {
    let params = new HttpParams()
      .set('CurrentPage', page.toString())
      .set('PageSize', size.toString());

    if (search) params = params.set('SearchText', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    params = params.set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}Treasuries`, { params });
  }
  getTreauryById(id:number){
    return this.http.get<any>(`${this.baseUrl}Treasuries/${id}`);
  }
  addTreaury(accountForm:any){
    return this.http.post<any>(`${this.baseUrl}Treasuries`,accountForm.value);
  }
  updateTreaury(id:number , accountForm:any){
    return this.http.put<any>(`${this.baseUrl}Treasuries/${id}`,accountForm.value);
  }
  deleteTreaury(id:number){
    return this.http.delete<any>(`${this.baseUrl}Treasuries/${id}`);
  }
  // البنوك
  getAllBanks(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = [
      
    ]
  ) {
    let params = new HttpParams()
      .set('CurrentPage', page.toString())
      .set('PageSize', size.toString());

    if (search) params = params.set('SearchText', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    params = params.set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}Banks`, { params });
  }
  getBankById(id:number){
    return this.http.get<any>(`${this.baseUrl}Banks/${id}`);
  }
  addBank(accountForm:any){
    return this.http.post<any>(`${this.baseUrl}Banks`,accountForm.value);
  }
  updateBank(id:number , accountForm:any){
    return this.http.put<any>(`${this.baseUrl}Banks/${id}`,accountForm.value);
  }
  deleteBank(id:number){
    return this.http.delete<any>(`${this.baseUrl}Banks/${id}`);
  }
  // أذون الإضافة
  getAllAdditionNotifications(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = [
      
    ]
  ) {
    let params = new HttpParams()
      .set('CurrentPage', page.toString())
      .set('PageSize', size.toString());

    if (search) params = params.set('SearchText', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    params = params.set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}AdditionNotifications`, { params });
  }
  getAdditionNotificationById(id:number){
    return this.http.get<any>(`${this.baseUrl}AdditionNotifications/${id}`);
  }
  addAdditionNotification(additionNotificationForm:any){
    return this.http.post<any>(`${this.baseUrl}AdditionNotifications`,additionNotificationForm);
  }
  updateAdditionNotification(id:number , additionNotificationForm:any){
    return this.http.put<any>(`${this.baseUrl}AdditionNotifications/${id}`,additionNotificationForm);
  }
  deleteAdditionNotification(id:number){
    return this.http.delete<any>(`${this.baseUrl}AdditionNotifications/${id}`);
  }
  // أذون الخصم
  getAllDiscountNotifications(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = [
      
    ]
  ) {
    let params = new HttpParams()
      .set('CurrentPage', page.toString())
      .set('PageSize', size.toString());

    if (search) params = params.set('SearchText', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    params = params.set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}DebitNotices`, { params });
  }
  getDiscountNotificationById(id:number){
    return this.http.get<any>(`${this.baseUrl}DebitNotices/${id}`);
  }
  addDiscountNotification(discountNotificationForm:any){
    return this.http.post<any>(`${this.baseUrl}DebitNotices`,discountNotificationForm);
  }
  updateDiscountNotification(id:number , discountNotificationForm:any){
    return this.http.put<any>(`${this.baseUrl}DebitNotices/${id}`,discountNotificationForm);
  }
  deleteDiscountNotification(id:number){
    return this.http.delete<any>(`${this.baseUrl}DebitNotices/${id}`);
  }
  // إيصال التوريد
  getSupplyReceipts(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = [
      
    ]
  ) {
    let params = new HttpParams()
      .set('CurrentPage', page.toString())
      .set('PageSize', size.toString());

    if (search) params = params.set('SearchText', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    params = params.set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}SupplyReceipts`, { params });
  }
  getSupplyReceiptById(id:number){
    return this.http.get<any>(`${this.baseUrl}SupplyReceipts/${id}`);
  }
  addSupplyReceipt(supplyReceiptForm:any){
    return this.http.post<any>(`${this.baseUrl}SupplyReceipts`,supplyReceiptForm);
  }
  updateSupplyReceipt(id:number , supplyReceiptForm:any){
    return this.http.put<any>(`${this.baseUrl}SupplyReceipts/${id}`,supplyReceiptForm);
  }
  deleteSupplyReceipt(id:number){
    return this.http.delete<any>(`${this.baseUrl}SupplyReceipts/${id}`);
  }
  // إذن صرف خزينة
  getDispensePermission(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = [
      
    ]
  ) {
    let params = new HttpParams()
      .set('CurrentPage', page.toString())
      .set('PageSize', size.toString());

    if (search) params = params.set('SearchText', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    params = params.set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}DispensePermission`, { params });
  }
  getDispensePermissionById(id:number){
    return this.http.get<any>(`${this.baseUrl}DispensePermission/${id}`);
  }
  addDispensePermission(addPermissionForm:any){
    return this.http.post<any>(`${this.baseUrl}DispensePermission`,addPermissionForm);
  }
  updateDispensePermission(id:number , addPermissionForm:any){
    return this.http.put<any>(`${this.baseUrl}DispensePermission/${id}`,addPermissionForm);
  }
  deleteDispensePermission(id:number){
    return this.http.delete<any>(`${this.baseUrl}DispensePermission/${id}`);
  }
}
