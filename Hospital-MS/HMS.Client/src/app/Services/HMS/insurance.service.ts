import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { InsuranceCompany } from '../../Models/HMS/insurance';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InsuranceService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) { }
  getAllInsurances():Observable<InsuranceCompany[]> {
    return this.http.get<InsuranceCompany[]>(`${this.baseUrl}/api/Insurances/company`);
  }
  getInsuranceById(id:number):Observable<InsuranceCompany> {
    return this.http.get<InsuranceCompany>(`${this.baseUrl}/api/Insurances/company/${id}`);
  }
  addInsurance(insurance: InsuranceCompany):Observable<InsuranceCompany> {
    return this.http.post<InsuranceCompany>(`${this.baseUrl}/api/Insurances/company`, insurance);
  }
  updateInsurance( insuranceId , insurance: InsuranceCompany):Observable<InsuranceCompany> {
    return this.http.put<InsuranceCompany>(`${this.baseUrl}/api/Insurances/company/${insuranceId}`, insurance);
  }
  deleteInsurance(id: number):Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/Insurances/company/${id}`);
  }
}