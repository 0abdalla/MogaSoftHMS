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
  getAddmision(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = []
  ): Observable<any> {
    let params = new HttpParams()
      .set('PageSize', size.toString())
      .set('CurrentPage', page.toString());
    if (search) params = params.set('SearchText', search); 
    params = params.set('FilterList', JSON.stringify(filterList));
  
    return this.http.get<any>(`${this.baseUrl}/api/patients`, { params });
  }
  getCounts(filterList: any[] = []): Observable<any> {
    let params = new HttpParams().set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}/api/patients/counts`, { params });
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
    return this.http.get<any>(`${this.baseUrl}/api/Departments`);
  }
  getWards(){
    return this.http.get<any>(`${this.baseUrl}/api/Wards`);
  }
  getRooms(){
    return this.http.get<any>(`${this.baseUrl}/api/Rooms`);
  }
  getBeds(){
    return this.http.get<any>(`${this.baseUrl}/api/Beds`);
  }
  deleteAdmision(id : number){
    return this.http.delete<any>(this.baseUrl + '/Admissions/' + id);
  }
}
