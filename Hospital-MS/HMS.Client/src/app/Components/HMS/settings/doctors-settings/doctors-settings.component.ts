import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { StaffService } from '../../../../Services/HMS/staff.service';

@Component({
  selector: 'app-doctors-settings',
  templateUrl: './doctors-settings.component.html',
  styleUrl: './doctors-settings.component.css'
})
export class DoctorsSettingsComponent implements OnInit {
  departments!: any;
  clinics!: any[];
  doctorForm!: FormGroup;
  days = [
    { value: 'Friday', label: 'الجمعة' },
    { value: 'Saturday', label: 'السبت' },
    { value: 'Sunday', label: 'الأحد' },
    { value: 'Monday', label: 'الإثنين' },
    { value: 'Tuesday', label: 'الثلاثاء' },
    { value: 'Wednesday', label: 'الأربعاء' },
    { value: 'Thursday', label: 'الخميس' },
  ];
  photoFile: File | null = null;
  photoError = false;

  constructor(
    private admissionService: AdmissionService,
    private appointmentService: AppointmentService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private doctorService: StaffService
  ) {
    this.doctorForm = this.fb.group({
      FullName: ['', Validators.required],
      NationalId: ['', Validators.required],
      Gender: ['', Validators.required],
      DateOfBirth: ['', Validators.required],
      MaritalStatus: ['', Validators.required],
      Phone: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Address: ['', Validators.required],
      DepartmentId: ['', Validators.required],
      SpecialtyId: ['1'],
      Degree: ['', Validators.required],
      Status: ['Active', Validators.required],
      StartDate: ['', Validators.required],
      Notes: [''],
      DoctorSchedules: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.getClinics();
    this.getDepartments();
    this.addSchedule();
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

  get schedules(): FormArray {
    return this.doctorForm.get('DoctorSchedules') as FormArray;
  }

  addSchedule(): void {
    this.schedules.push(
      this.fb.group({
        weekDay: ['', Validators.required],
        startTime: ['', Validators.required],
        endTime: ['', Validators.required],
      })
    );
  }

  removeSchedule(index: number): void {
    if (this.schedules.length > 1) {
      this.schedules.removeAt(index);
    } else {
      this.messageService.add({
        severity: 'warn',
        summary: 'تحذير',
        detail: 'يجب أن يحتوي الجدول على موعد واحد على الأقل',
      });
    }
  }

  onSubmit() {
    if (this.doctorForm.valid && !this.photoError) {
      const formData = new FormData();

      formData.append('FullName', this.doctorForm.value.FullName);
      formData.append('NationalId', this.doctorForm.value.NationalId);
      formData.append('Gender', this.doctorForm.value.Gender);
      formData.append('DateOfBirth', this.doctorForm.value.DateOfBirth);
      formData.append('MaritalStatus', this.doctorForm.value.MaritalStatus);
      formData.append('Phone', this.doctorForm.value.Phone);
      formData.append('Email', this.doctorForm.value.Email);
      formData.append('Address', this.doctorForm.value.Address);
      formData.append('DepartmentId', this.doctorForm.value.DepartmentId);
      formData.append('SpecialtyId', this.doctorForm.value.SpecialtyId);
      formData.append('Degree', this.doctorForm.value.Degree);
      formData.append('Status', this.doctorForm.value.Status);
      formData.append('StartDate', this.doctorForm.value.StartDate);
      formData.append('Notes', this.doctorForm.value.Notes);

      if (this.photoFile) {
        formData.append('Photo', this.photoFile);
      }
      this.doctorForm.value.DoctorSchedules.forEach((schedule: any, index: number) => {
        if (schedule.weekDay && schedule.startTime && schedule.endTime) {
          formData.append(`DoctorSchedules[${index}].weekDay`, schedule.weekDay);
          formData.append(`DoctorSchedules[${index}].startTime`, schedule.startTime);
          formData.append(`DoctorSchedules[${index}].endTime`, schedule.endTime);
        }
      });

      this.doctorService.postDoctor(formData).subscribe({
        next: (res) => {
          console.log('Doctor added successfully', res);
          this.messageService.add({
            severity: 'success',
            summary: 'عملية ناجحة',
            detail: 'تمت إضافة الطبيب بنجاح',
          });
          this.doctorForm.reset();
          this.schedules.clear();
          this.addSchedule();
        },
        error: (err) => {
          console.error('Error:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'حدث خطأ',
            detail: 'حدث خطأ في تسجيل البيانات',
          });
        },
      });
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'خطأ',
        detail: 'الرجاء ملء جميع الحقول المطلوبة بشكل صحيح',
      });
    }
  }

  onFileChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      const validTypes = ['image/jpeg', 'image/png'];
      const maxSize = 5 * 1024 * 1024;

      if (!validTypes.includes(file.type)) {
        this.photoError = true;
        this.messageService.add({
          severity: 'error',
          summary: 'خطأ',
          detail: 'الرجاء تحميل صورة بصيغة JPEG أو PNG',
        });
        return;
      }

      if (file.size > maxSize) {
        this.photoError = true;
        this.messageService.add({
          severity: 'error',
          summary: 'خطأ',
          detail: 'حجم الصورة يجب ألا يتجاوز 5 ميجا',
        });
        return;
      }

      this.photoFile = file;
      this.photoError = false;
    }
  }
}
