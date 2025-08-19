import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { PagingFilterModel } from '../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../Models/Generics/PagedResponseModel';

@Injectable({
  providedIn: 'root'
})
export class AdmissionService {
  baseUrl = environment.baseUrl
  constructor(private http : HttpClient) { }
  getAddmision(pagingFilter: PagingFilterModel): Observable<any> {
    return this.http.post<PagedResponseModel<any>>(this.baseUrl + 'patients', pagingFilter);
  }
  getCounts(filterList: any[] = []): Observable<any> {
    let params = new HttpParams().set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}patients/counts`, { params });
  }
  getAddmisionById(id : number): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'Admissions/' + id)
  }
  getPatientById(id : number): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'Patients/' + id)
  }
  addAdmision(addAdmision : FormData){
    return this.http.post(this.baseUrl + 'Admissions', addAdmision)
  }
  updateAdmision(id : number, patient : FormData){
    return this.http.put(this.baseUrl + 'Patients/status/' + id, patient)
  }
  // 
  addDepartment(department : any){
    return this.http.post(this.baseUrl + 'Departments', department)
  }
  getDepartments(){
    return this.http.get<any>(`${this.baseUrl}Departments`);
  }
  getDepartmentsById(id : number){
    return this.http.get<any>(`${this.baseUrl}Departments/${id}`);
  }
  updateDepartment(id : number, department : any){
    return this.http.put(this.baseUrl + 'Departments/' + id, department)
  }
  deleteDepartment(id : number){
    return this.http.delete<any>(this.baseUrl + 'Departments/' + id);
  }
  getWards(){
    return this.http.get<any>(`${this.baseUrl}Wards`);
  }
  getRooms(){
    return this.http.get<any>(`${this.baseUrl}Rooms`);
  }
  getBeds(){
    return this.http.get<any>(`${this.baseUrl}Beds`);
  }
  deleteAdmision(id : number){
    return this.http.delete<any>(this.baseUrl + 'Admissions/' + id);
  }
  // 
  getMedicalHistory(id:number){
    return this.http.get(`${this.baseUrl}Patients/medical-history/${id}`)
  }
}
