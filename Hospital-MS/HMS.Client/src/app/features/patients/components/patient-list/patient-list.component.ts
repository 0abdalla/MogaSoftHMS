import { Component } from '@angular/core';
import { AdmissionService } from '../../../../core/services/admission.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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
  pageSize = 16;
  currentPage = 1;
  total = 0;
  fixed = Math.ceil(this.total / this.pageSize);
  constructor(private admissionService: AdmissionService , private fb : FormBuilder) {}

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
      }
    });
  }

  getCounts(){
    this.admissionService.getCounts().subscribe({
      next: (data) => {
        this.patientStatuses = [
          { name: 'إقامة', value : 'Staying' , count: data.stayingCount, color: '#1E90FF' },
          { name: 'عمليات', value : 'Surgery' , count: data.surgeryCount, color: '#8B0000' },
          { name: 'حالة حرجة', value : 'CriticalCondition' , count: data.criticalConditionCount, color: '#FF0000' },
          { name: 'تم علاجه', value : 'Treated' , count: data.treatedCount, color: '#008000' },
          { name: 'عيادات خارجية', value : 'Outpatient' , count: data.outpatientCount, color: '#FFA500' },
          { name: 'أرشيف', value : 'Archived' , count: data.archivedCount, color: '#808080' },
        ];
      },
      error: (err) => {
        console.log(err);
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
    return statusObj ? statusObj.color : '#000';
  }
  openAdmissionDetails(id: number) {
    this.getAdmissionById(id);
  }
  getAdmissionById(id: number) {
    this.admissionService.getAddmisionById(id).subscribe({
      next: (res) => {
        this.admissionDetails = res;
        console.log('Admission data:', this.admissionDetails);
      },
      error: (err) => {
        console.error('Failed to fetch admission data', err);
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
          this.loadPatients();
          this.statusForm.reset();
        },
        error: (err) => {
          console.error('Failed to update status', err);
        }
      });
    }
  }
}
