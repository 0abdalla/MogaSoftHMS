import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { Patients } from '../../Models/HMS/patient';
import { PagingFilterModel } from '../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../Models/Generics/PagedResponseModel';
import { ErrorResponseModel } from '../../Models/Generics/ErrorResponseModel';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) { }

  getAllAppointments(pagingFilter: PagingFilterModel): Observable<any> {
    return this.http.post<PagedResponseModel<any>>(this.baseUrl + 'Appointments/GetAppointments', pagingFilter);
  }

  getAppointmentById(id: number): Observable<ErrorResponseModel<Patients>> {
    return this.http.get<ErrorResponseModel<Patients>>(`${this.baseUrl}Appointments/${id}`);
  }

  createAppointment(appointment: FormData): Observable<ErrorResponseModel<string>> {
    return this.http.post<ErrorResponseModel<string>>(`${this.baseUrl}Appointments`, appointment);
  }
  updateEmergency(id: number, updateEmergencyForm: FormGroup): Observable<ErrorResponseModel<string>> {
    return this.http.put<ErrorResponseModel<string>>(`${this.baseUrl}Appointments/emergency/${id}`, updateEmergencyForm.value);
  }

  getCounts(pagingFilter: PagingFilterModel): Observable<any> {
    return this.http.post<PagedResponseModel<any>>(`${this.baseUrl}Appointments/GetAppointmentsCounts`, pagingFilter);
  }
  // 
  getClinics() {
    return this.http.get<any>(`${this.baseUrl}Clinics`);
  }
  // 
  getServices(
    currentPage: number = 1,
    pageSize: number = 16,
    searchText: string = '',
    filterList: any = {
      
    }
  ) {
    const params = new HttpParams()
      .set('CurrentPage', currentPage.toString())
      .set('PageSize', pageSize.toString())
      .set('SearchText', searchText)
      .set('FilterList', JSON.stringify(filterList));
  
    return this.http.get<any>(`${this.baseUrl}MedicalService`, { params });
  }
}
