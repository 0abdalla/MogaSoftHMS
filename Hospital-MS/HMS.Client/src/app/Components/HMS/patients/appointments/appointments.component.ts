import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.css'
})
export class AppointmentsComponent {
  admissionsForm: FormGroup;
  selectedAttachments: File[] = [];
  selectedScansAndXRays: File[] = [];

  constructor(private fb: FormBuilder) {
    this.admissionsForm = this.fb.group({
      title: ['', Validators.required],
      orderNumber: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: [''],
      client: [''],
      assignedEmployees: [''],
      budget: [''],
      tags: [''],
      description: [''],
      examType: [''],
      doctorName: [''],
      patientNumber: [''],
      email: [''],
      age: [''],
      address: [''],
      chronicDiseases: [''],
      formerSurgeries: [''],
      lastMedicalVisit: [''],
      vaccinated: ['false'],
      complain: [''],
      attachments: [null],
      scansAndXRays: [null],
      medicalNotes: [''],
      insuranceType: [''],
      insuranceCompany: [''],
      partnerFullName: [''],
      partnerPhoneNumber: [''],
      partnerEmail: ['']
    });
  }

  ngOnInit(): void {}

  onFileChange(event: Event, controlName: string): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const files = Array.from(input.files);
      if (controlName === 'attachments') {
        this.selectedAttachments = files;
        this.admissionsForm.patchValue({ attachments: files });
      } else if (controlName === 'scansAndXRays') {
        this.selectedScansAndXRays = files;
        this.admissionsForm.patchValue({ scansAndXRays: files });
      }
    }
  }

  onSubmit(): void {
    if (this.admissionsForm.valid) {
    } else {
    }
  }
}
