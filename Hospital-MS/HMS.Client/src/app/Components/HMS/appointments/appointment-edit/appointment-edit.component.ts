import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { ActivatedRoute } from '@angular/router';
import { trigger, transition, style, animate } from '@angular/animations';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-appointment-edit',
  templateUrl: './appointment-edit.component.html',
  styleUrl: './appointment-edit.component.css',
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
export class AppointmentEditComponent {
  appointmentId!:number;
  pagingFilterModel: PagingFilterModel = {
      searchText: '',
      currentPage: 1,
      pageSize: 100,
      filterList: []
    };
  reservationForm!:FormGroup;
  showServicePrice!:boolean;
  selectedServicePrice!:number;
  services!:any[];
  filteredServices!:any[];
  filteredDoctorsByService!:any[];
  doctors!:any[];
  filteredDoctors!:any[];
  insuranceCompanies!:any[];
  insuranceCategories!:any[];
  selectedAppointmentType!:string;
  constructor(private fb:FormBuilder , private appointmentService : AppointmentService , private staffService : StaffService , private insuranceService : InsuranceService , private activ : ActivatedRoute , private messageService : MessageService){
    this.activ.params.subscribe((params: any) => {
      this.appointmentId = params.id;
    });

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
    });


    this.getAppointmentById();



    this.reservationForm.get('appointmentType')?.valueChanges.subscribe((selectedType) => {
      this.onAppointmentTypeChange();
      // this.filteredClinics = this.clinics.filter((clinic: any) => clinic.type === selectedType);
      const clinicControl = this.reservationForm.get('medicalServiceId');
      if (selectedType) {
        clinicControl?.enable();
      } else {
        clinicControl?.disable();
      }
    });




  
  this.reservationForm.get('medicalServiceId')?.valueChanges.subscribe((medicalServiceId) => {
    if (medicalServiceId) {
      this.filteredDoctors = this.doctors.filter((doc: any) => 
        doc.medicalServiceId === +medicalServiceId || doc.medicalServiceId === null
      );
      this.reservationForm.get('doctorId')?.enable();
    } else {
      this.filteredDoctors = [];
      this.reservationForm.get('doctorId')?.disable();
    }
    this.reservationForm.get('doctorId')?.reset();
    
    const selectedService = this.filteredServices.find(
      (service: any) => service.serviceId == medicalServiceId
    );
    this.selectedServicePrice = selectedService ? selectedService.price : null;
    this.showServicePrice = !!selectedService;
  });





    this.reservationForm.get('insuranceCompanyId')?.valueChanges.subscribe(companyId => {
      const selectedCompany = this.insuranceCompanies.find(company => company.id === +companyId);
      this.insuranceCategories = selectedCompany?.insuranceCategories || [];
    });
  
  }
  ngOnInit(): void {
    this.getServices();
    this.getStaff();
    this.getInsuranceCompanies();
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
      this.filteredServices = this.services.filter(
        (service: any) => service.serviceType === 'General'
      );
    }
    else if (selectedType === 'Consultation') {
      this.filteredServices = this.services.filter(
        (service: any) => service.serviceType === 'Consultation'
      );
    }
    else if (selectedType === 'Screening') {
      this.filteredServices = this.services.filter(
        (service: any) => service.serviceType === 'Screening'
      );
    } else if (selectedType === 'Radiology') {
      this.filteredServices = this.services.filter(
        (service: any) => service.serviceType === 'Radiology'
      );
    } else {
      this.filteredServices = [];
    }
  }
  
  onServiceSelected() {
    const selectedServiceId = this.reservationForm.get('medicalServiceId')?.value;

    if (selectedServiceId) {
      const selectedService = this.filteredServices.find(
        (service: any) => service.serviceId == selectedServiceId
      );
      this.filteredDoctorsByService = this.doctors.filter(
        doctor => doctor.medicalServiceId === selectedServiceId
      );
      this.reservationForm.get('doctorId')?.reset();
      this.selectedServicePrice = selectedService ? selectedService.price : null;
      this.showServicePrice = true;
      
    } else {
      this.filteredDoctorsByService = [];
      this.selectedServicePrice = null;
      this.showServicePrice = false;
    }
  }

  getServices() {
    this.appointmentService.getServices(1, 100, '', {}).subscribe({
      next: (data) => {
        this.services = data.results || [];
        this.filteredServices = this.services;
      },
      error: (err) => {
        this.services = [];
        this.filteredServices = [];
      }
    });
  }

  getStaff() {
    this.staffService.getDoctors(this.pagingFilterModel).subscribe({
      next: (data) => {
        this.doctors = data.results;
        this.filteredDoctors = this.doctors
      },
      error: (err) => {
      }
    });
  }

  getInsuranceCompanies() {
    this.insuranceService.getAllInsurances().subscribe({
      next: (data) => {
        this.insuranceCompanies = data.results;
      },
      error: (err) => {
      }
    });
  }
  
  getAppointmentById() {
    this.appointmentService.getAppointmentById(this.appointmentId).subscribe((res: any) => {
      const appointmentData = res.results;
      if (!this.filteredServices) {
        this.filteredServices = [];
      }
      this.reservationForm.get('medicalServiceId')?.enable();
      this.reservationForm.get('doctorId')?.enable();
  
      const formData = {
        patientName: appointmentData.patientName,
        patientPhone: appointmentData.patientPhone,
        gender: appointmentData.gender === 'ذكر' ? 'Male' : 'Female',
        appointmentType: this.mapAppointmentType(appointmentData.type),
        medicalServiceId: appointmentData.medicalServiceId,
        doctorId: appointmentData.doctorId,
        appointmentDate: appointmentData.appointmentDate,
        insuranceCompanyId: appointmentData.insuranceCompanyId || null,
        insuranceCategoryId: appointmentData.insuranceCategoryId || null,
        insuranceNumber: appointmentData.insuranceNumber || '',
        referred: appointmentData.referred || 'no',
        referredClinic: appointmentData.referredClinic || '',
        paymentMethod: this.mapPaymentMethod(appointmentData.paymentMethod),
      };
      this.reservationForm.patchValue(formData, { emitEvent: false });
      this.selectedAppointmentType = formData.appointmentType;
      this.selectedServicePrice = appointmentData.medicalServicePrice;
      this.showServicePrice = !!appointmentData.medicalServicePrice;
  
    });
  }
  
  private mapAppointmentType(apiType: string): string {
    const typeMap: {[key: string]: string} = {
      'كشف': 'General',
      'استشارة': 'Consultation',
      'عمليات': 'Surgery',
      'تحاليل': 'Screening',
      'أشعة': 'Radiology',
    };
    return typeMap[apiType];
  }
  
  private mapPaymentMethod(apiPaymentMethod: string): string {
    const paymentMap: {[key: string]: string} = {
      'Cash': 'نقدي',
      'InstantTransfer': 'تحويل لحظي',
      'نقدي': 'نقدي',
      'تحويل لحظي': 'تحويل لحظي'
    };
    return paymentMap[apiPaymentMethod];
  }
  onSubmit(){
    const formData = this.reservationForm.value;
    this.appointmentService.editAppointment(this.appointmentId, formData).subscribe({
      next: (data) => {
        this.messageService.add({ severity: 'success', summary: 'تم تحديث الحجز', detail: 'تم تحديث الحجز بنجاح' });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'فشل تحديث الحجز', detail: 'حدث خطأ أثناء تحديث الحجز' });
      }
    });
  }
}
