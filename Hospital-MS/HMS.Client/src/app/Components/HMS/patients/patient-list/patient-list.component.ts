import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { SharedService } from '../../../../Services/shared.service';
import { PatientService } from '../../../../Services/HMS/patient.service';
import { Router } from '@angular/router';
declare var bootstrap: any;

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css',
})
export class PatientListComponent {
  TitleList = ['المرضى'];
  filterForm!: FormGroup;
  statusForm!: FormGroup;
  isFilter = true;

  patients!: any[];
  patientStatuses!: any[];
  patientDetails: any;
  medicalHistory!: any;

  pageSize = 16;
  currentPage = 1;
  total = 0;

  private destroy$ = new Subject<void>();

  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterType: '',
    filterItems: [],
    filterList: [],
  };
  Statues = [
    { value: 'Active', name: 'نشط' },
    { value: 'Inactive', name: 'غير نشط' },
    { value: 'Emergency', name: 'طوارئ' },
    { value: 'Outpatient', name: 'عيادات خارجية' },
    { value: 'Inpatient', name: 'منوم / إقامة داخلية' },
    { value: 'Deceased', name: 'متوفي' },
    { value: 'Archived', name: 'أرشيف' }
  ];


  constructor(
    private admissionService: AdmissionService,
    private patientService: PatientService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private sharedService: SharedService,
    private router: Router
  ) {
    this.filterForm = this.fb.group({
      Search: [''],
      Status: [''],
      FromDate: [''],
      ToDate: [''],
    });

    this.statusForm = this.fb.group({
      newStatus: ['', Validators.required],
      notes: [''],
    });
  }

  ngOnInit(): void {
    this.loadPatients();
    this.getCounts();

    this.filterForm.get('Search')!.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe((searchText) => {
      this.pagingFilterModel.searchText = searchText;
      this.pagingFilterModel.currentPage = 1;
      this.loadPatients();
    });

    this.filterForm.get('Status')!.valueChanges.pipe(
      takeUntil(this.destroy$)
    ).subscribe((status) => {
      if (status) {
        this.pagingFilterModel.filterList = [
          {
            categoryName: 'Status',
            itemId: status,
            itemKey: 'PatientStatus',
            itemValue: status,
            isChecked: true,
            filterType: 'PatientStatus',
          },
        ];
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
    this.patientService.getPatients(this.pagingFilterModel).subscribe({
      next: (data) => {
        this.patients = data.results.map((patient: any) => {
          // Patient status is already translated by backend
          return patient;
        });
        this.total = data.totalCount;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'فشل التحميل',
          detail: 'حدث خطأ أثناء تحميل البيانات',
        });
      },
    });
  }

  getCounts() {
    this.patientService.getCounts().subscribe({
      next: (data) => {
        const statusCounts: any = {};
        data.results.forEach((item: { status: string; count: number }) => {
          statusCounts[item.status] = item.count;
        });
        this.patientStatuses = [
          { name: 'نشط', value: "Active", count: statusCounts["Active"] || 0, color: 'linear-gradient(237.82deg, #F12B43 30.69%, #FCD5D9 105.5%)', back: '#F12B43', img: '../../../../../assets/vendors/imgs/Vector 2.png' },
          { name: 'غير نشط', value: "Inactive", count: statusCounts["Inactive"] || 0, color: 'linear-gradient(236.62deg, #6A4C93 30.14%, #A98ECD 83.62%)', back: '#6A4C93', img: '../../../../../assets/vendors/imgs/purble.png' },
          { name: 'طوارئ', value: "Emergency", count: statusCounts["Emergency"] || 0, color: 'linear-gradient(227.58deg, #06A561 26.13%, #C4F8E2 115.78%)', back: '#06A561', img: '../../../../../assets/vendors/imgs/green.png' },
          { name: 'منوم / إقامة داخلية', value: "Inpatient", count: statusCounts["Inpatient"] || 0, color: 'linear-gradient(248.13deg, #3A86FF 35.68%, #87BFFF 99.61%)', back: '#3A86FF', img: '../../../../../assets/vendors/imgs/blue.png' },
          { name: 'عيادات خارجية', value: "Outpatient", count: statusCounts["Outpatient"] || 0, color: 'linear-gradient(243.59deg, #00ACCE 33.41%, #7EDDF0 96.19%)', back: '#00ACCE', img: '../../../../../assets/vendors/imgs/bluee.png' },
          { name: 'متوفي', value: "Deceased", count: statusCounts["Deceased"] || 0, color: 'linear-gradient(236.62deg, #6C757D 30.14%, #ADB5BD 83.62%)', back: '#6C757D', img: '../../../../../assets/vendors/imgs/grey.png' },
          { name: 'أرشيف', value: "Archived", count: statusCounts["Archived"] || 0, color: 'linear-gradient(236.62deg, #6C757D 30.14%, #ADB5BD 83.62%)', back: '#6C757D', img: '../../../../../assets/vendors/imgs/grey.png' },
        ];
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'فشل التحميل',
          detail: 'حدث خطأ أثناء تحميل البيانات',
        });
      },
    });
  }

  filterChecked(filters: FilterModel[]) {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.searchText = filters.find(f => f.categoryName === 'SearchText')?.itemValue ?? '';
    this.loadPatients();
  }

  ApplyCardFilter(item: any) {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = this.sharedService.CreateFilterList('Status', item.value);
    this.loadPatients();
  }

  resetFilters() {
    this.filterForm.reset({ Status: '' });
    this.pagingFilterModel = { searchText: '', currentPage: 1, pageSize: 16, filterList: [] };
    this.loadPatients();
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'نشط':
        return '#06A561';
      case 'غير نشط':
        return '#6C757D';
      case 'طوارئ':
        return '#F12B43';
      case 'عيادات خارجية':
        return '#00ACCE';
      case 'منوم / إقامة داخلية':
        return '#3A86FF';
      case 'متوفي':
        return '#000000';
      case 'أرشيف':
        return '#ADB5BD';
      default:
        return '#000';
    }
  }



  openPatientDetails(id: number) {
    this.getPatientById(id);
  }

  getPatientById(id: number) {
    this.patientService.getPatientById(id).subscribe({
      next: (res) => {
        console.log('res', res.results)
        this.patientDetails = res.results;
        this.patientDetails.patientStatusArabic = this.mapStatusToArabic(this.patientDetails.patientStatus);
        this.getMedicalHistory(id);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'فشل التحميل', detail: 'حدث خطأ أثناء تحميل البيانات' });
      },
    });
  }

  getMedicalHistory(id: number) {
    this.patientService.getMedicalHistory(id).subscribe({
      next: (res: any) => {
        this.medicalHistory = res.results;
        console.log('add', res.results)
      },
      error: () => { this.messageService.add({ severity: 'error', summary: 'فشل التحميل', detail: 'حدث خطأ أثناء تحميل البيانات' }); },
    });
  }

  mapStatusToArabic(status: string): string {
    switch (status) {
      case 'Pending': return 'قيد الانتظار';
      case 'Completed': return 'مكتمل';
      case 'Cancelled': return 'ملغي';
      case 'Staying': return 'إقامة';
      case 'IntensiveCare': return 'عناية مركزة';
      case 'Treated': return 'تم علاجه';
      case 'CriticalCondition': return 'رعاية مركزة';
      case 'Archived': return 'حضانات الأطفال';
      case 'Surgery': return 'عمليات';
      case 'Outpatient': return 'عيادات خارجية';
      case 'Inpatient': return 'منوم / إقامة داخلية';
      case 'Active': return 'نشط';
      case 'Inactive': return 'غير نشط';
      case 'Emergency': return 'طوارئ';
      case 'Deceased': return 'متوفي';
      case 'Archived': return 'أرشيف';
      default: return status;
    }
  }
  mapTypeToArabic(type: string): string { switch (type) { case 'General': return 'كشف'; case 'Consultation': return 'إستشارة'; case 'Radiology': return 'أشعة'; case 'Screening': return 'تحاليل'; case 'Surgery': return 'عمليات'; case 'Emergency': return 'طوارئ'; case 'CriticalCondition': return 'حالة حرجة'; case 'Treated': return 'تم علاجه'; case 'Archived': return 'أرشيف'; case 'Surgery': return 'عمليات'; default: return type; } }

  onPageChange(page: any) {
    this.pagingFilterModel.currentPage = page.page;
    this.loadPatients();
  }

  openStatusUpdateModal() {
    if (!this.patientDetails?.id) return;
    this.statusForm.reset({ newStatus: '' });
    const modal = new bootstrap.Modal(document.getElementById('patientModal')!);
    modal.show();
  }

  updateStatus() {
    if (!this.statusForm.valid) return;

    const newStatus = this.statusForm.value.newStatus;
    const notes = this.statusForm.value.notes;

    this.patientService.changePatientStatus(this.patientDetails.id, newStatus, notes)
      .subscribe({
        next: () => {
          console.log('success');
          this.messageService.add({

            severity: 'success',
            summary: 'تم التحديث',
            detail: 'تم تحديث حالة المريض بنجاح'
          });
          this.loadPatients();
          this.statusForm.reset();
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'فشل التحديث',
            detail: 'حدث خطأ أثناء تحديث الحالة'
          });
        }
      });
  }


  getAge(dateOfBirth: string): number {
    const birthDate = new Date(dateOfBirth);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) age--;
    return age;
  }

  filterByStatus(statusValue: string) {
    this.filterForm.patchValue({ Status: statusValue });
    this.pagingFilterModel.currentPage = 1;
    this.loadPatients();
  }

  backToMainModal(currentModalId: string, mainModalId: string = 'inpatientDetailsModal') {
    const currentModalEl = document.getElementById(currentModalId);
    const mainModalEl = document.getElementById(mainModalId);
    const currentModal = bootstrap.Modal.getInstance(currentModalEl!);
    const mainModal = new bootstrap.Modal(mainModalEl!);
    if (currentModal) {
      currentModal.hide();
      setTimeout(() => {
        document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
        document.body.classList.add('modal-open');
        mainModal.show();
      }, 100);
    }
  }

  goToAppointments(patient: any) {
    this.router.navigate(['/hms/appointments/add'], {
      state: {
        patientData: {
          patientName: patient?.patientName,
          patientPhone: patient?.phone,
          gender: patient?.patientGender === 'ذكر' ? 'Male' : 'Female',
        },
      },
    });
  }
}
