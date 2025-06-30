import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { trigger, transition, style, animate } from '@angular/animations';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import { SharedService } from '../../../../Services/shared.service';
import { VisitTypeLabels } from '../../../../Models/HMS/enums';
import { PrintInvoiceComponent } from '../print-invoice/print-invoice.component';
import { MessageService } from 'primeng/api';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';

@Component({
  selector: 'app-appointment-form',
  templateUrl: './appointment-form.component.html',
  styleUrl: './appointment-form.component.css',
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
export class AppointmentFormComponent implements OnInit {
  @ViewChild('PrintInvioce', { static: false }) PrintInvoiceComponent: PrintInvoiceComponent;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 100,
    filterList: []
  };
  clinics: any[] = [];
  filteredClinics: any[] = [];
  doctors: any[] = [];
  filteredDoctors: any[] = [];
  services: any[] = [];
  filteredServices: any[] = [];
  reservationForm: FormGroup;
  // 
  insuranceCompanies!: any;
  departments!: any;
  patients!:any;
  // 
  // 
  radiologyTypes: string[] = [];
  // 
  showReceipt: boolean = false;
  submittedData: any = {};
  printInvoiceData: any = {};
  // 
  insuranceCategories!: any;
  // 
  selectedServicePrice!:number | null;
  showServicePrice:boolean = false;
  filteredDoctorsByService: any[] = [];
  selectedDate: Date | null = null;
  // 
  showAdditionalInfo:boolean = false;
  constructor(
    private fb: FormBuilder,
    private appointmentService: AppointmentService,
    private staffService: StaffService,
    private messageService: MessageService,
    private insuranceService: InsuranceService,
    private admissionService: AdmissionService,
    private sharedService: SharedService
  ) {
    this.reservationForm = this.fb.group({
      patientName: ['', Validators.required],
      patientPhone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      gender: ['', Validators.required],
      appointmentType: ['', Validators.required],
      medicalServiceId: [{ value: '', disabled: true }, Validators.required],
      doctorId: [{ value: '', disabled: true }],
      appointmentDate: ['', Validators.required],
      insuranceCompanyId: [null],
      insuranceCategoryId: [null],
      insuranceNumber: [''],
      referred: ['no'],
      referredClinic: [''],
      paymentMethod: ['نقدي', Validators.required],
      emergencyLevel: ['Normal'],
      companionName: [''],
      companionNationalId: [''],
      companionPhone: [''],
    });
  
    this.reservationForm.get('appointmentType')?.valueChanges.subscribe((selectedType) => {
      this.onAppointmentTypeChange();
      this.filteredClinics = this.clinics.filter((clinic: any) => clinic.type === selectedType);
      const clinicControl = this.reservationForm.get('medicalServiceId');
      if (selectedType) {
        clinicControl?.enable();
      } else {
        clinicControl?.disable();
      }
      this.selectedDate = this.reservationForm.get('appointmentDate')?.value ? new Date(this.reservationForm.get('appointmentDate')?.value) : null;
      this.filterServicesByDay();
      this.filterDoctorsByDay();
    });
    
  
    this.reservationForm.get('medicalServiceId')?.valueChanges.subscribe((medicalServiceId) => {
      if (medicalServiceId) {
        this.filteredDoctors = this.doctors.filter((doc: any) =>
          doc.medicalServices?.some((service: any) => service.id === Number(medicalServiceId))
        );
        this.reservationForm.get('doctorId')?.enable();
        this.reservationForm.get('doctorId')?.setValue('');
      } else {
        this.filteredDoctors = [];
        this.reservationForm.get('doctorId')?.disable();
        this.reservationForm.get('doctorId')?.setValue('');
      }
    
      const selectedService = this.filteredServices.find(
        (service: any) => service.id === Number(medicalServiceId) // Adjust to service.id if serviceId is not used
      );
      this.selectedServicePrice = selectedService ? selectedService.price : null;
      this.showServicePrice = !!selectedService;
    });
  
    this.reservationForm.get('insuranceCompanyId')?.valueChanges.subscribe((companyId) => {
      const selectedCompany = this.insuranceCompanies.find((company) => company.id === Number(companyId));
      this.insuranceCategories = selectedCompany?.insuranceCategories || [];
    });
  }
  filterServicesByDay() {
    if (!this.selectedDate || !this.services.length) {
      return;
    }
  
    const dayOfWeek = this.getEnglishDayOfWeek(this.selectedDate);
    
    this.filteredServices = this.filteredServices.filter(service => {
      if (!service.medicalServiceSchedules || !service.medicalServiceSchedules.length) {
        return false;
      }
      return service.medicalServiceSchedules.some((schedule: any) => {
        return schedule.weekDay === dayOfWeek;
      });
    });
  }
  
  filterDoctorsByDay() {
    if (!this.selectedDate || !this.doctors.length) {
      this.filteredDoctors = [];
      this.filteredDoctorsByService = [];
      return;
    }
  
    const dayOfWeek = this.getEnglishDayOfWeek(this.selectedDate);
    
    this.filteredDoctors = this.filteredDoctors.filter(doctor => {
      if (!doctor.doctorSchedules || !doctor.doctorSchedules.length) {
        return false;
      }
      return doctor.doctorSchedules.some((schedule: any) => {
        return schedule.weekDay === dayOfWeek;
      });
    });
    this.filteredDoctorsByService = [...this.filteredDoctors];
  }

  getArabicDayOfWeek(date: Date): string {
    const days = ['الأحد', 'الإثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'];
    return days[date.getDay()];
  }
  
  ngOnInit(): void {
    this.getStaff();
    this.getInsuranceCompanies();
    this.loadPatients();
    this.getServices();
  }
  onDayChange() {
  this.selectedDate = this.reservationForm.get('appointmentDate')?.value
    ? new Date(this.reservationForm.get('appointmentDate')?.value)
    : null;
  this.filterServicesByDay();
  this.filterDoctorsByDay();
}
  onAppointmentTypeChange(): void {
    this.filterServices();
    this.reservationForm.get('medicalServiceId')?.setValue('');
  }
  
  onServiceSelected(): void {
    this.filterDoctors();
    const selectedService = this.filteredServices.find(
      service => service.id === Number(this.reservationForm.get('medicalServiceId')?.value)
    );
    this.selectedServicePrice = selectedService?.price || null;
    this.showServicePrice = !!selectedService;
  }

  onSubmit() {
    if (this.reservationForm.invalid) {
      this.messageService.add({ severity: 'warn', summary: 'بيانات غير مكتملة', detail: 'يرجى ملء جميع الحقول المطلوبة' });
      return;
    }
  
    const formData = this.reservationForm.value;
  
    const payload = {
      patientName: formData.patientName,
      patientPhone: formData.patientPhone,
      gender: formData.gender,
      appointmentType: formData.appointmentType,
      medicalServiceId: Number(formData.medicalServiceId),
      doctorId: formData.doctorId ? Number(formData.doctorId) : null,
      appointmentDate: formData.appointmentDate,
      insuranceCompanyId: formData.insuranceCompanyId || null,
      insuranceCategoryId: formData.insuranceCategoryId || null,
      insuranceNumber: formData.insuranceNumber || '',
      paymentMethod: formData.paymentMethod,
      emergencyLevel: formData.emergencyLevel,
      companionName: formData.companionName,
      companionNationalId: formData.companionNationalId,
      companionPhone: formData.companionPhone
    };
  
  
    this.appointmentService.createAppointment(payload).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'تم الحجز', detail: response.message });
          this.submittedData = { ...formData };
          this.printInvoiceData = this.createInvoiceObj(response.results);
          this.PrintInvoiceComponent.invoiceData = this.printInvoiceData;
          this.PrintInvoiceComponent.generatePdf();
          this.showReceipt = true;
          this.reservationForm.reset();
        } else {
          this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: response.message });
        }
      },
      error: (error) => {
        console.error('Error creating appointment:', error);
        console.error('Details:', formData);
        const errorMessage = error.error?.message || 'حدث خطأ أثناء إنشاء الحجز';
        this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: errorMessage });
      }
    });
  }

  getStaff() {
    this.staffService.getDoctors(this.pagingFilterModel).subscribe({
      next: (data) => {
        this.doctors = data.results;
      },
      error: (err) => {
      }
    });
  }
  // 
  loadPatients() {
    this.admissionService.getAddmision(this.pagingFilterModel).subscribe({
      next: (data) => {
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
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'فشل التحميل',
          detail: 'حدث خطأ أثناء تحميل البيانات',
        });
      },
    });
  }
  getServices() {
    const filterParams = {
    };
  
    this.appointmentService.getServices(1, 100, '', filterParams).subscribe({
      next: (data) => {
        this.services = data.results || [];
      },
      error: (err) => {
        this.services = [];
      }
    });
  }
  // 
  get selectedAppointmentType() {
    return this.reservationForm.get('appointmentType')?.value;
  }
  // 
  getClinicName(medicalServiceId: string): string {
    const clinic = this.clinics.find((c: any) => c.id === +medicalServiceId);
    return clinic ? clinic.name : 'غير محدد';
  }

  getDoctorName(doctorId: string): string {
    const doctor = this.doctors.find((d: any) => d.id === +doctorId);
    return doctor ? doctor.fullName : 'غير محدد';
  }
  printReceipt() {
    window.print();
  }

  closeReceipt() {
    this.showReceipt = false;
    this.submittedData = {};
  }
  // 
  getInsuranceCompanies() {
    this.insuranceService.getAllInsurances().subscribe({
      next: (data) => {
        this.insuranceCompanies = data.results;
      },
      error: (err) => {
      }
    });
  }

  createInvoiceObj(apiData: any): any {
    const formData = this.submittedData;
    
    return {
      appointmentNumber: apiData.appointmentNumber,
      patientName: formData.patientName,
      patientPhone: formData.patientPhone,
      // appointmentType: formData.appointmentType,
      medicalServiceName: apiData.medicalServiceName,
      doctorName: this.getDoctorName(this.reservationForm.value.doctorId),
      selectedServicePrice: formData.selectedServicePrice || 0,
      appointmentDate: this.sharedService.getArabicDayAndTimeRange(this.reservationForm.value.appointmentDate),
      insuranceCompany: formData.insuranceCompany,
      insuranceNumber: formData.insuranceNumber,
      insuranceCategory: formData.insuranceCategory,
      hospitalPhone: '01000201499',
      hospitalEmail: 'info@elnourelmohamady.com',
    };
  }
  // 
  searchPatientByPhone(event: Event) {
    const input = event.target as HTMLInputElement;
    const phoneNumber = input.value.trim();
    if (phoneNumber.length === 11 && /^01[0125][0-9]{8}$/.test(phoneNumber)) {
      this.pagingFilterModel.searchText = phoneNumber;
      
      this.admissionService.getAddmision(this.pagingFilterModel).subscribe({
        next: (data) => {
          if (data.results && data.results.length > 0) {
            const patient = data.results[0];
            
            this.reservationForm.patchValue({
              patientName: patient.patientName,
              patientPhone: patient.phone
            });
            this.reservationForm.get('patientName')?.disable();
            this.reservationForm.get('patientPhone')?.disable();
            
            this.messageService.add({
              severity: 'success',
              summary: 'تم العثور على المريض',
              detail: 'تم تسجيل بيانات المريض تلقائياً',
            });
            
          } else {
            this.messageService.add({
              severity: 'info',
              summary: 'لا يوجد مريض',
              detail: 'لم يتم العثور على مريض بهذا الرقم',
            });
          }
        },
        error: (err) => {
          this.messageService.add({
            severity: 'error',
            summary: 'خطأ في البحث',
            detail: 'حدث خطأ أثناء البحث عن المريض',
          });
        }
      });
    }
  }
  // 
  onDoctorSelected() {
    const doctorId = this.reservationForm.get('doctorId')?.value;
  }
  // 
  private filterDoctors(): void {
    const selectedServiceId = this.reservationForm.get('medicalServiceId')?.value;
    
    if (!selectedServiceId) {
      this.filteredDoctorsByService = [];
      return;
    }

    let doctors = this.doctors.filter(doctor => 
      doctor.medicalServiceId === Number(selectedServiceId)
    );

    if (this.selectedDate) {
      const dayOfWeek = this.getEnglishDayOfWeek(this.selectedDate);
      doctors = doctors.filter(doctor => {
        if (!doctor.doctorSchedules || doctor.doctorSchedules.length === 0) {
          return true;
        }
        return doctor.doctorSchedules.some(schedule => 
          schedule.weekDay === dayOfWeek
        );
      });
    }

    this.filteredDoctorsByService = doctors;
  }
  private getEnglishDayOfWeek(date: Date): string {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return days[date.getDay()];
  }

  private filterServices(): void {
    const selectedType = this.reservationForm.get('appointmentType')?.value;
    
    if (!selectedType) {
      this.filteredServices = [];
      return;
    }
    let services = this.services.filter(service => service.type === selectedType);

    if (this.selectedDate) {
      const dayOfWeek = this.getEnglishDayOfWeek(this.selectedDate);
      services = services.filter(service => {
        if (!service.medicalServiceSchedules || service.medicalServiceSchedules.length === 0) {
          return true;
        }
        return service.medicalServiceSchedules.some(schedule => 
          schedule.weekDay === dayOfWeek
        );
      });
    }

    this.filteredServices = services;
  }

}
