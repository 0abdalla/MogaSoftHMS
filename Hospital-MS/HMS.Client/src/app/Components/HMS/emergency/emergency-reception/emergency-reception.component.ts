import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { trigger, transition, style, animate } from '@angular/animations';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
@Component({
  selector: 'app-emergency-reception',
  templateUrl: './emergency-reception.component.html',
  styleUrl: './emergency-reception.component.css',
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
export class EmergencyReceptionComponent {
  emergencyForm: FormGroup;
  TitleList = ['الطوارئ والإستقبال'];
  insuranceCompanies: any;
  constructor(private fb: FormBuilder, private appointmentService: AppointmentService, private messageService: MessageService , private insuranceService: InsuranceService) {
    this.emergencyForm = this.fb.group({
      patientName: ['', Validators.required],
      patientPhone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      emergencyLevel: ['', Validators.required],
      companionName: ['', Validators.required],
      appointmentType: ['Emergency'],
      companionNationalId: ['', [Validators.required, Validators.pattern(/^[0-9]{14}$/)]],
      companionPhone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      notes: [''],
      insuranceCompanyId: [''],
      insuranceCategoryId: [''],
      insuranceNumber: [''],
      referred: [''],
      referredClinic: [''],
      paymentMethod: [''],

    });
    this.insuranceService.getAllInsurances().subscribe({
      next: (data) => {
        this.insuranceCompanies = data.results;
        console.log(this.insuranceCompanies);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  submitForm() {
    this.appointmentService.createAppointment(this.emergencyForm.value).subscribe({
      next: (data) => {
        this.emergencyForm.reset();
        this.messageService.add({ severity: 'success', summary: 'تم الحجز', detail: 'تم إنشاء الحجز بنجاح' });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: 'حدث خطأ أثناء إنشاء الحجز' });
      }
    })
  }
}
