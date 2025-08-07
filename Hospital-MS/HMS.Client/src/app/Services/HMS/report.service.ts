import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  baseUrl = environment.baseUrl;
  constructor(private http : HttpClient) { }
  getLedgerReport(id : number , from : any , to : any){
    return this.http.get<any>(this.baseUrl + `DailyRestrictions/report/${id}?fromDate=${from}&toDate=${to}`);
  }
  getStoreMovement(storeId:number , from : any , to : any , MainGroupId : number , ItemGroupId : number){
    return this.http.get<any>(this.baseUrl + `Stores/movements/${storeId}?fromDate=${from}&toDate=${to}&MainGroupId=${MainGroupId}&ItemGroupId=${ItemGroupId}`);
  }
  getItemMovement(itemId:number , from : any , to : any , storeId : number){
    return this.http.get<any>(this.baseUrl + `Items/movement/${itemId}?FromDate=${from}&ToDate=${to}&StoreId=${storeId}`);
  }
}
