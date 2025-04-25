import { Component } from '@angular/core';
import { AdmissionService } from '../../../../core/services/admission.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css'
})
export class PatientListComponent {
  filterForm!:FormGroup;
  statusForm!:FormGroup;
  // 
  patients!: any;
  patientStatuses!:any[]
  filteredPatients!: any[];
  // 
  admissionDetails: any;
  // 
  pageSize = 8;
  currentPage = 1;
  total = 0;
  fixed = Math.ceil(this.total / this.pageSize);
  constructor(private admissionService: AdmissionService , private fb : FormBuilder , private messageService : MessageService) {}

  ngOnInit(): void {
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
    this.loadPatients();
    this.getCounts();
  }

  loadPatients() {
    const { Search, Status , FromDate , ToDate } = this.filterForm.value;
    this.admissionService.getAddmision(this.currentPage, this.pageSize , Search, Status , FromDate , ToDate ).subscribe({
      next: (res:any) => {
        this.patients = res.data.map((patient:any)=>{
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
        this.pageSize = res.pageSize;
        this.currentPage = res.pageIndex;
        this.total = res.count;
        this.fixed = Math.ceil(this.total / this.pageSize);
        console.log('data', res);
        this.filteredPatients = [...this.patients];
      },
      error: (err) => {
        console.error('Error loading patients', err);
        this.messageService.add({ severity: 'error', summary: 'فشل التحميل', detail: 'حدث خطأ أثناء تحميل البيانات' });
      }
    });
  }

  getCounts(){
    this.admissionService.getCounts().subscribe({
      next: (data) => {
        this.patientStatuses = [
          { name: 'حالة حرجة', value : 'CriticalCondition' , count: data.criticalConditionCount, color: 'linear-gradient(237.82deg, #F12B43 30.69%, #FCD5D9 105.5%)' , back:'#F12B43' },
          { name: 'عمليات', value : 'Surgery' , count: data.surgeryCount, color: 'linear-gradient(236.62deg, #6A4C93 30.14%, #A98ECD 83.62%)' , back : '#6A4C93' },
          { name: 'تم علاجه', value : 'Treated' , count: data.treatedCount, color: 'linear-gradient(227.58deg, #06A561 26.13%, #C4F8E2 115.78%)' , back : '#06A561' },
          { name: 'إقامة', value : 'Staying' , count: data.stayingCount, color: 'linear-gradient(248.13deg, #3A86FF 35.68%, #87BFFF 99.61%)' , back : '#3A86FF' },
          { name: 'عيادات خارجية', value : 'Outpatient' , count: data.outpatientCount, color: 'linear-gradient(236.62deg, #6A4C93 30.14%, #A98ECD 83.62%)' , back : '#6A4C93' },
          { name: 'أرشيف', value : 'Archived' , count: data.archivedCount, color: 'linear-gradient(236.62deg, #6C757D 30.14%, #ADB5BD 83.62%)' , back : '#6C757D' },
        ];
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({ severity: 'error', summary: 'فشل التحميل', detail: 'حدث خطأ أثناء تحميل البيانات' });
      }
    });
  }

  applyFilters(event: Event) {
    event.preventDefault();
    console.log('Filter Values:', this.filterForm.value);
    this.currentPage = 1;
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
        this.admissionDetails = res;
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
}
