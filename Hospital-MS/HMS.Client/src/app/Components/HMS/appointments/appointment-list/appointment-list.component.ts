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
declare var bootstrap: any;
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
    { name: 'أشعة', value: 'CTScan' },
    { name: 'طوارئ', value: 'Emergency' },
  ]
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName')
  get today(): string {
    const date = new Date();
    const dateStr = date.toLocaleDateString('ar-EG', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  
    const timeStr = date.toLocaleTimeString('ar-EG', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
  
    return `${dateStr} - الساعة ${timeStr}`;
  } 
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
    this.getAllShifts();
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

  // exportToPDF() {
  //   const element = document.getElementById('pdfContent');

  //   const opt = {
  //     margin: 0.5,
  //     filename: 'booking-details.pdf',
  //     image: { type: 'jpeg', quality: 1 },
  //     html2canvas: { scale: 2 },
  //     jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
  //   };

  //   html2pdf().set(opt).from(element).save();
  // }
  // ==================================================================
  exportToPDF() {
    const pdfDiv = document.getElementById('printablePDFContent');
  
    if (!pdfDiv) return;
  
    const opt = {
      margin: 0.5,
      filename: 'تفاصيل حجز رقم ' + this.selectedAppointment.id + '.pdf',
      image: { type: 'jpeg', quality: 1 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    const clonedDiv = pdfDiv.cloneNode(true) as HTMLElement;
    clonedDiv.style.display = 'block';
    document.body.appendChild(clonedDiv);
  
    html2pdf().set(opt).from(clonedDiv).save().then(() => {
      document.body.removeChild(clonedDiv);
    });
  }
  
  getPatients() {
    this.appointmentService.getAllAppointments(this.pagingFilterModel).subscribe({
      next: (data) => {
        debugger;
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
  totalPrice = 0;
  openAppointmentModal(id: number) {
    this.appointmentService.getAppointmentById(id).subscribe({
      next: (data) => {
        this.selectedAppointment = data.results;
        this.totalPrice = this.selectedAppointment.medicalServices.reduce((sum: number, item: any) => sum + item.medicalServicePrice, 0);
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
  // 
  medicalServices: any[];
  totalAmountForShift: any;
  closedBy: any;
  closedAt: any;
  shiftDay = new Date().toLocaleDateString('ar-EG', { weekday: 'long' });
  shifts!:any;
  getAllShifts(){
    this.appointmentService.getAllShifts().subscribe({
      next:(data:any)=>{
        this.shifts = data.results
        console.log('Shifts : ',this.shifts);
      }
    })
  }
  confirmShiftClose() {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد إغلاق حركة الشيفت؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، أغلق الشيفت',
      cancelButtonText: 'إلغاء',
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
    }).then(result => {
      if (result.isConfirmed) {
        this.appointmentService.closeShift().subscribe(res => {
          console.log(res);
          
          if (res?.isSuccess) {
            const data = res.results;
            this.medicalServices = data.medicalServices;
            this.totalAmountForShift = data.totalAmount;
            this.closedBy = data.closedBy;
            this.closedAt = data.closedAt;

            const modalElement = document.getElementById('shiftReportModal');
            const modal = new bootstrap.Modal(modalElement);
            modal.show();
          } else {
            Swal.fire('خطأ', res.message || 'حدث خطأ ما', 'error');
          }
        }, () => {
          Swal.fire('خطأ', 'فشل الاتصال بالسيرفر', 'error');
        });
      }
    });
  }
  printShiftReport() {
    const element = document.getElementById('printableShiftModal');
    const opt = {
      margin: 0.5,
      filename: `تقرير-إيرادات-الشيفت-${new Date().toLocaleDateString('ar-EG')}.pdf`,
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'cm', format: 'a4', orientation: 'portrait' }
    };
    html2pdf().set(opt).from(element).save();
  }
  // 
  backToMainModal(currentModalId: string, mainModalId: string = 'bookingDetailsModal') {
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
  // 
  selectedShiftId: number | null = null;
  openChooseShiftModal() {
    this.getAllShifts();
    const modalElement = document.getElementById('chooseShiftModal');
    const modal = new bootstrap.Modal(modalElement!);
    modal.show();
  }

  openShiftReport() {
    if (!this.selectedShiftId) {
      Swal.fire('تنبيه', 'من فضلك اختر شيفت أولاً', 'warning');
      return;
    }
  
    this.appointmentService.getShiftById(this.selectedShiftId).subscribe({
      next: (res: any) => {
        if (res?.isSuccess) {
          const data = res.results;
          this.medicalServices = data.medicalServices;
          this.totalAmountForShift = data.totalAmount;
          this.closedBy = data.closedBy;
          this.closedAt = data.closedAt;
          this.shiftDay = data.day;
          this.userName = data.closedBy;
          const chooseModal = bootstrap.Modal.getInstance(document.getElementById('chooseShiftModal')!);
          chooseModal?.hide();
          const reportModal = new bootstrap.Modal(document.getElementById('shiftReportModal')!);
          reportModal.show();
        } else {
          Swal.fire('خطأ', res.message || 'حدث خطأ ما', 'error');
        }
      },
      error: () => {
        Swal.fire('خطأ', 'فشل الاتصال بالسيرفر', 'error');
      }
    });
  }
  getDayName(dateString: string): string {
    const date = new Date(dateString);
    const days = ['الأحد','الإثنين','الثلاثاء','الأربعاء','الخميس','الجمعة','السبت'];
    return days[date.getDay()];
  }  
}
