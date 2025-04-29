import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  baseUrl = environment.baseUrl
  constructor(private http: HttpClient) { }
  getStaff(){
    return this.http.get<any>(`${this.baseUrl}Staff`);
  }
  getAllStaff(currentPage: number, pageSize: number, type: string, search: string){
    let params = new HttpParams()
          .set('PageSize', pageSize)
          .set('PageIndex', currentPage);
      
        if (type) params = params.set('Type', type);
        if (search) params = params.set('Search', search);

    return this.http.get<any>(`${this.baseUrl}Staff/all`, { params });
  }
  getCounts(){
    return this.http.get<any>(`${this.baseUrl}Staff/counts`);
  }
  getStaffById(id: number){
    return this.http.get<any>(`${this.baseUrl}Staff/${id}`);
  }
  addStaff(staff: any){
    return this.http.post<any>(`${this.baseUrl}Staff`, staff);
  }
  updateStaff(staff: any){
    return this.http.put<any>(`${this.baseUrl}Staff`, staff);
  }
  deleteStaff(id: number){
    return this.http.delete<any>(`${this.baseUrl}Staff/${id}`);
  }
  // ===========================================================================
  getDoctors(currentPage: number, pageSize: number, type: string, search: string){
    let params = new HttpParams()
          .set('PageSize', pageSize)
          .set('PageIndex', currentPage);
      
        if (type) params = params.set('Type', type);
        if (search) params = params.set('Search', search);

    return this.http.get<any>(`${this.baseUrl}Doctors/all`, { params });
  }
  getDoctorById(id:any){
    return this.http.get<any>(`${this.baseUrl}Doctors/${id}`);
  }
  postDoctor(doctor: any){
    return this.http.post<any>(`${this.baseUrl}Doctors`, doctor);
  }
  putDoctor(doctor: any, id: number){
    return this.http.put<any>(`${this.baseUrl}Doctors/${id}`, doctor);
  }
  getDoctorsCount(){
    return this.http.get<any>(`${this.baseUrl}Doctors/counts`);
  }
}
