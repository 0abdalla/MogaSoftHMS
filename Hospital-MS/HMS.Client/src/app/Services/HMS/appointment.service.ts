import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { Patients } from '../../Models/HMS/patient';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) { }

  getAllAppointments(page: number, size: number, type?: string, search?: string): Observable<any> {
    let params = new HttpParams()
      .set('PageSize', size)
      .set('PageIndex', page);
  
    if (type) params = params.set('Type', type);
    if (search) params = params.set('Search', search);

    return this.http.get<any>(this.baseUrl + '/api/Appointments', { params });
  }

  getAppointmentById(id: number): Observable<Patients> {
    return this.http.get<Patients>(`${this.baseUrl}/api/Appointments/${id}`);
  }

  createAppointment(appointment: FormData): Observable<Patients> {
    return this.http.post<Patients>(`${this.baseUrl}/api/Appointments`, appointment);
  }
  updateEmergency(id: number, updateEmergencyForm: FormGroup): Observable<Patients> {
    return this.http.put<Patients>(`${this.baseUrl}/api/Appointments/emergency/${id}`, updateEmergencyForm.value);
  }

  getCounts(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/api/Appointments/counts`);
  }
  // 
  getClinics() {
    return this.http.get<any>(`${this.baseUrl}/api/Clinics`);
  }
}
