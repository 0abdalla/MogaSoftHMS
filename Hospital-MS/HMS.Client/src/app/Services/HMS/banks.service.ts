import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PagingFilterModel } from '../../Models/Generics/PagingFilterModel';
@Injectable({
  providedIn: 'root'
})
export class BanksService {
  baseUrl:string=environment.baseUrl;
  constructor(private http:HttpClient) { }

  getBanks(pagingFilter: PagingFilterModel){
    return this.http.get<any>(this.baseUrl + 'Banks?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
  getBankById(id:number){
    return this.http.get<any>(this.baseUrl + 'Banks/' + id);
  }
  addBank(Bank:any){
    return this.http.post<any>(this.baseUrl + 'Banks', Bank);
  }
  updateBank(id:number,Bank:any){
    return this.http.put<any>(this.baseUrl + 'Banks/' + id, Bank);
  }
  deleteBank(id:number){
    return this.http.delete<any>(this.baseUrl + 'Banks/' + id);
  }
  getAccounts(pagingFilter: PagingFilterModel){
    return this.http.get<any>(this.baseUrl + 'Accounts?currentPage=' + pagingFilter.currentPage + '&pageSize=' + pagingFilter.pageSize + '&filterList=' + pagingFilter.filterList);
  }
}
