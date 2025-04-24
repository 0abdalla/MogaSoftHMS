import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaginatedPatients, Patients } from '../../../../core/models/patient';
import { AppointmentService } from '../../../../core/services/appointment.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { MessageService } from 'primeng/api';
declare var html2pdf: any;

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrl: './appointment-list.component.css',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-in', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        animate('200ms ease-out', style({ opacity: 0 })),
      ])
    ])
  ],
})
export class AppointmentListComponent implements OnInit {
  patients:Patients[] = [];
  patientServices: any[] = [];
  filterForm!: FormGroup;
  updateEmergencyForm!:FormGroup
  // 
  pageSize = 16;
  currentPage = 1;
  total = 0;
  fixed = Math.ceil(this.total / this.pageSize);
  // 
  selectedAppointment: any;
  // 
  clinics!:any;
  constructor(private appointmentService: AppointmentService , private fb : FormBuilder , private messageService : MessageService) {}
  ngOnInit() {
    this.filterForm = this.fb.group({
      Type: [''],
      Search: ['']
    });
    this.updateEmergencyForm = this.fb.group({
      newStatus: ['' , Validators.required],
      notes: ['']
    });
    this.getPatients();
    this.getCounts();
  }
  getServiceColor(type: string): string {
    const serviceObj = this.patientServices.find((s) => s.name === type);
    return serviceObj ? serviceObj.back : '#000';
  }
  print() {
    window.print();
  }
  
  exportToPDF() {
    const element = document.getElementById('pdfContent');
  
    const opt = {
      margin:       0.5,
      filename:     'booking-details.pdf',
      image:        { type: 'jpeg', quality: 1 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().set(opt).from(element).save();
  }
  // ==================================================================
  getPatients() {
    const { Search, Type } = this.filterForm.value;
    this.appointmentService.getAllAppointments(this.currentPage, this.pageSize, Type, Search).subscribe({
      next: (data) => {
        this.patients = data.data.map((patient:Patients) => {
          switch (patient.type) {
            case 'Emergency':
              patient.type = 'طوارئ';
              break;
            case 'Radiology':
              patient.type = 'أشعة';
              break;
            case 'Screening':
              patient.type = 'تحاليل';
              break;
            case 'Surgery':
              patient.type = 'عمليات';
              break;
            case 'Consultation':
              patient.type = 'استشارة';
              break;
            case 'General':
              patient.type = 'كشف';
              break;
          }
          return patient;
        });
        this.pageSize = data.pageSize;
        this.currentPage = data.pageIndex;
        this.total = data.count;
        this.fixed = Math.ceil(this.total / this.pageSize);
        console.log('data', data);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  applyFilters() {
    this.currentPage = 1;
    this.getPatients();
  }

  resetFilters() {
    this.filterForm.reset();
    this.currentPage = 1;
    this.getPatients();
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.getPatients();
  }
  openAppointmentModal(id: number) {
    this.appointmentService.getAppointmentById(id).subscribe({
      next: (data) => {
        this.selectedAppointment = data;
        console.log(this.selectedAppointment);
      },
      error: (err) => {
        console.error('Failed to fetch appointment', err);
      }
    });
  } 
  getCounts(){
    this.appointmentService.getCounts().subscribe({
      next: (data) => {
        this.patientServices = [
          { name: 'كشف', value: 'General', count: data.generalCount, color: 'linear-gradient(237.82deg, #1E90FF 30.69%, #A3D4FF 105.5%)', back: '#1E90FF' },
          { name: 'استشارة', value: 'Consultation', count: data.consultationCount, color: 'linear-gradient(236.62deg, #FFA500 30.14%, #FFDCA3 83.62%)', back: '#FFA500' },
          { name: 'عمليات', value: 'Surgery', count: data.surgeryCount, color: 'linear-gradient(227.58deg, #8B0000 26.13%, #FFB6B6 115.78%)', back: '#8B0000' },
          { name: 'تحاليل', value: 'Screening', count: data.screeningCount, color: 'linear-gradient(248.13deg, #FF6347 35.68%, #FFAAA5 99.61%)', back: '#FF6347' },
          { name: 'أشعة', value: 'Radiology', count: data.radiologyCount, color: 'linear-gradient(236.62deg, #20B2AA 30.14%, #A3E4E0 83.62%)', back: '#20B2AA' },
          { name: 'طوارئ', value: 'Emergency', count: data.emergencyCount, color: 'linear-gradient(236.62deg, #FF0000 30.14%, #FF9999 83.62%)', back: '#FF0000' },
        ];
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
  onSubmit() {
    if (this.updateEmergencyForm.invalid || !this.selectedAppointment) return;
  
    const id = this.selectedAppointment.id;
  
    this.appointmentService.updateEmergency(id, this.updateEmergencyForm).subscribe({
      next: (updatedPatient) => {
        console.log('تم التحديث بنجاح:', updatedPatient);
        this.messageService.add({ severity: 'success', summary: 'تم التحديث', detail: 'تم التحديث بنجاح' });
        this.getPatients();
        this.updateEmergencyForm.reset();
      },
      error: (err) => {
        console.error('حدث خطأ أثناء التحديث:', err);
        this.messageService.add({ severity: 'error', summary: 'فشل التحديث', detail: 'حدث خطأ أثناء التحديث' });
      }
    });
  }
  getServiceName(type: string): string {
    const map: { [key: string]: string } = {
      Emergency: 'طوارئ',
      Radiology: 'أشعة',
      Screening: 'تحاليل',
      Surgery: 'عمليات',
      Consultation: 'استشارة',
      General: 'كشف'
    };
    return map[type] || type;
  }  
}
