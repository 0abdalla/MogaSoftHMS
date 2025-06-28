import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { trigger, transition, style, animate } from '@angular/animations';
import { MessageService } from 'primeng/api';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { Patients } from '../../../../Models/HMS/patient';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { SharedService } from '../../../../Services/shared.service';
declare var html2pdf: any;
import Swal from 'sweetalert2';
import { Modal } from 'bootstrap';
import { FormDropdownModel } from '../../../../Models/Generics/FormDropdownModel';

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
 TitleList = ['المواعيد والحجز']; 
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  isFilter = true;
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
  AppointmentTypes: FormDropdownModel[] = [
    { name: 'كشف', value: 'General' },
    { name: 'استشارة', value: 'Consultation' },
    { name: 'عمليات', value: 'Surgery' },
    { name: 'تحاليل', value: 'Screening' },
    { name: 'اشعه', value: 'Radiology' },
    { name: 'طوارئ', value: 'Emergency' },
  ]
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

   filterChecked(filters: FilterModel[]) {
      this.pagingFilterModel.currentPage = 1;
      this.pagingFilterModel.filterList = filters;
      if (filters.some(i => i.categoryName == 'SearchText'))
        this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
      else
        this.pagingFilterModel.searchText = '';
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

 onPageChange(page: any) {
    this.pagingFilterModel.currentPage = page.page;
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
  // 
  displayCashMovement = false;
  cashMovementData: any[] = [];
  ashMovementData = [
    { day: '2025-06-18', count: 10, total: 1500 },
    { day: '2025-06-17', count: 8, total: 1200 },
    // ...
  ];

  clinicCount = 5;
  clinicTotal = 700;

  totalCount = this.cashMovementData.reduce((sum, i) => sum + i.count, 0) + this.clinicCount;
  totalAmount = this.cashMovementData.reduce((sum, i) => sum + i.total, 0) + this.clinicTotal;
  closeShift() {
    Swal.fire({
      title: 'هل أنت متأكد أنك تريد إغلاق الشيفت؟',
      text: 'لن يمكنك إضافة حجوزات أخرى',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، أغلق الشيفت',
      cancelButtonText: 'لا، إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.appointmentService.closeShift().subscribe({
          next: () => {
            this.messageService.add({ severity: 'success', summary: 'تم الإغلاق', detail: 'تم إغلاق الشيفت بنجاح' });
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'فشل', detail: 'حدث خطأ أثناء إغلاق الشيفت' });
          }
        });
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        const modalElement = document.getElementById('cashMovementModal');
        if (modalElement) {
          const modal = new Modal(modalElement);
          modal.show();
        }
      }
    });
  }
  showCashMovementModal() {
    this.displayCashMovement = true;
  }

}
