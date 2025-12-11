import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PagingFilterModel } from '../../Models/Generics/PagingFilterModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  baseUrl: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getPatients(pagingFilter: PagingFilterModel) {

    let params = new HttpParams()
      .set('SearchText', pagingFilter.searchText ?? '')
      .set('CurrentPage', pagingFilter.currentPage)
      .set('PageSize', pagingFilter.pageSize);

    pagingFilter.filterList.forEach((f: any, index: number) => {
      params = params
        .set(`FilterList[${index}].CategoryName`, f.categoryName ?? '')
        .set(`FilterList[${index}].ItemValue`, f.itemValue ?? '')
        .set(`FilterList[${index}].FromDate`, f.fromDate ?? '')
        .set(`FilterList[${index}].ToDate`, f.toDate ?? '');
    });

    return this.http.get<any>(this.baseUrl + 'Patients', { params });
  }

  getPatientById(id: number) {
    return this.http.get<any>(this.baseUrl + 'Patients/' + id);
  }


  updatePatient(id: number, patient: any) {
    return this.http.put<any>(this.baseUrl + 'Patients/' + id, patient);
  }


  getAdmissions(requestModel: any) {
    return this.http.post<any>(this.baseUrl + 'Patients/GetAdmissions', requestModel);
  }


  // getMedicalHistory(patientId: number) {
  //   return this.http.get<any>(this.baseUrl + `Patients/GetMedicalHistory/${patientId}`);
  // }
  getMedicalHistory(id: number) {
    return this.http.get(`${this.baseUrl}Patients/medical-history/${id}`)
  }

  addMedicalHistory(model: any) {
    return this.http.post<any>(this.baseUrl + 'Patients/AddMedicalHistory', model);
  }


  changePatientStatus(patientId: number, newStatus: string, notes?: string) {
    return this.http.put<any>(
      `${this.baseUrl}Patients/status/${patientId}`,
      { newStatus, notes }
    );
  }



  getPatientCounts() {
    return this.http.get<any>(this.baseUrl + 'Patients/GetCounts');
  }

  getAdmissionHistory(patientId: number) {
    return this.http.get<any>(
      this.baseUrl + `Patients/GetAdmissionHistory/${patientId}`
    );
  }




  getPatientAccount(patientId: number) {
    return this.http.get<any>(
      this.baseUrl + `Patients/GetPatientAccount/${patientId}`
    );
  }

  getCounts(filterList: any[] = []): Observable<any> {
    let params = new HttpParams().set('FilterList', JSON.stringify(filterList));
    return this.http.get<any>(`${this.baseUrl}patients/counts`, { params });
  }

}
