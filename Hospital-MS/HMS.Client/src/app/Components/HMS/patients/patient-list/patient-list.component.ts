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
    filterList: []
  };
  pagedResponseModel: PagedResponseModel<any> = {};
  // 
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
    this.filterForm
      .get('Search')
      .valueChanges.pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.currentPage = 1;
        this.loadPatients();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadPatients() {
    this.admissionService.getAddmision(this.pagingFilterModel).subscribe({
      next: (data) => {
        console.log("Data : " , data);
        this.patients = data.results.map((patient: any) => {
            switch (patient.patientStatus) {
              case 'CriticalCondition':
                patient.patientStatus = 'حالة حرجة';
                break;
              case 'Treated':
                patient.patientStatus = 'تم علاجه';
                break;
              case 'Archived':
                patient.patientStatus = 'أرشيف';
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
  // 
  // loadPatients() {
  //   const { Search, Status, FromDate, ToDate } = this.filterForm.value;
  //   const filterList = [...this.pagingFilterModel.filterList]; // Use the filterList from pagingFilterModel
  
  //   // Add additional filters from filterForm if present
  //   if (FromDate) {
  //     filterList.push({
  //       categoryName: 'Date',
  //       itemKey: 'createdOn',
  //       itemValue: FromDate,
  //       isChecked: true,
  //       fromDate: FromDate,
  //       toDate: '',
  //       filterType: 'date',
  //       isVisible: true,
  //       filterItems: [],
  //     });
  //   }
  //   if (ToDate) {
  //     filterList.push({
  //       categoryName: 'Date',
  //       itemKey: 'createdOn',
  //       itemValue: ToDate,
  //       isChecked: true,
  //       fromDate: '',
  //       toDate: ToDate,
  //       filterType: 'date',
  //       isVisible: true,
  //       filterItems: [],
  //     });
  //   }
  
  //   this.admissionService
  //     .getAddmision(
  //       this.pagingFilterModel.currentPage,
  //       this.pagingFilterModel.pageSize,
  //       this.pagingFilterModel.searchText || Search, // Prefer pagingFilterModel.searchText if set
  //       Status, // Keep Status from filterForm for now (optional, adjust as needed)
  //       FromDate,
  //       ToDate,
  //       filterList
  //     )
  //     .subscribe({
  //       next: (res: any) => {
  //         this.patients = res.results.map((patient: any) => {
  //           switch (patient.patientStatus) {
  //             case 'CriticalCondition':
  //               patient.patientStatus = 'حالة حرجة';
  //               break;
  //             case 'Treated':
  //               patient.patientStatus = 'تم علاجه';
  //               break;
  //             case 'Archived':
  //               patient.patientStatus = 'أرشيف';
  //               break;
  //             case 'Surgery':
  //               patient.patientStatus = 'عمليات';
  //               break;
  //             case 'Outpatient':
  //               patient.patientStatus = 'عيادات خارجية';
  //               break;
  //             case 'Staying':
  //               patient.patientStatus = 'إقامة';
  //               break;
  //           }
  //           return patient;
  //         });
  //         this.total = res.totalCount;
  //         this.fixed = Math.ceil(this.total / this.pageSize);
  //         this.filteredPatients = [...this.patients];
  //       },
  //       error: (err) => {
  //         console.error('Error loading patients', err);
  //         this.messageService.add({
  //           severity: 'error',
  //           summary: 'فشل التحميل',
  //           detail: 'حدث خطأ أثناء تحميل البيانات',
  //         });
  //       },
  //     });
  // }

  getCounts() {
    this.admissionService.getCounts().subscribe({
      next: (data) => {
        const statusCounts = {};
        data.results.forEach((item: { status: string; count: number }) => {
          statusCounts[item.status] = item.count;
        });
        this.patientStatuses = [
          {
            name: 'حالة حرجة',
            value: 'CriticalCondition',
            count: statusCounts['CriticalCondition'] || 0,
            color: 'linear-gradient(237.82deg, #F12B43 30.69%, #FCD5D9 105.5%)',
            back: '#F12B43',
          },
          {
            name: 'عمليات',
            value: 'Surgery',
            count: statusCounts['Surgery'] || 0,
            color: 'linear-gradient(236.62deg, #6A4C93 30.14%, #A98ECD 83.62%)',
            back: '#6A4C93',
          },
          {
            name: 'تم علاجه',
            value: 'Treated',
            count: statusCounts['Treated'] || 0,
            color: 'linear-gradient(227.58deg, #06A561 26.13%, #C4F8E2 115.78%)',
            back: '#06A561',
          },
          {
            name: 'إقامة',
            value: 'Staying',
            count: statusCounts['Staying'] || 0,
            color: 'linear-gradient(248.13deg, #3A86FF 35.68%, #87BFFF 99.61%)',
            back: '#3A86FF',
          },
          {
            name: 'عيادات خارجية',
            value: 'Outpatient',
            count: statusCounts['Outpatient'] || 0,
            color: 'linear-gradient(243.59deg, #00ACCE 33.41%, #7EDDF0 96.19%)',
            back: '#00ACCE',
          },
          {
            name: 'أرشيف',
            value: 'Archived',
            count: statusCounts['Archived'] || 0,
            color: 'linear-gradient(236.62deg, #6C757D 30.14%, #ADB5BD 83.62%)',
            back: '#6C757D',
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
    this.pagingFilterModel.filterList = this.sharedService.CreateFilterList('Type', item.value);
    console.log('Type', item.value);
    console.log('filterList', this.pagingFilterModel.filterList);
    console.log('Data:' , this.pagingFilterModel);
    this.loadPatients();
  }
  
  resetFilters() {
    this.filterForm = this.fb.group({
      Search: [''],
      Status: [''],
      FromDate: [''],
      ToDate: ['']
    });
    this.currentPage = 1;
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
      },
      error: (err) => {
        console.error('Failed to fetch admission data', err);
        this.messageService.add({ severity: 'error', summary: 'فشل التحميل', detail: 'حدث خطأ أثناء تحميل البيانات' });
      }
    });
  }
  onPageChange(page: number) {
    this.currentPage = page;
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
