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
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }


  getAdmissions(pagingFilter: PagingFilterModel): Observable<PagedResponseModel<any>> {
    const params = new HttpParams({ fromObject: pagingFilter as any });
    return this.http.get<PagedResponseModel<any>>(`${this.baseUrl}Admissions`, { params });
  }


  getAdmissionById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}Admissions/${id}`);
  }


  addAdmission(request: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}Admissions`, request);
  }


  updateAdmission(id: number, request: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}Admissions/${id}`, request);
  }

  deleteAdmission(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}Admissions/${id}`);
  }

  getPatientAdmissions(patientId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}Admissions/patient/${patientId}`);
  }

  getPatientById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}Patients/${id}`);
  }



  getRooms(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}Rooms`);
  }

  getBeds(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}Beds`);
  }

  getMedicalHistory(id: number) {
    return this.http.get(`${this.baseUrl}Patients/medical-history/${id}`)
  }

  getCounts(filterList: any[] = []): Observable<any> {
    let params = new HttpParams().set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}patients/counts`, { params });
  }

  addDepartment(department: any) {
    return this.http.post(this.baseUrl + 'Departments', department)
  }
  getDepartments() {
    return this.http.get<any>(`${this.baseUrl}Departments`);
  }
  getDepartmentsById(id: number) {
    return this.http.get<any>(`${this.baseUrl}Departments/${id}`);
  }
  updateDepartment(id: number, department: any) {
    return this.http.put(this.baseUrl + 'Departments/' + id, department)
  }
  deleteDepartment(id: number) {
    return this.http.delete<any>(this.baseUrl + 'Departments/' + id);
  }
  getWards() {
    return this.http.get<any>(`${this.baseUrl}Wards`);
  }
}
