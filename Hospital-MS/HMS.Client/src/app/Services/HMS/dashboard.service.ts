import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Home } from '../../Models/HMS/home';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  baseUrl = environment.baseUrl;
  constructor(private http : HttpClient) { }
  getHome(month:string):Observable<Home>{
    return this.http.get<Home>(`${this.baseUrl}Dashboard/metrics?month=${month}`);
  }
  getTopDoctors(month:string):Observable<any>{
    return this.http.get<any>(`${this.baseUrl}Dashboard/topDoctors?month=${month}`);
  }
  getMedicalServices(){
    return this.http.get<any>(`${this.baseUrl}Dashboard/medicalServices`);
  }
}
