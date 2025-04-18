import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AppointmentService } from '../../../../core/services/appointment.service';
import { StaffService } from '../../../../core/services/staff.service';
import { trigger, transition, style, animate } from '@angular/animations';

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
  reservationForm: FormGroup;
  // 
  clinics!:any;
  filteredClinics: any[] = [];
  // 
  doctors!:any;
  filteredDoctors: any[] = [];
  constructor(private fb: FormBuilder , private appointmentService: AppointmentService , private staffService: StaffService) {
    this.reservationForm = this.fb.group({
      patientName: ['', Validators.required],
      patientPhone: ['', Validators.required],
      appointmentType: ['', Validators.required],
      clinicId: [{ value: '', disabled: true }, Validators.required],
      doctorId: [{ value: '', disabled: true }, Validators.required],
      appointmentDate: ['', Validators.required],
      insuranceCompanyId: [null],
      insuranceCategoryId: [null],
      insuranceNumber: [''],
      referred: ['no'],
      referredClinic: [''],
      paymentMethod: ['', Validators.required],
    });
    this.reservationForm.get('appointmentType')?.valueChanges.subscribe((selectedType) => {
      this.filteredClinics = this.clinics.filter((clinic: any) => clinic.type === selectedType);
    
      const clinicControl = this.reservationForm.get('clinicId');
    
      if (selectedType) {
        clinicControl?.enable();
      } else {
        clinicControl?.disable();
      }
    });
    this.reservationForm.get('clinicId')?.valueChanges.subscribe((clinicId) => {
      const doctorControl = this.reservationForm.get('doctorId');
    
      if (clinicId) {
        this.filteredDoctors = this.doctors.filter((doc: any) => doc.clinicId === +clinicId);
        doctorControl?.enable();
      } else {
        doctorControl?.disable();
      }
      doctorControl?.reset();
    });
  }

  ngOnInit(): void {
    this.getClinics();    
    this.getStaff();
  }
  onReferredChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    if (value === 'no') {
      this.reservationForm.get('referredClinic')?.reset();
    }
  }

  onSubmit() {
    this.appointmentService.createAppointment(this.reservationForm.value).subscribe({
      next: (response) => {
        console.log('Appointment created successfully', response);
        this.reservationForm.reset();
      },
      error: (error) => {
        console.error('Error creating appointment', error);
        console.error('details:', this.reservationForm.value);
      }
    })
  }
  getClinics() {
    this.appointmentService.getClinics().subscribe({
      next: (data) => {
        this.clinics = data;
        this.filteredClinics = this.clinics;
        
        console.log(this.clinics);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
  getStaff(){
    this.staffService.getStaff().subscribe({
      next: (data) => {
        this.doctors = data.filter((staff: any) => staff.type === 'Doctor' && staff.status === 'Active');
        console.log(this.doctors);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
}
