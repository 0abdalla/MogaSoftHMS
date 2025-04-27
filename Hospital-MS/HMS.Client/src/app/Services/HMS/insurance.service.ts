import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InsuranceService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) { }
  getAllInsurances() {
    return this.http.get(`${this.baseUrl}/api/Insurance/companies`);
  }
}