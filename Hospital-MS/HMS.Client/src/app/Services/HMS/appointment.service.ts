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

  createAppointment(appointment: any): Observable<ErrorResponseModel<string>> {
    return this.http.post<ErrorResponseModel<string>>(`${this.baseUrl}Appointments`, appointment);
  }
  editAppointment(id: number, appointment: any): Observable<ErrorResponseModel<string>> {
    return this.http.put<ErrorResponseModel<string>>(`${this.baseUrl}Appointments/${id}`, appointment);
  }
  deleteAppointment(id: number): Observable<ErrorResponseModel<string>> {
    return this.http.delete<ErrorResponseModel<string>>(`${this.baseUrl}Appointments?id=${id}`);
  }
  getAllShifts(){
    return this.http.get(`${this.baseUrl}Appointments/all-shifts`);
  }
  getShiftById(id:number){
    return this.http.get(`${this.baseUrl}Appointments/shift/${id}`);
  }
  closeShift(){
    return this.http.post<any>(`${this.baseUrl}Appointments/close-shift`, {});
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

  GetMedicalService(pagingFilter: PagingFilterModel) {
    return this.http.post<any>(`${this.baseUrl}MedicalService/GetMedicalService`, pagingFilter);
  }

  GetRadiologyBodyTypes() {
    return this.http.get<any>(`${this.baseUrl}MedicalService/GetRadiologyBodyTypes`);
  }

  addService(service: any) {
    return this.http.post<any>(`${this.baseUrl}MedicalService`, service);
  }

  CreateRadiologyBodyType(service: any) {
    return this.http.post<any>(`${this.baseUrl}MedicalService/CreateRadiologyBodyType`, service);
  }

  editService(id: number, service: any) {
    return this.http.put<any>(`${this.baseUrl}MedicalService/${id}`, service);
  }
  // 
  getWards(){
    return this.http.get<any>(`${this.baseUrl}Wards`);
  }
  getWardsById(id: number){
    return this.http.get<any>(`${this.baseUrl}Wards/${id}`);
  }
  addWard(ward: any){
    return this.http.post<any>(`${this.baseUrl}Wards`, ward);
  }
  editWard(id: number, ward: any){
    return this.http.put<any>(`${this.baseUrl}Wards/${id}`, ward);
  }
  deleteWard(id: number){
    return this.http.delete<any>(`${this.baseUrl}Wards/${id}`);
  }
  // 
  getRooms(){
    return this.http.get<any>(`${this.baseUrl}Rooms`);
  }
  getRoomsById(id: number){
    return this.http.get<any>(`${this.baseUrl}Rooms/${id}`);
  }
  addRoom(room: any){
    return this.http.post<any>(`${this.baseUrl}Rooms`, room);
  }
  editRoom(id: number, room: any){
    return this.http.put<any>(`${this.baseUrl}Rooms/${id}`, room);
  }
  deleteRoom(id: number){
    return this.http.delete<any>(`${this.baseUrl}Rooms/${id}`);
  }
  // 
  getBeds(){
    return this.http.get<any>(`${this.baseUrl}Beds`);
  }
  getBedsById(id: number){
    return this.http.get<any>(`${this.baseUrl}Beds/${id}`);
  }
  addBed(bed: any){
    return this.http.post<any>(`${this.baseUrl}Beds`, bed);
  }
  editBed(id: number, bed: any){
    return this.http.put<any>(`${this.baseUrl}Beds/${id}`, bed);
  }
  deleteBed(id: number){
    return this.http.delete<any>(`${this.baseUrl}Beds/${id}`);
  }
}
