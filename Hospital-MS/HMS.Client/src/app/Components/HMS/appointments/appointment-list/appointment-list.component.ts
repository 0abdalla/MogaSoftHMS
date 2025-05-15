import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
    private sharedService: SharedService, private cdr: ChangeDetectorRef) { }
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
    const printContent = document.getElementById('pdfContent');
    if (!printContent) return;

    const content = printContent.outerHTML;
    const printWindow = window.open('', '', 'width=800,height=600');
    if (!printWindow) return;

    printWindow.document.open();
    printWindow.document.write(`
    <html dir="rtl">
      <head>
        <title>تفاصيل الحجز</title>
        <style>
          body { font-family: Arial; padding: 20px; direction: rtl; }
          .strong { font-weight: bold; }
          .span { margin-right: 5px; }
        </style>
      </head>
      <body>${content}</body>
    </html>
  `);
    printWindow.document.close();
    printWindow.focus();
    printWindow.onafterprint = () => {
      printWindow.close();
    };
    printWindow.print();
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
        this.patients = data.results;
        this.total = data.totalCount;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'فشل', detail: 'حدث خطأ أثناء جلب البيانات' });
      }
    });
  }

  applyFilters() {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = this.sharedService.CreateFilterList('Type', this.filterForm.value.Type);
    this.pagingFilterModel.searchText = this.filterForm.value.Search;
    this.getPatients();
  }
  ApplyCardFilter(item: any) {
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = this.sharedService.CreateFilterList('Type', item.value);
    this.getPatients();
  }

  SearchTextChange() {
    if (this.filterForm.value.Search.length > 2 || this.filterForm.value.Search.length == 0) {
      this.pagingFilterModel.searchText = this.filterForm.value.Search;
      this.pagingFilterModel.currentPage = 1;
      this.getPatients();
    }
  }

  resetFilters() {
    this.filterForm.reset();
    this.filterForm.patchValue({ Type: '' });
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = [];
    this.pagingFilterModel.searchText = '';
    this.getPatients();
    this.getCounts();
  }

  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.getPatients();
  }
  openAppointmentModal(id: number) {
    this.appointmentService.getAppointmentById(id).subscribe({
      next: (data) => {
        this.selectedAppointment = data.results;
      },
      error: (err) => {
        console.error('Failed to fetch appointment', err);
        this.messageService.add({ severity: 'error', summary: 'فشل', detail: 'حدث خطأ أثناء جلب البيانات' });
      }
    });
  }
  getCounts() {
    this.appointmentService.getCounts(this.pagingFilterModel).subscribe({
      next: (data) => {
        this.patientServices = data.results.map((service: any) => {
          let imgPath: string;
          switch (service.name) {
            case 'كشف':
              imgPath = '../../../../../assets/vendors/imgs/blue.png';
              break;
            case 'استشارة':
              imgPath = '../../../../../assets/vendors/imgs/yellow.png';
              break;
            case 'عمليات':
              imgPath = '../../../../../assets/vendors/imgs/red.png';
              break;
            case 'تحاليل':
              imgPath = '../../../../../assets/vendors/imgs/redd.png';
              break;
            case 'أشعة':
              imgPath = '../../../../../assets/vendors/imgs/bluee.png';
              break;
            case 'طوارئ':
              imgPath = '../../../../../assets/vendors/imgs/reddd.png';
              break;
            default:
              imgPath = '';
          }
          return {
            ...service,
            img: imgPath
          };
        });

      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'فشل', detail: 'حدث خطأ أثناء جلب البيانات' });
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
  // 
  deleteAppointment(id: number) {
    this.appointmentService.deleteAppointment(id).subscribe({
      next: (data) => {
        this.getPatients();
        this.getCounts();
        this.openAppointmentModal(id);
        this.messageService.add({ severity: 'success', summary: 'تم الإلغاء', detail: 'تم الإلغاء بنجاح' });
        this.sharedService.closeModal('deleteAppointmentModal');
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'فشل', detail: 'حدث خطأ أثناء الإلغاء' });
      }
    });
  }
}
