import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { SharedService } from '../../../../Services/shared.service';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css'
})
export class PatientListComponent {
  filterForm!:FormGroup;
  statusForm!:FormGroup;
  // 
  patients!: any[];
  patientStatuses!:any[]
  filteredPatients!: any[];
  // 
  admissionDetails: any;
  // 
  pageSize = 8;
  currentPage = 1;
  total = 0;
  fixed = Math.ceil(this.total / this.pageSize);
  // 
  private destroy$ = new Subject<void>();
  // 
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterType: '',
    filterItems: [],
    filterList: []
  };
  pagedResponseModel: PagedResponseModel<any> = {};
  // 
  medicalHistory!:any;
  constructor(private admissionService: AdmissionService , private fb : FormBuilder , private messageService : MessageService , private sharedService : SharedService) {
    this.filterForm = this.fb.group({
      Search: [''],
      Status: [''],
      FromDate: [''],
      ToDate: ['']
    });
    this.statusForm = this.fb.group({
      newStatus: ['' , Validators.required],
      notes:['']
    });
  }

  ngOnInit(): void {
    this.loadPatients();
    this.getCounts();
    this.filterForm.get('Search').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
      ).subscribe((searchText) => {
          this.pagingFilterModel.searchText = searchText;
          this.pagingFilterModel.currentPage = 1;
          this.loadPatients();
      });
    this.filterForm.get('Status').valueChanges.pipe(
      takeUntil(this.destroy$)
    ).subscribe((status) => {
      if (status) {
          this.pagingFilterModel.filterList = [{
              categoryName: 'PatientStatus',
              itemId: status,
              itemKey: 'PatientStatus',
              itemValue: status,
              isChecked: true,
              filterType: 'PatientStatus'
          }];
      } else {
          this.pagingFilterModel.filterList = [];
      }
      this.pagingFilterModel.currentPage = 1;
      this.loadPatients();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadPatients() {
    const requestModel = {
      searchText: this.pagingFilterModel.searchText,
      currentPage: this.pagingFilterModel.currentPage,
      pageSize: this.pagingFilterModel.pageSize,
      filterList: this.pagingFilterModel.filterList,
      filterType: this.pagingFilterModel.filterType,
      filterItems: this.pagingFilterModel.filterItems
  };
    this.admissionService.getAddmision(requestModel).subscribe({
      next: (data) => {
        console.log("Data : " , data);
        this.patients = data.results.map((patient: any) => {
            switch (patient.patientStatus) {
              case 'CriticalCondition':
                patient.patientStatus = 'رعاية مركزة';
                break;
              case 'Treated':
                patient.patientStatus = 'تم علاجه';
                break;
              case 'Archived':
                patient.patientStatus = 'حضانات الأطفال';
                break;
              case 'Surgery':
                patient.patientStatus = 'عمليات';
                break;
              case 'Outpatient':
                patient.patientStatus = 'عيادات خارجية';
                break;
              case 'Staying':
                patient.patientStatus = 'إقامة';
                break;
            }
            return patient;
          });
        this.total = data.totalCount;
        console.log("Patients : " , this.patients);
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({
          severity: 'error',
          summary: 'فشل التحميل',
          detail: 'حدث خطأ أثناء تحميل البيانات',
        });
      },
    });
  }
  

  getCounts() {
    this.admissionService.getCounts().subscribe({
      next: (data) => {
        const statusCounts = {};
        data.results.forEach((item: { status: string; count: number }) => {
          statusCounts[item.status] = item.count;
        });
        this.patientStatuses = [
          {
            name: 'رعاية مركزة',
            value: 'CriticalCondition',
            count: statusCounts['CriticalCondition'] || 0,
            color: 'linear-gradient(237.82deg, #F12B43 30.69%, #FCD5D9 105.5%)',
            back: '#F12B43',
            img: '../../../../../assets/vendors/imgs/Vector 2.png'
          },
          {
            name: 'عمليات',
            value: 'Surgery',
            count: statusCounts['Surgery'] || 0,
            color: 'linear-gradient(236.62deg, #6A4C93 30.14%, #A98ECD 83.62%)',
            back: '#6A4C93',
            img: '../../../../../assets/vendors/imgs/purble.png'
          },
          {
            name: 'تم علاجه',
            value: 'Treated',
            count: statusCounts['Treated'] || 0,
            color: 'linear-gradient(227.58deg, #06A561 26.13%, #C4F8E2 115.78%)',
            back: '#06A561',
            img: '../../../../../assets/vendors/imgs/green.png'
          },
          {
            name: 'إقامة',
            value: 'Staying',
            count: statusCounts['Staying'] || 0,
            color: 'linear-gradient(248.13deg, #3A86FF 35.68%, #87BFFF 99.61%)',
            back: '#3A86FF',
            img: '../../../../../assets/vendors/imgs/blue.png'
          },
          {
            name: 'عيادات خارجية',
            value: 'Outpatient',
            count: statusCounts['Outpatient'] || 0,
            color: 'linear-gradient(243.59deg, #00ACCE 33.41%, #7EDDF0 96.19%)',
            back: '#00ACCE',
            img: '../../../../../assets/vendors/imgs/bluee.png'
          },
          {
            name: 'حضانات الأطفال',
            value: 'Archived',
            count: statusCounts['Archived'] || 0,
            color: 'linear-gradient(236.62deg, #6C757D 30.14%, #ADB5BD 83.62%)',
            back: '#6C757D',
            img: '../../../../../assets/vendors/imgs/grey.png'
          },
        ];
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({
          severity: 'error',
          summary: 'فشل التحميل',
          detail: 'حدث خطأ أثناء تحميل البيانات',
        });
      },
    });
  }

  applyFilters() {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = this.sharedService.CreateFilterList('Type', this.filterForm.value.Type);
    this.pagingFilterModel.searchText = this.filterForm.value.Search;
    this.loadPatients();
  }

  ApplyCardFilter(item: any) {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterType = 'Type';
    this.pagingFilterModel.filterItems = [item.value];
    this.loadPatients();
  }
  
  resetFilters() {
    this.filterForm.reset();
    this.pagingFilterModel = {
        searchText: '',
        currentPage: 1,
        pageSize: 16, 
        filterList: []
    };
    this.loadPatients();
  }

  getStatusColor(status: string): string {    
    if (!this.patientStatuses) {
      return '#000';
    }
    const statusObj = this.patientStatuses.find((s) => s.name === status);
    return statusObj ? statusObj.back : '';
  }
  openAdmissionDetails(id: number) {
    this.getAdmissionById(id);
  }
  getAdmissionById(id: number) {
    this.admissionService.getPatientById(id).subscribe({
      next: (res) => {
        this.admissionDetails = res.results;
        console.log('Admission data:', this.admissionDetails);
        this.getMedicalHistory(id);
      },
      error: (err) => {
        console.error('Failed to fetch admission data', err);
        this.messageService.add({ severity: 'error', summary: 'فشل التحميل', detail: 'حدث خطأ أثناء تحميل البيانات' });
      }
    });
  }
  getMedicalHistory(id:number){
    this.admissionService.getMedicalHistory(id).subscribe({
      next: (res:any) => {
        this.medicalHistory = res.results;
        console.log('Medical history:', this.medicalHistory);
      },
      error: (err) => {
        console.error('Failed to fetch medical history', err);
        this.messageService.add({ severity: 'error', summary: 'فشل التحميل', detail: 'حدث خطأ أثناء تحميل البيانات' });
      }
    });
  }
  mapTypeToArabic(type: string): string {
    switch (type) {
      case 'General': return 'كشف';
      case 'Consultation': return 'إستشارة';
      case 'Radiology': return 'أشعة';
      case 'Screening': return 'تحاليل';
      case 'Surgery': return 'عمليات';
      case 'Emergency': return 'طوارئ';
      case 'CriticalCondition': return 'حالة حرجة';
      case 'Treated': return 'تم علاجه';
      case 'Archived': return 'أرشيف';
      case 'Surgery': return 'عمليات';
      default: return type;
    }
  }
  
  mapStatusToArabic(status: string): string {
    switch (status) {
      case 'Pending': return 'قيد الانتظار';
      case 'Completed': return 'مكتمل';
      case 'Cancelled': return 'ملغي';
      case 'Staying' : return 'إقامة';
      default: return status;
    }
  }
  
  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.loadPatients();
  }
  // 
  openStatusUpdateModal() {
    this.statusForm.reset();
    if (this.admissionDetails?.patientStatus) {
      this.statusForm.patchValue({ newStatus: this.admissionDetails.patientStatus });
    }
  }
  updateStatus() {
    if (this.statusForm.valid) {
      this.admissionService.updateAdmision(this.admissionDetails.patientId, this.statusForm.value).subscribe({
        next: (res) => {
          console.log('Status updated successfully', res);
          this.messageService.add({ severity: 'success', summary: 'تم التحديث', detail: 'تم التحديث بنجاح' });
          this.loadPatients();
          this.statusForm.reset();
        },
        error: (err) => {
          console.error('Failed to update status', err);
          this.messageService.add({ severity: 'error', summary: 'فشل التحديث', detail: 'حدث خطأ أثناء التحديث' });
        }
      });
    }
  }
  getAge(dateOfBirth: string): number {
    const birthDate = new Date(dateOfBirth);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
  
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
  
    return age;
  }
  // 
  filterByStatus(statusValue: string) {
    this.filterForm.patchValue({ Status: statusValue });
    this.currentPage = 1;
    this.loadPatients();
  }
}
