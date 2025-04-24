import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { AdmissionService } from '../../../../core/services/admission.service';
import { AppointmentService } from '../../../../core/services/appointment.service';
import { StaffService } from '../../../../core/services/staff.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-staff-form',
  templateUrl: './staff-form.component.html',
  styleUrl: './staff-form.component.css',
  animations: [
    trigger('fadeIn', [
      transition('void => *', [
        style({ opacity: 0 }),
        animate('300ms ease-in', style({ opacity: 1 }))
      ])
    ])
  ]
})
export class StaffFormComponent implements OnInit {
  employeeForm: FormGroup;
  departments: any;
  clinics: any;
  selectedFiles: File[] = [];

  constructor(
    private fb: FormBuilder,
    private admissionService: AdmissionService,
    private appointmentService: AppointmentService,
    private staffService: StaffService,
    private messageService: MessageService
  ) {
    this.employeeForm = this.fb.group({
      fullName: ['', Validators.required],
      nationalId: ['', Validators.required],
      gender: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      specialization: ['', Validators.required],
      type: ['', Validators.required],
      jobTitle: [''],
      hireDate: ['', Validators.required],
      status: ['', Validators.required],
      departmentId: [''],
      clinicId: [''],
      maritalStatus: ['', Validators.required],
      notes: [''],
      Files: [null],
    });
  }

  ngOnInit() {
    this.getDepartments();
    this.getClinics();
    this.employeeForm.get('type')?.valueChanges.subscribe((type) => {
      if (type === 'Doctor' || type === 'Nurse') {
        this.employeeForm.get('departmentId')?.setValidators([Validators.required]);
        this.employeeForm.get('clinicId')?.setValidators([Validators.required]);
        this.employeeForm.get('jobTitle')?.clearValidators();
      } else {
        this.employeeForm.get('departmentId')?.clearValidators();
        this.employeeForm.get('clinicId')?.clearValidators();
        this.employeeForm.get('jobTitle')?.setValidators([Validators.required]);
      }
      this.employeeForm.get('departmentId')?.updateValueAndValidity();
      this.employeeForm.get('clinicId')?.updateValueAndValidity();
      this.employeeForm.get('jobTitle')?.updateValueAndValidity();
    });
  }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const files: FileList = target.files as FileList;
    this.selectedFiles = [];

    if (files && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const validTypes = ['image/png', 'image/jpeg', 'application/pdf'];
        if (validTypes.includes(file.type)) {
          this.selectedFiles.push(file);
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'خطأ',
            detail: `الملف ${file.name} غير مدعوم. يُسمح فقط بـ PNG, JPG, JPEG, PDF.`,
          });
        }
      }
    }
  }

  onSubmit() {
    if (this.employeeForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'فشل',
        detail: 'يرجى ملء جميع الحقول المطلوبة بشكل صحيح',
      });
      return;
    }

    if (!this.selectedFiles || this.selectedFiles.length === 0) {
      this.messageService.add({
        severity: 'error',
        summary: 'فشل',
        detail: 'يجب اختيار ملف واحد على الأقل',
      });
      return;
    }

    const formData = new FormData();
    this.selectedFiles.forEach((file, index) => {
      formData.append(`Files[${index}]`, file, file.name);
    });

    formData.append('fullName', this.employeeForm.get('fullName')?.value || '');
    formData.append('nationalId' , this.employeeForm.get('nationalId')?.value || '')
    formData.append('gender', this.employeeForm.get('gender')?.value || '');
    formData.append('phoneNumber', this.employeeForm.get('phoneNumber')?.value || '');
    formData.append('email', this.employeeForm.get('email')?.value || '');
    formData.append('address', this.employeeForm.get('address')?.value || '');
    formData.append('specialization', this.employeeForm.get('specialization')?.value || '');
    formData.append('type', this.employeeForm.get('type')?.value || '');
    formData.append('jobTitle', this.employeeForm.get('jobTitle')?.value || '');
    formData.append('hireDate', this.employeeForm.get('hireDate')?.value || '');
    formData.append('status', this.employeeForm.get('status')?.value || '');
    formData.append('departmentId', this.employeeForm.get('departmentId')?.value || '');
    formData.append('clinicId', this.employeeForm.get('clinicId')?.value || '');
    formData.append('maritalStatus', this.employeeForm.get('maritalStatus')?.value || '');
    formData.append('notes', this.employeeForm.get('notes')?.value || '');

    this.staffService.addStaff(formData).subscribe({
      next: (data) => {
        console.log(data);
        console.log(formData);
        console.log(this.employeeForm.value);
        this.employeeForm.reset();
        this.selectedFiles = [];
        this.messageService.add({
          severity: 'success',
          summary: 'نجاح',
          detail: 'تم إضافة الموظف بنجاح',
        });
      },
      error: (error) => {
        console.error(error);
        this.messageService.add({
          severity: 'error',
          summary: 'فشل',
          detail: 'فشل في إضافة الموظف',
        });
      },
    });
  }

  getDepartments() {
    this.admissionService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  getClinics() {
    this.appointmentService.getClinics().subscribe({
      next: (data) => {
        this.clinics = data;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
}
