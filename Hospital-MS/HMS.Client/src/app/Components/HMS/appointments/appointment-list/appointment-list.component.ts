import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { trigger, transition, style, animate } from '@angular/animations';
import { MessageService } from 'primeng/api';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { Patients } from '../../../../Models/HMS/patient';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { SharedService } from '../../../../Services/shared.service';
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
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  pagedResponseModel: PagedResponseModel<any> = {};
  patients: Patients[] = [];
  patientServices: any[] = [];
  filterForm!: FormGroup;
  updateEmergencyForm!: FormGroup
  total = 0;
  // 
  selectedAppointment: any;
  // 
  clinics!: any;
  constructor(private appointmentService: AppointmentService, private fb: FormBuilder, private messageService: MessageService,
    private sharedService: SharedService) { }
  ngOnInit() {
    this.filterForm = this.fb.group({
      Type: [''],
      Search: ['']
    });
    this.updateEmergencyForm = this.fb.group({
      newStatus: ['', Validators.required],
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
      margin: 0.5,
      filename: 'booking-details.pdf',
      image: { type: 'jpeg', quality: 1 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };

    html2pdf().set(opt).from(element).save();
  }
  // ==================================================================
  getPatients() {
    this.appointmentService.getAllAppointments(this.pagingFilterModel).subscribe({
      next: (data) => {
        debugger;
        this.patients = data.results.map((patient: Patients) => {
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
        this.total = data.totalCount;
        console.log(this.patients);
        
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  applyFilters() {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = this.sharedService.CreateFilterList('Type', this.filterForm.value.Type);
    this.pagingFilterModel.searchText = this.filterForm.value.Search;
    this.getPatients();
  }

  resetFilters() {
    this.filterForm.reset();
    this.filterForm.patchValue({ Type: '' });
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = [];
    this.pagingFilterModel.searchText = '';
    this.getPatients();
  }

  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.getPatients();
  }
  openAppointmentModal(id: number) {
    this.appointmentService.getAppointmentById(id).subscribe({
      next: (data) => {
        this.selectedAppointment = data.results;
        console.log(this.selectedAppointment);
      },
      error: (err) => {
        console.error('Failed to fetch appointment', err);
      }
    });
  }
  getCounts() {
    this.appointmentService.getCounts(this.pagingFilterModel).subscribe({
      next: (data) => {
        this.patientServices = data.results;
        console.log(this.patientServices);
        console.log(data);
        
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
        if (updatedPatient.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'تم التحديث', detail: 'تم التحديث بنجاح' });
          this.getPatients();
          this.getCounts();
          this.updateEmergencyForm.reset();
        } else
        this.messageService.add({ severity: 'error', summary: 'فشل التحديث', detail: 'لا يمكن تحديث إلا صاحب الخدمة الطبية طوارئ' });

        this.sharedService.closeModal('updateEmergencyModal');
      },
      error: (err) => {
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
