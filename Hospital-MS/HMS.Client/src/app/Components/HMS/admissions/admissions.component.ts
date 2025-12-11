import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AdmissionService } from '../../../Services/HMS/admission.service';
import { FilterModel, PagingFilterModel } from '../../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../../Models/Generics/PagedResponseModel';

@Component({
  selector: 'app-admissions',
  templateUrl: './admissions.component.html',
  styleUrls: ['./admissions.component.css']
})
export class AdmissionsComponent implements OnInit {

  admissions: any[] = [];
  totalRecords = 0;
  pagingFilter: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 10,
    filterList: []
  };

  loading = false;

  departments: any[] = [];
  wards: any[] = [];
  rooms: any[] = [];
  beds: any[] = [];
  counts: any = {};

  admissionForm: FormGroup;

  constructor(private admissionService: AdmissionService, private fb: FormBuilder) {
    this.admissionForm = this.fb.group({
      PatientName: [''],
      PatientPhone: [''],
      PatientBirthDate: [''],
      PatientNationalId: [''],
      PatientAddress: [''],
      PatientGender: [''],
      DoctorId: [''],
      DepartmentId: [''],
      RoomId: [''],
      BedId: [''],
      AdmissionType: [''],
      PaymentMethod: [''],
      HealthStatus: [''],
      InitialDiagnosis: [''],
      Notes: [''],
      HasCompanion: [false],
      CompanionName: [''],
      CompanionPhone: [''],
      CompanionNationalId: ['']
    });
  }

  ngOnInit(): void {
    this.loadAdmissions();
    this.loadDepartments();
    this.loadRoomsBeds();
    this.loadCounts();
  }

  loadAdmissions(): void {
    this.loading = true;
    this.admissionService.getAdmissions(this.pagingFilter).subscribe({
      next: res => {
        this.admissions = res.results;
        this.totalRecords = res.totalCount;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onSearch(searchText: string): void {
    this.pagingFilter.searchText = searchText;
    this.pagingFilter.currentPage = 1;
    this.loadAdmissions();
  }

  changePage(page: number): void {
    this.pagingFilter.currentPage = page;
    this.loadAdmissions();
  }


  loadDepartments(): void {
    this.admissionService.getDepartments().subscribe(res => this.departments = res);
  }

  loadRoomsBeds(): void {
    this.admissionService.getRooms().subscribe(res => this.rooms = res);
    this.admissionService.getBeds().subscribe(res => this.beds = res);
  }

  loadCounts(): void {
    this.admissionService.getCounts().subscribe(res => this.counts = res.results);
  }

  getAdmissionById(id: number): void {
    this.admissionService.getAdmissionById(id).subscribe(res => {
      this.admissionForm.patchValue(res.results);
    });
  }

  addAdmission(): void {
    const formData = new FormData();
    Object.keys(this.admissionForm.value).forEach(key => formData.append(key, this.admissionForm.value[key]));
    this.admissionService.addAdmission(formData).subscribe(() => {
      this.loadAdmissions();
      this.admissionForm.reset();
    });
  }

  updateAdmission(id: number): void {
    const formData = new FormData();
    Object.keys(this.admissionForm.value).forEach(key => formData.append(key, this.admissionForm.value[key]));
    this.admissionService.updateAdmission(id, formData).subscribe(() => {
      this.loadAdmissions();
      this.admissionForm.reset();
    });
  }

  deleteAdmission(id: number): void {
    this.admissionService.deleteAdmission(id).subscribe(() => this.loadAdmissions());
  }

  getMedicalHistory(patientId: number): void {
    this.admissionService.getMedicalHistory(patientId).subscribe(res => console.log(res));
  }


}
