import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { trigger, transition, style, animate } from '@angular/animations';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import { SharedService } from '../../../../Services/shared.service';
import { PrintInvoiceComponent } from '../print-invoice/print-invoice.component';
import { MessageService } from 'primeng/api';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { todayDateValidator } from '../../../../validators/today-date.validator';
declare var bootstrap: any;
import html2pdf from 'html2pdf.js';

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
  // @ViewChild('PrintInvioce', { static: false }) PrintInvoiceComponent: PrintInvoiceComponent;
  userName: any;
    currentDate = new Date();
    @ViewChild('printSection', { static: false }) printSectionRef: ElementRef;
    invoiceData: any;
  // private router = inject(Router);
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 100,
    filterList: []
  };
  appointmentDetailsSelected: any[] = [];
  clinics: any[] = [];
  filteredClinics: any[] = [];
  doctors: any[] = [];
  filteredDoctors: any[] = [];
  services: any[] = [];
  filteredServices: any[] = [];
  reservationForm: FormGroup;
  appointmentDetailsForm: FormGroup;
  // 
  insuranceCompanies!: any;
  departments!: any;
  patients!: any;
  // 
  // 
  radiologyTypes: string[] = [];
  // 
  showReceipt: boolean = false;
  submittedData: any = {};
  printInvoiceData: any = {};
  // 
  insuranceCategories!: any;
  appointmentType = '';
  // 
  selectedServicePrice!: number | null;
  showServicePrice: boolean = false;
  filteredDoctorsByService: any[] = [];
  radiologyTypeList: any[] = [];
  radiologyTypesSelected: any[] = [];
  selectedDate: Date | null = null;
  SelectedService: any;
  IsScreening: boolean = false;
  IsRadiology: boolean = false;
  IsGeneral: boolean = false;
  // 
  showAdditionalInfo: boolean = false;
  totalPrice = 0;
  constructor(
    private fb: FormBuilder,
    private appointmentService: AppointmentService,
    private staffService: StaffService,
    private messageService: MessageService,
    private insuranceService: InsuranceService,
    private admissionService: AdmissionService,
    private sharedService: SharedService,
    private router: Router,
    ) {
      this.userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName');    
    this.reservationForm = this.fb.group({
      patientName: ['', Validators.required],
      patientPhone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      gender: ['', Validators.required],
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
    this.appointmentDetailsForm = this.fb.group({
      id: null,
      appointmentDate: [new Date().toISOString().substring(0, 10), []],
      appointmentType: ['', Validators.required],
      medicalServiceId: ['', Validators.required],
      medicalServiceName: null,
      doctorId: [''],
      price: null,
      doctorName: null,
    });

    this.appointmentDetailsForm.get('appointmentType')?.valueChanges.subscribe((selectedType) => {
      this.IsScreening = selectedType === 'Screening';
      this.IsRadiology = selectedType === 'MRI' || selectedType === 'Panorama' || selectedType === 'XRay'
        || selectedType === 'CTScan' || selectedType === 'Ultrasound' || selectedType === 'Echo' || selectedType === 'Mammogram';
      this.IsGeneral = selectedType === 'General' || selectedType === 'Consultation' || selectedType === 'Surgery';
      this.appointmentType = selectedType;
      this.onAppointmentTypeChange();
      this.filteredClinics = this.clinics.filter((clinic: any) => clinic.type === selectedType);
      const clinicControl = this.reservationForm.get('medicalServiceId');
      if (selectedType) {
        clinicControl?.enable();
      } else {
        clinicControl?.disable();
      }
      this.selectedDate = this.appointmentDetailsForm.get('appointmentDate')?.value ? new Date(this.appointmentDetailsForm.get('appointmentDate')?.value) : null;
      this.filterServicesByDay();
      this.filterDoctorsByDay();
    });


    this.appointmentDetailsForm.get('medicalServiceId')?.valueChanges.subscribe((medicalServiceId) => {
      if (medicalServiceId) {
        this.filteredDoctors = this.doctors.filter((doc: any) =>
          doc.medicalServices?.some((service: any) => service.id === Number(medicalServiceId))
        );
        this.appointmentDetailsForm.get('doctorId')?.enable();
        this.appointmentDetailsForm.get('doctorId')?.setValue(null);
      } else {
        this.filteredDoctors = [];
        this.appointmentDetailsForm.get('doctorId')?.disable();
        this.appointmentDetailsForm.get('doctorId')?.setValue(null);
      }

      const selectedService = (this.filteredServices || []).find(
        (service: any) => service.id === Number(medicalServiceId)
      );
      console.log(selectedService);
      this.SelectedService = selectedService;
      this.selectedServicePrice = selectedService ? selectedService.price : null;
      this.showServicePrice = !!selectedService;

      if ((this.IsScreening || this.IsRadiology) && medicalServiceId) {
        let checked = this.radiologyTypesSelected.find(i => i.id == medicalServiceId);
        if (!checked) {
          this.radiologyTypesSelected.push({ id: selectedService.id, name: selectedService.name });
        }
      }
      const doctorControl = this.appointmentDetailsForm.get('doctorId');
      if (this.IsGeneral && medicalServiceId) {
        doctorControl?.setValidators(Validators.required);
      } else {
        doctorControl?.clearValidators();
        doctorControl?.setValue(null);
      }
      doctorControl?.updateValueAndValidity();
    });

    this.reservationForm.get('insuranceCompanyId')?.valueChanges.subscribe((companyId) => {
      const selectedCompany = this.insuranceCompanies.find((company) => company.id === Number(companyId));
      this.insuranceCategories = selectedCompany?.insuranceCategories || [];
    });
  }

  openAppointmentDetailsModal(item: any) {
    this.appointmentDetailsForm.reset();
    const modal = new bootstrap.Modal(document.getElementById('AppointmentDetailsModal')!);
    modal.show();
    if (item)
      this.formInit(item);
  }

  formInit(item: any) {
    this.appointmentDetailsForm.patchValue({
      id: item.id,
      appointmentDate: item.appointmentDate,
      appointmentType: item.appointmentType,
      medicalServiceId: item.medicalServiceId,
      medicalServiceName: item.medicalServiceName,
      doctorId: item.doctorId,
      price: item.price,
      doctorName: item.doctorName
    });
  }

  AddAppointmentDetails() {
    const validService = ['General', 'Consultation', 'Surgery'];
    const formData = this.appointmentDetailsForm.value;

    const isDuplicate = this.appointmentDetailsSelected.some(item =>
      item.appointmentType === formData.appointmentType &&
      item.medicalServiceId === formData.medicalServiceId &&
      item.id !== formData.id
    );

    if (isDuplicate) {
      this.messageService.add({
        severity: 'error',
        summary: 'مكرر',
        detail: 'تمت إضافة هذه الخدمة من قبل.'
      });
      return;
    }

    if (validService.includes(formData.appointmentType)) {
      const hasDifferentType = this.appointmentDetailsSelected.some(item =>
        validService.includes(item.appointmentType) &&
        item.appointmentType !== formData.appointmentType
      );

      if (hasDifferentType) {
        this.messageService.add({
          severity: 'warn',
          summary: 'نوع غير مسموح',
          detail: 'لا يمكن الجمع بين الاستشارة، الكشف، والعمليات في نفس الحجز.'
        });
        return;
      }
    }

    if (this.IsScreening || this.IsRadiology) {
      this.radiologyTypesSelected.forEach(item => {
        let service = this.filteredServices.find(service => service.id === item.id);
        let obj = {
          id: this.appointmentDetailsSelected.length + 1,
          appointmentDate: formData.appointmentDate,
          appointmentType: formData.appointmentType,
          medicalServiceId: item.id,
          medicalServiceName: item.name,
          price: service.price || 0,
          doctorId: null,
          doctorName: null,
        }

        this.appointmentDetailsSelected.push(obj);
      });
    } else {
      formData.price = this.SelectedService.price || 0;
      formData.medicalServiceName = this.SelectedService.name || '';
      formData.doctorName = this.getDoctorName(formData.doctorId);

      if (formData.id) {
        const index = this.appointmentDetailsSelected.findIndex(item => item.id === formData.id);
        if (index !== -1) {
          this.appointmentDetailsSelected[index] = formData;
        }
      } else {
        formData.id = this.appointmentDetailsSelected.length + 1;
        this.appointmentDetailsSelected.push(formData);
      }
    }

    this.totalPrice = this.appointmentDetailsSelected.reduce((sum, item) => sum + (item.price || 0), 0);
    this.appointmentDetailsForm.reset();
    this.radiologyTypesSelected = [];
    const modal = bootstrap.Modal.getInstance(document.getElementById('AppointmentDetailsModal')!);
    modal.hide();
  }

  deleteAppointment(index: number) {
    this.appointmentDetailsSelected.splice(index, 1);
    this.totalPrice = this.appointmentDetailsSelected.reduce((sum, item) => sum + (item.price || 0), 0);
  }

  filterServicesByDay() {
    if (!this.services || this.services.length === 0) {
      this.filteredServices = [];
      return;
    }
  
    const dayOfWeek = this.selectedDate
      ? new Date(this.selectedDate).toLocaleDateString('en-US', { weekday: 'long' })
      : null;
  
    const formType = this.appointmentDetailsForm.get('appointmentType')?.value;
  
    this.filteredServices = this.services.filter(service => {
      const matchesType = formType
        ? service.type?.toLowerCase() === formType.toLowerCase()
        : true;
  
      const matchesDay = dayOfWeek
        ? (service.medicalServiceSchedules?.length > 0 &&
           service.medicalServiceSchedules.some((s: any) =>
             s.weekDay.toLowerCase() === dayOfWeek.toLowerCase()
           ))
        : true;
  
      return matchesType && matchesDay;
    });
  
    console.log("Filtered Services (final):", this.filteredServices);
  }
  

  filterDoctorsByDay() {
    if (!this.selectedDate || !this.doctors.length) {
      this.filteredDoctors = [];
      return;
    }
  
    const dayOfWeek = this.getEnglishDayOfWeek(this.selectedDate);
  
    this.filteredDoctors = this.doctors.filter(doctor => {
      if (!doctor.doctorSchedules || !doctor.doctorSchedules.length) {
        return false;
      }
      return doctor.doctorSchedules.some((schedule: any) =>
        schedule.weekDay.toLowerCase() === dayOfWeek.toLowerCase()
      );
    });
  
    console.log("Filtered Doctors:", this.filteredDoctors);
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
    const navState: any = history.state;
    if (navState?.patientData) {
      this.reservationForm.patchValue(navState.patientData);
      console.log('PatientData from state:', navState.patientData);
    }else{
      console.log('No patient data found in state');
    }
  }

  onDayChange() {
    this.selectedDate = this.reservationForm.get('appointmentDate')?.value
      ? new Date(this.reservationForm.get('appointmentDate')?.value)
      : null;
    this.filterServicesByDay();
    this.filterDoctorsByDay();
  }

  onAppointmentTypeChange(): void {
    // this.filterServices();
    // this.appointmentDetailsForm.get('medicalServiceId')?.setValue(null);
    this.filterDoctorsByDay();
    this.filterServicesByDay();
  }

  onServiceSelected() {
    const selectedId = Number(this.appointmentDetailsForm.get('medicalServiceId')?.value);
  
    const selectedService = this.filteredServices.find(
      (s: any) => s.id === selectedId
    );
  
    this.SelectedService = selectedService;
    console.log("Selected Service:", selectedService);
  }
  

  // onSubmit() {
  //   if (this.reservationForm.invalid) {
  //     this.messageService.add({ severity: 'warn', summary: 'بيانات غير مكتملة', detail: 'يرجى ملء جميع الحقول المطلوبة' });
  //     return;
  //   }

  //   if (this.appointmentDetailsSelected.length === 0) {
  //     this.messageService.add({ severity: 'warn', summary: 'لا توجد خدمات', detail: 'يرجى إضافة خدمة طبية واحدة على الأقل' });
  //     return;
  //   }

  //   let validService = ['General', 'Consultation', 'Surgery'];
  //   let service = this.appointmentDetailsSelected.find(i => validService.includes(i.appointmentType));
  //   const formData = this.reservationForm.value;
  //   let medicalServices = [];
  //   let appointmentTypes = [... new Set(this.appointmentDetailsSelected.map(item => item.appointmentType))];
  //   appointmentTypes.forEach(type => {
  //     let types = this.appointmentDetailsSelected.filter(item => item.appointmentType === type);
  //     let obj = {
  //       medicalServiceIds: types.map(item => item.medicalServiceId),
  //       appointmentDate: types[0].appointmentDate,
  //       appointmentType: types[0].appointmentType,
  //     }

  //     medicalServices.push(obj);
  //   })
  //   const payload = {
  //     patientName: formData.patientName,
  //     patientPhone: formData.patientPhone,
  //     gender: formData.gender,
  //     appointmentType: service?.appointmentType,
  //     doctorId: service?.doctorId ? Number(service?.doctorId) : null,
  //     insuranceCompanyId: formData.insuranceCompanyId || null,
  //     insuranceCategoryId: formData.insuranceCategoryId || null,
  //     insuranceNumber: formData.insuranceNumber || '',
  //     paymentMethod: formData.paymentMethod,
  //     emergencyLevel: formData.emergencyLevel,
  //     companionName: formData.companionName,
  //     companionNationalId: formData.companionNationalId,
  //     companionPhone: formData.companionPhone,
  //     medicalServices: medicalServices
  //   };

  //   this.appointmentService.createAppointment(payload).subscribe({
  //     next: (response) => {
  //       console.log(response);
  //       console.log('Payload',payload);
  //       console.log('Appointment Details Form',this.appointmentDetailsForm.value);
  //       console.log('Reservation Form',this.reservationForm.value);
  //       if (response.isSuccess) {
  //         this.messageService.add({ severity: 'success', summary: 'تم الحجز', detail: response.message });
  //         this.submittedData = { ...formData };
  //         this.printInvoiceData = this.createInvoiceObj(response.results);
  //         this.PrintInvoiceComponent.invoiceData = this.printInvoiceData;
  //         this.PrintInvoiceComponent.generatePdf();
  //         this.showReceipt = true;
  //         this.reservationForm.reset();
  //       } else {
  //         this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: response.message });
  //       }
  //     },
  //     error: (error) => {
  //       console.error('Error creating appointment:', error);
  //       console.error('Details:', formData);
  //       console.log('Payload',payload);
  //       console.log('Appointment Details Form',this.appointmentDetailsForm.value);
  //       console.log('Reservation Form',this.reservationForm.value);
  //       const errorMessage = error.error?.message || 'حدث خطأ أثناء إنشاء الحجز';
  //       this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: errorMessage });
  //     }
  //   });
  // }

  getStaff() {
    this.staffService.getDoctors(this.pagingFilterModel).subscribe({
      next: (data) => {
        this.doctors = data.results;
        console.log("Doctors: " , this.doctors);
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
        console.log('Services: ',this.services);
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
    return doctor ? doctor.fullName : '---';
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


  onSubmit() {
    if (this.reservationForm.invalid) {
      this.messageService.add({ severity: 'warn', summary: 'بيانات غير مكتملة', detail: 'يرجى ملء جميع الحقول المطلوبة' });
      return;
    }
  
    if (this.appointmentDetailsSelected.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'لا توجد خدمات', detail: 'يرجى إضافة خدمة طبية واحدة على الأقل' });
      return;
    }
  
    let validService = ['General', 'Consultation', 'Surgery'];
    let service = this.appointmentDetailsSelected.find(i => validService.includes(i.appointmentType));
    const formData = this.reservationForm.value;
  
    let medicalServices = [];
    let appointmentTypes = [... new Set(this.appointmentDetailsSelected.map(item => item.appointmentType))];
    appointmentTypes.forEach(type => {
      let types = this.appointmentDetailsSelected.filter(item => item.appointmentType === type);
      let obj = {
        medicalServiceIds: types.map(item => item.medicalServiceId),
        appointmentDate: types[0].appointmentDate,
        appointmentType: types[0].appointmentType,
      }
      medicalServices.push(obj);
    });
  
    const payload = {
      patientName: formData.patientName,
      patientPhone: formData.patientPhone,
      gender: formData.gender,
      appointmentType: service?.appointmentType,
      doctorId: service?.doctorId ? Number(service?.doctorId) : null,
      insuranceCompanyId: formData.insuranceCompanyId || null,
      insuranceCategoryId: formData.insuranceCategoryId || null,
      insuranceNumber: formData.insuranceNumber || '',
      paymentMethod: formData.paymentMethod,
      emergencyLevel: formData.emergencyLevel,
      companionName: formData.companionName,
      companionNationalId: formData.companionNationalId,
      companionPhone: formData.companionPhone,
      medicalServices: medicalServices
    };
  
    this.appointmentService.createAppointment(payload).subscribe({
      next: (response) => {
        console.log('Response : ', response);
      
        if (response.isSuccess) {
          this.messageService.add({ severity: 'success', summary: 'تم الحجز', detail: response.message });
          this.invoiceData = response.results;
          this.generatePdf();
          this.resetForms();
        } else {
          this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: response.message });
        }
      },error: (error) => {
        const errorMessage = error.error?.message || 'حدث خطأ أثناء إنشاء الحجز';
        this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: errorMessage });
      }
    });
  }
  // To-Do
  createInvoiceObj(apiData: any): any[] {
    if (!apiData) return [];
  
    const invoices = apiData.medicalServices.map((service: any) => {
      return {
        appointmentNumber: apiData.appointmentNumber || '--',
        patientName: apiData.patientName || '',
        patientPhone: apiData.patientPhone || '',
        medicalServiceName: service.name,
        doctorName: apiData.doctorName || '',
        selectedServicePrice: service.price || 0,
        appointmentDate: apiData.appointmentDate
          ? this.sharedService.getArabicDayAndTimeRange(apiData.appointmentDate)
          : '',
        insuranceCompany: apiData.insuranceCompany || '',
        insuranceNumber: apiData.insuranceNumber || '',
        insuranceCategory: apiData.insuranceCategory || '',
        hospitalPhone: '01000201499',
        hospitalEmail: 'info@elnourelmohamady.com',
      };
    });
  
    return invoices;
  }
  
  generatePdf() {
    setTimeout(async () => {
      for (let i = 0; i < this.invoiceData.length; i++) {
        const element = document.getElementById('printSection-' + i);
        if (element) {
          await html2pdf().from(element).save(`invoice-${i + 1}.pdf`);
        }
      }
    }, 500);
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
  getEnglishDayOfWeek(date: Date): string {
    const days = [
      'Sunday', 'Monday', 'Tuesday', 'Wednesday',
      'Thursday', 'Friday', 'Saturday'
    ];
    return days[new Date(date).getDay()];
  }
  

  // private filterServices(): void {
  //   const selectedType = this.appointmentDetailsForm.get('appointmentType')?.value;

  //   if (!selectedType) {
  //     this.filteredServices = [];
  //     return;
  //   }
  //   let services = this.services.filter(service => service.type === selectedType);

  //   if (this.selectedDate) {

  //     const dayOfWeek = this.getArabicDayOfWeek(this.selectedDate);
  //     var servicesFiltered = services.filter(service => {
  //       if (!service.medicalServiceSchedules || service.medicalServiceSchedules.length === 0) {
  //         return true;
  //       }
  //       return service.medicalServiceSchedules.some(schedule =>
  //         schedule.weekDay === dayOfWeek
  //       );
  //     });
  //     if (servicesFiltered.length == 0) {
  //       const dayOfWeek = this.getEnglishDayOfWeek(this.selectedDate);
  //       var servicesFiltered = services.filter(service => {
  //         if (!service.medicalServiceSchedules || service.medicalServiceSchedules.length === 0) {
  //           return true;
  //         }
  //         return service.medicalServiceSchedules.some(schedule =>
  //           schedule.weekDay === dayOfWeek
  //         );
  //       });
  //     }
  //   }

  //   this.filteredServices = servicesFiltered;
  // }

  removeRadiologyType(index: number) {
    this.radiologyTypesSelected.splice(index, 1);
  }
  
  resetForms() {
  this.reservationForm = this.fb.group({
    patientName: ['', Validators.required],
    patientPhone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
    gender: ['', Validators.required],
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

  this.appointmentDetailsForm = this.fb.group({
    id: null,
    appointmentDate: [new Date().toISOString().substring(0, 10), []],
    appointmentType: ['', Validators.required],
    medicalServiceId: ['', Validators.required],
    medicalServiceName: null,
    doctorId: [''],
    price: null,
    doctorName: null,
  });
  this.appointmentDetailsSelected = [];
  }
}
