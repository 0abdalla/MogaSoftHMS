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
      endDate: ['', Validators.required],
      client: ['', Validators.required],
      assignedEmployees: ['', Validators.required],
      budget: ['', [Validators.min(0)]],
      tags: ['', Validators.required],
      description: [''],
      examType: ['', Validators.required],
      doctorName: ['', Validators.required],
      patientNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      age: ['', [Validators.min(0)]],
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
      partnerEmail: ['', [Validators.email]]
    });
  }

  ngOnInit(): void { }

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
