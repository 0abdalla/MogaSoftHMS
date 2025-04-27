import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdmissionService {
  baseUrl = environment.baseUrl
  constructor(private http : HttpClient) { }
  getAddmision(page: number, size: number, search?: string, status?: string, fromDate?: string, toDate?: string): Observable<any> {
    let params = new HttpParams()
      .set('PageSize', size)
      .set('PageIndex', page);
  
    if (search) params = params.set('Search', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    return this.http.get<any>(`${this.baseUrl}/api/patients`, { params });
  }  
  getCounts(){
    return this.http.get<any>(`${this.baseUrl}/api/patients/counts`)
  }
  getAddmisionById(id : number): Observable<any> {
    return this.http.get<any>(this.baseUrl + '/api/Admissions/' + id)
  }
  getPatientById(id : number): Observable<any> {
    return this.http.get<any>(this.baseUrl + '/api/Patients/' + id)
  }
  addAdmision(addAdmision : FormData){
    return this.http.post(this.baseUrl + '/api/Admissions', addAdmision)
  }
  updateAdmision(id : number, patient : FormData){
    return this.http.put(this.baseUrl + '/api/Patients/status/' + id, patient)
  }
  // 
  getDepartments(){
    return this.http.get(this.baseUrl + '/api/Departments')
  }
  getWards(){
    return this.http.get(this.baseUrl + '/api/Wards')
  }
  getRooms(){
    return this.http.get(this.baseUrl + '/api/Rooms')
  }
  getBeds(){
    return this.http.get(this.baseUrl + '/api/Beds')
  }
  deleteAdmision(id : number){
    return this.http.delete(this.baseUrl + '/api/Admissions/' + id)
  }
}
