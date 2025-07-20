import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PagingFilterModel } from '../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../Models/Generics/PagedResponseModel';
import { EmployeePenaltyModel } from '../../Models/HMS/Staff/EmployeePenaltyModel';
import { EmployeeContractModel } from '../../Models/HMS/Staff/EmployeeContractModel';
import { FormDropdownModel } from '../../Models/Generics/FormDropdownModel';
import { EmployeeVacationModel } from '../../Models/HMS/Staff/EmployeeVacationModel';
import { GeneralSelectorModel } from '../../Models/Generics/GeneralSelectorModel';
import { EmployeeAdvanceModel } from '../../Models/HMS/Staff/EmployeeAdvanceModel';
import { EmployeeSalarySummaryModel } from '../../Models/HMS/Staff/EmployeeSalarySummaryModel';

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

  // ================================= Vacation ==========================================

  GetVacationsByEmployeeId(employeeId, model: PagingFilterModel) {
    return this.http.post<PagedResponseModel<EmployeeVacationModel[]>>(this.baseUrl + 'Vacation/GetVacationsByEmployeeId?EmployeeId=' + employeeId, model);
  }

  AddNewEmployeeVacation(employeeId: number, model: EmployeeVacationModel) {
    return this.http.post<any>(this.baseUrl + 'Vacation/AddNewEmployeeVacation?EmployeeId=' + employeeId, model);
  }

  EditEmployeeVacation(employeeId: number, model: EmployeeVacationModel) {
    return this.http.post<any>(this.baseUrl + 'Vacation/EditEmployeeVacation?EmployeeId=' + employeeId, model);
  }

  GetVacationTypesSelector() {
    return this.http.get<GeneralSelectorModel[]>(this.baseUrl + 'Vacation/GetVacationTypesSelector');
  }

  DeleteVacation(VacationId: number) {
    return this.http.get<any>(this.baseUrl + 'Vacation/DeleteVacation?VacationId=' + VacationId);
  }

  // ================================= Advances ==========================================

  GetAdvancesByEmployeeId(employeeId, model: PagingFilterModel) {
    return this.http.post<PagedResponseModel<EmployeeAdvanceModel[]>>(this.baseUrl + 'EmployeeAdvances/GetAdvancesByEmployeeId?EmployeeId=' + employeeId, model);
  }

  AddNewEmployeeAdvance(employeeId: number, model: EmployeeAdvanceModel) {
    return this.http.post<any>(this.baseUrl + 'EmployeeAdvances/AddNewEmployeeAdvance?EmployeeId=' + employeeId, model);
  }

  EditEmployeeAdvance(employeeId: number, model: EmployeeAdvanceModel) {
    return this.http.post<any>(this.baseUrl + 'EmployeeAdvances/EditEmployeeAdvance?EmployeeId=' + employeeId, model);
  }

  DeleteEmployeeAdvance(employeeAdvanceId: number) {
    return this.http.get<any>(this.baseUrl + 'EmployeeAdvances/DeleteEmployeeAdvance?EmployeeAdvanceId=' + employeeAdvanceId);
  }

  ApproveEmployeeAdvance(employeeAdvanceId: number, isApproved: boolean = true) {
    return this.http.get<any>(this.baseUrl + `EmployeeAdvances/ApproveEmployeeAdvance?EmployeeAdvanceId=${employeeAdvanceId}&IsApproved=${isApproved}`);
  }

  GetAdvanceTypesSelector() {
    return this.http.get<FormDropdownModel[]>(this.baseUrl + 'EmployeeAdvances/GetAdvanceTypesSelector');
  }

  GetEmployeeSalarySummary(year: number, month: number, model: PagingFilterModel) {
    year = year ?? new Date().getFullYear();
    month = month ?? new Date().getMonth() + 1;
    return this.http.post<PagedResponseModel<EmployeeSalarySummaryModel[]>>(this.baseUrl + 'Attendance/GetEmployeeSalarySummary?Year=' + year + '&Month=' + month, model);
  }

  // ================================= Attendance ==========================================

  AddAttendaceSalaries(Model: any) {
    return this.http.post<any>(this.baseUrl + 'Attendance/AddAttendaceSalaries', Model);
  }

  GetAllAttendanceSalaries(filter: PagingFilterModel) {
    return this.http.post<any>(this.baseUrl + 'Attendance/GetAllAttendanceSalaries', filter);
  }

  // ================================= Branches ==========================================

  GetAllBranches(filter: PagingFilterModel) {
    return this.http.post<any>(this.baseUrl + 'Branches/GetAllBranches', filter);
  }

}
