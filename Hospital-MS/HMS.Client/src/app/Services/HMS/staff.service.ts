import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PagingFilterModel } from '../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../Models/Generics/PagedResponseModel';
import { EmployeePenaltyModel } from '../../Models/HMS/Staff/EmployeePenaltyModel';
import { EmployeeContractModel } from '../../Models/HMS/Staff/EmployeeContractModel';
import { FormDropdownModel } from '../../Models/Generics/FormDropdownModel';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  baseUrl = environment.baseUrl
  constructor(private http: HttpClient) { }
  getStaff() {
    return this.http.get<any>(`${this.baseUrl}Staff`);
  }
  getAllStaff(
    page: number,
    size: number,
    search?: string,
    status?: string,
    fromDate?: string,
    toDate?: string,
    filterList: any[] = []
  ) {
    let params = new HttpParams()
      .set('CurrentPage', page.toString())
      .set('PageSize', size.toString());

    if (search) params = params.set('SearchText', search);
    if (status) params = params.set('Status', status);
    if (fromDate) params = params.set('FromDate', fromDate);
    if (toDate) params = params.set('ToDate', toDate);
    params = params.set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}Staff`, { params });
  }
  getCounts() {
    let params = new HttpParams()
      .set('CurrentPage', '1')
      .set('PageSize', '16')
      .set('FilterList', JSON.stringify([{
        categoryName: 'string',
        itemId: 'string',
        itemKey: 'string',
        itemValue: 'string',
        isChecked: true,
        fromDate: '2025-05-01T21:50:30.220Z',
        toDate: '2025-05-01T21:50:30.220Z',
        filterType: 'string',
        isVisible: true,
        filterItems: ['string'],
        displayOrder: 0
      }]));

    return this.http.get<any>(`${this.baseUrl}Staff/counts`, { params });
  }
  getStaffById(id: number) {
    return this.http.get<any>(`${this.baseUrl}Staff/${id}`);
  }
  addStaff(staff: any) {
    return this.http.post<any>(`${this.baseUrl}Staff`, staff);
  }
  updateStaff(staff: any) {
    return this.http.put<any>(`${this.baseUrl}Staff`, staff);
  }
  deleteStaff(id: number) {
    return this.http.delete<any>(`${this.baseUrl}Staff/${id}`);
  }
  // ===========================================================================
  getDoctors(pagingFilter: PagingFilterModel) {
    return this.http.post<PagedResponseModel<any>>(`${this.baseUrl}Doctors/all`, pagingFilter);
  }
  getDoctorById(id: any) {
    return this.http.get<any>(`${this.baseUrl}Doctors/${id}`);
  }
  postDoctor(doctor: any) {
    return this.http.post<any>(`${this.baseUrl}Doctors`, doctor);
  }
  putDoctor(doctor: any, id: number) {
    return this.http.put<any>(`${this.baseUrl}Doctors/${id}`, doctor);
  }
  getDoctorsCount() {
    return this.http.get<PagedResponseModel<any>>(`${this.baseUrl}Doctors/counts`);
  }
  // ===========================================================================
  getJobTypes(searchText: string, currentPage: number, pageSize: number, filterList: any[] = []) {
    let params = new HttpParams()
      .set('SearchText', searchText.toString())
      .set('CurrentPage', currentPage.toString())
      .set('PageSize', pageSize.toString())
      .set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}JobTypes`, { params });
  }
  getJobTypeById(id: number) {
    return this.http.get<any>(`${this.baseUrl}JobTypes/${id}`);
  }
  addJobType(jobType: any) {
    return this.http.post<any>(`${this.baseUrl}JobTypes`, jobType);
  }
  updateJobType(id: number, jobType: any) {
    return this.http.put<any>(`${this.baseUrl}JobTypes/${id}`, jobType);
  }
  // 
  getJobTitles(searchText: string, currentPage: number, pageSize: number, filterList: any[] = []) {
    let params = new HttpParams()
      .set('SearchText', searchText.toString())
      .set('CurrentPage', currentPage.toString())
      .set('PageSize', pageSize.toString())
      .set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}JobTitles`, { params });
  }
  getJobTitleById(id: number) {
    return this.http.get<any>(`${this.baseUrl}JobTitles/${id}`);
  }
  addJobTitle(jobTitle: any) {
    return this.http.post<any>(`${this.baseUrl}JobTitles`, jobTitle);
  }
  updateJobTitle(id: number, jobTitle: any) {
    return this.http.put<any>(`${this.baseUrl}JobTitles/${id}`, jobTitle);
  }
  // 
  getJobLevels(searchText: string, currentPage: number, pageSize: number, filterList: any[] = []) {
    let params = new HttpParams()
      .set('SearchText', searchText.toString())
      .set('CurrentPage', currentPage.toString())
      .set('PageSize', pageSize.toString())
      .set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}JobLevels`, { params });
  }
  getJobLevelById(id: number) {
    return this.http.get<any>(`${this.baseUrl}JobLevels/${id}`);
  }
  addJobLevel(jobLevel: any) {
    return this.http.post<any>(`${this.baseUrl}JobLevels`, jobLevel);
  }
  updateJobLevel(id: number, jobLevel: any) {
    return this.http.put<any>(`${this.baseUrl}JobLevels/${id}`, jobLevel);
  }
  // 
  getJobDepartment(searchText: string, currentPage: number, pageSize: number, filterList: any[] = []) {
    let params = new HttpParams()
      .set('SearchText', searchText.toString())
      .set('CurrentPage', currentPage.toString())
      .set('PageSize', pageSize.toString())
      .set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}JobDepartment`, { params });
  }
  getJobDepartmentById(id: number) {
    return this.http.get<any>(`${this.baseUrl}JobDepartment/${id}`);
  }
  addJobDepartment(jobDepartment: any) {
    return this.http.post<any>(`${this.baseUrl}JobDepartment`, jobDepartment)
  }
  updateJobDeprtment(id: number, jobDepartment: any) {
    return this.http.put<any>(`${this.baseUrl}JobDepartment/${id}`, jobDepartment)
  }

  // ================================= Penalty ==========================================

  GetPenaltiesByEmployeeId(employeeId, model: PagingFilterModel) {
    return this.http.post<PagedResponseModel<EmployeePenaltyModel[]>>(this.baseUrl + 'Penalty/GetPenaltiesByEmployeeId?EmployeeId=' + employeeId, model);
  }

   GetEmployeeContractDetails(EmployeeId: number) {
    return this.http.get<EmployeeContractModel>(this.baseUrl + 'Penalty/GetEmployeeContractDetails?EmployeeId=' + EmployeeId);
  }

   AddNewEmployeePenalty(employeeId: number, model: EmployeePenaltyModel) {
    return this.http.post<any>(this.baseUrl + 'Penalty/AddNewEmployeePenalty?EmployeeId=' + employeeId, model);
  }

  EditEmployeePenalty(employeeId: number, model: EmployeePenaltyModel) {
    return this.http.post<any>(this.baseUrl + 'Penalty/EditEmployeePenalty?EmployeeId=' + employeeId, model);
  }

  GetPenaltyTypesSelector() {
    return this.http.get<FormDropdownModel[]>(this.baseUrl + 'Penalty/GetPenaltyTypesSelector');
  }

  GetActiveEmployeesSelector() {
    return this.http.get<FormDropdownModel[]>(this.baseUrl + 'Penalty/GetActiveEmployeesSelector');
  }

  DeleteEmployeePenalty(PenaltyId: number) {
    return this.http.get<any>(this.baseUrl + 'Penalty/DeleteEmployeePenalty?PenaltyId=' + PenaltyId);
  }
}
