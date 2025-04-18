import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaginatedPatients, Patients } from '../../../../core/models/patient';
import { AppointmentService } from '../../../../core/services/appointment.service';
import { trigger, transition, style, animate } from '@angular/animations';
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
  constructor(private appointmentService: AppointmentService , private fb : FormBuilder) {}
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
    return serviceObj ? serviceObj.color : '#000';
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
          { name: 'كشف', count: data.generalCount, color: '#1E90FF' },
          { name: 'استشارة', count: data.consultationCount, color: '#FFA500' },
          { name: 'عمليات', count: data.surgeryCount, color: '#8B0000' },
          { name: 'تحاليل', count: data.screeningCount, color: '#FF6347' },
          { name: 'أشعة', count: data.radiologyCount, color: '#20B2AA' },
          { name: 'طوارئ', count: data.emergencyCount, color: '#FF0000' },
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
        this.getPatients();
        this.updateEmergencyForm.reset();
      },
      error: (err) => {
        console.error('حدث خطأ أثناء التحديث:', err);
      }
    });
  }
  
}
