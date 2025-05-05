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
    });
  
    this.reservationForm.get('medicalServiceId')?.valueChanges.subscribe((medicalServiceId) => {
      if (medicalServiceId) {
        this.filteredDoctors = this.doctors.filter(
          (doc: any) => doc.medicalServiceId === Number(medicalServiceId) || doc.medicalServiceId === null
        );
        console.log('Filtered Doctors:', this.filteredDoctors); // Debug
        this.reservationForm.get('doctorId')?.enable();
        this.reservationForm.get('doctorId')?.setValue(''); // Set to empty string
      } else {
        this.filteredDoctors = [];
        this.reservationForm.get('doctorId')?.disable();
        this.reservationForm.get('doctorId')?.setValue('');
      }
    
      const selectedService = this.filteredServices.find(
        (service: any) => service.serviceId === Number(medicalServiceId)
      );
      this.selectedServicePrice = selectedService ? selectedService.price : null;
      this.showServicePrice = !!selectedService;
    });
  
    this.reservationForm.get('insuranceCompanyId')?.valueChanges.subscribe((companyId) => {
      const selectedCompany = this.insuranceCompanies.find((company) => company.id === Number(companyId));
      this.insuranceCategories = selectedCompany?.insuranceCategories || [];
    });
  }

  ngOnInit(): void {
    this.getStaff();
    this.getInsuranceCompanies();
    this.loadPatients();
    this.getServices();
  }
  onAppointmentTypeChange() {
    this.showServicePrice = false;
    this.selectedServicePrice = null;
    const selectedType = this.reservationForm.get('appointmentType')?.value;
    this.reservationForm.get('medicalServiceId')?.setValue('', { emitEvent: false });
    this.selectedServicePrice = null;
  
    if (!this.services || this.services.length === 0) {
      this.filteredServices = [];
      return;
    }
  
    if (selectedType === 'General') {
      this.filteredServices = this.services.filter((service: any) => service.serviceType === 'General');
    } else if (selectedType === 'Consultation') {
      this.filteredServices = this.services.filter((service: any) => service.serviceType === 'Consultation');
    } else if (selectedType === 'Screening') {
      this.filteredServices = this.services.filter((service: any) => service.serviceType === 'Screening');
    } else if (selectedType === 'Radiology') {
      this.filteredServices = this.services.filter((service: any) => service.serviceType === 'Radiology');
    } else if (selectedType === 'Surgery') {
      this.filteredServices = this.services.filter((service: any) => service.serviceType === 'Surgery');
    } else {
      this.filteredServices = [];
    }
  }
  
  onServiceSelected() {
    const selectedServiceId = this.reservationForm.get('medicalServiceId')?.value;
    console.log('Selected Service ID:', selectedServiceId); // Debug
    console.log('Filtered Services:', this.filteredServices); // Debug
  
    if (selectedServiceId) {
      const selectedService = this.filteredServices.find(
        (service: any) => service.serviceId === Number(selectedServiceId)
      );
      this.filteredDoctorsByService = this.doctors.filter(
        (doctor) => doctor.medicalServiceId === Number(selectedServiceId)
      );
      console.log('Filtered Doctors By Service:', this.filteredDoctorsByService); // Debug
      this.selectedServicePrice = selectedService ? selectedService.price : null;
      this.showServicePrice = true;
    } else {
      this.filteredDoctorsByService = [];
      this.selectedServicePrice = null;
      this.showServicePrice = false;
    }
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
  
    console.log('Payload before sending:', payload);
  
    this.appointmentService.createAppointment(payload).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          console.log('Appointment created successfully', response);
          console.log('Appointment Data:', formData);
          this.messageService.add({ severity: 'success', summary: 'تم الحجز', detail: response.message });
          this.submittedData = { ...formData };
          this.printInvoiceData = this.createInvoiceObj(response.results);
          this.PrintInvoiceComponent.invoiceData = this.printInvoiceData;
          this.PrintInvoiceComponent.generatePdf();
          this.showReceipt = true;
          this.reservationForm.reset();
        } else {
          this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: response.message });
          console.log('Appointment Data:', formData);
          console.log('Response:', response);
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
        console.log('Doctors',this.doctors);
      },
      error: (err) => {
        console.log(err);
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
        // this.total = data.total;
        console.log('Patients : ',this.patients);
      },
      error: (err) => {
        console.log(err);
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
        console.log('Services', this.services);
      },
      error: (err) => {
        console.log(err);
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
        console.log(err);
      }
    });
  }

  createInvoiceObj(number: string): any {
    let obj = {
      appointmentNumber: number,
      patientName: this.reservationForm.value.patientName,
      patientPhone: this.reservationForm.value.patientPhone,
      appointmentType: VisitTypeLabels[this.reservationForm.value.appointmentType],
      clinicName: this.getClinicName(this.reservationForm.value.medicalServiceId),
      doctorName: this.getDoctorName(this.reservationForm.value.doctorId),
      appointmentDate: this.sharedService.getArabicDayAndTimeRange(this.reservationForm.value.appointmentDate),
      insuranceNumber: this.reservationForm.value.insuranceNumber,
      insuranceCompany: this.insuranceCompanies.find((i: any) => i.id == this.reservationForm.value.insuranceCompanyId)?.name,
      insuranceCategory: this.reservationForm.value.insuranceCategoryId,
    }

    return obj;
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
            console.log('Patient : ',patient);
            
          } else {
            this.messageService.add({
              severity: 'info',
              summary: 'لا يوجد مريض',
              detail: 'لم يتم العثور على مريض بهذا الرقم',
            });
          }
        },
        error: (err) => {
          console.log(err);
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
    console.log('Selected Doctor ID:', doctorId);
  }
}
