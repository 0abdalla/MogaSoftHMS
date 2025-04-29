import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { AdmissionService } from '../../../../Services/HMS/admission.service';

@Component({
  selector: 'app-doctors-settings-edit',
  templateUrl: './doctors-settings-edit.component.html',
  styleUrl: './doctors-settings-edit.component.css'
})
export class DoctorsSettingsEditComponent implements OnInit {
  doctorForm!: FormGroup;
  departments!: any;
  doctorId!: number;
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
    private staffService: StaffService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private activ: ActivatedRoute,
    private admissionService: AdmissionService,
    private router: Router
  ) {
    this.activ.params.subscribe((params) => {
      this.doctorId = params['id'];
    });

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
      Status: ['', Validators.required],
      StartDate: ['', Validators.required],
      Notes: [''],
      DoctorSchedules: this.fb.array([]), // Initialize empty FormArray
    });
  }

  ngOnInit(): void {
    this.getDeps();
    this.getDoctorById(this.doctorId);
  }

  // Getter for DoctorSchedules FormArray
  get schedules(): FormArray {
    return this.doctorForm.get('DoctorSchedules') as FormArray;
  }

  // Add a new schedule row
  addSchedule(): void {
    this.schedules.push(
      this.fb.group({
        weekDay: ['', Validators.required],
        startTime: ['', Validators.required],
        endTime: ['', Validators.required],
      })
    );
  }

  // Remove a schedule row
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

  getDeps() {
    this.admissionService.getDepartments().subscribe((res) => {
      this.departments = res;
    });
  }

  getDoctorById(id: number) {
    this.staffService.getDoctorById(id).subscribe((doctor) => {
      this.doctorForm.patchValue({
        FullName: doctor.fullName,
        NationalId: doctor.nationalId,
        Gender: doctor.gender,
        DateOfBirth: doctor.dateOfBirth,
        MaritalStatus: doctor.maritalStatus,
        Phone: doctor.phone,
        Email: doctor.email,
        Address: doctor.address,
        DepartmentId: doctor.departmentId,
        Degree: doctor.degree,
        Status: doctor.status === 'Suspended' ? 'Inactive' : 'Active',
        StartDate: doctor.startDate,
        Notes: doctor.notes,
        // PhotoUrl is not patched into the form as it's handled via file input
      });
      this.patchDoctorSchedules(doctor.doctorSchedules);
    });
  }

  patchDoctorSchedules(schedules: any[]) {
    const formArray = this.schedules;
    formArray.clear(); // Clear existing schedules

    if (schedules && schedules.length > 0) {
      schedules.forEach((schedule) => {
        formArray.push(
          this.fb.group({
            weekDay: [schedule.weekDay || '', Validators.required],
            startTime: [this.formatTime(schedule.startTime) || '', Validators.required],
            endTime: [this.formatTime(schedule.endTime) || '', Validators.required],
          })
        );
      });
    } else {
      this.addSchedule(); // Add one empty schedule if none exist
    }
  }

  formatTime(timeString: string): string {
    if (!timeString) return '';
    // Assuming timeString is in "HH:mm:ss" format, convert to "HH:mm"
    return timeString.substring(0, 5);
  }

  onSubmit() {
    if (this.doctorForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'خطأ',
        detail: 'الرجاء ملء جميع الحقول المطلوبة بشكل صحيح',
      });
      return;
    }

    const formData = new FormData();
    const formValue: any = this.doctorForm.value;
    this.buildFormData(formData,formValue);
    // Append schedules
    formValue.DoctorSchedules.forEach((schedule: any, index: number) => {
      if (schedule.weekDay && schedule.startTime && schedule.endTime) {
        formData.append(`DoctorSchedules[${index}].weekDay`, schedule.weekDay);
        formData.append(`DoctorSchedules[${index}].startTime`, schedule.startTime);
        formData.append(`DoctorSchedules[${index}].endTime`, schedule.endTime);
      }
    });

    this.staffService.putDoctor(formData, this.doctorId).subscribe({
      next: (res: any) => {
        this.messageService.add({
          severity: 'success',
          summary: 'عملية ناجحة',
          detail: 'تم تحديث بيانات الطبيب بنجاح',
        });
        setTimeout(() => {
          this.router.navigate(['/settings/doctors-list']);
        }, 1000);
      },
      error: (err) => {
        console.error('Error:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'حدث خطأ',
          detail: 'فشل في تحديث بيانات الطبيب',
        });
      },
    });
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

  buildFormData(formData: any, data: any, parentKey: any = null, key: any = null) {
    if (data instanceof File)
      formData.append(key, data);
    else if (data && typeof data === 'object' && !(data instanceof Date) && !(data instanceof File)) {
      Object.keys(data).forEach(key => {
        this.buildFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key, key);
      });
    } else {
      const value = data == null ? '' : data;

      formData.append(parentKey, value);
    }
  }
}
