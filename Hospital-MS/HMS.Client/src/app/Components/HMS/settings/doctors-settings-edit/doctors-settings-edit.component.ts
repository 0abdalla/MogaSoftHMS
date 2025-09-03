import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
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
  departments: any[] = [];
  doctorId: any;
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
    const minBirthDate = new Date(1920, 0, 1);
    const maxBirthDate = new Date(2025 , 0 , 1);

    this.doctorId = this.activ.snapshot.queryParamMap.get('id');

    this.doctorForm = this.fb.group({
      FullName: ['', [Validators.required]],
      NationalId: ['', [Validators.required, Validators.pattern(/^[0-9]{14}$/)]],
      Gender: ['', [Validators.required]],
      DateOfBirth: ['', [Validators.required, this.minDateValidator(minBirthDate), this.maxDateValidator(maxBirthDate)]],
      MaritalStatus: ['', [Validators.required]],
      Phone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      Email: ['', [Validators.required, Validators.email]],
      Address: ['', [Validators.required]],
      DepartmentId: ['', [Validators.required]],
      SpecialtyId: ['1'],
      Degree: ['', [Validators.required]],
      Status: ['', [Validators.required]],
      StartDate: ['', [Validators.required]],
      Notes: [''],
      Price: ['', [Validators.required]],
      DoctorSchedules: this.fb.array([]),
    });

  }

  ngOnInit(): void {
    this.getDeps();
    this.addSchedule();
    if (this.doctorId)
      this.getDoctorById(this.doctorId);
  }
  
    minDateValidator(minDate: Date): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } | null => {
        if (!control.value) {
          return null;
        }
        const inputDate = new Date(control.value);
        if (inputDate < minDate) {
          return { minDate: { value: control.value } };
        }
        return null;
      };
    }
    maxDateValidator(maxDate: Date): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } | null => {
        if (!control.value) {
          return null;
        }
        const inputDate = new Date(control.value);
        if (inputDate > maxDate) {
          return { maxDate: { value: control.value } };
        }
        return null;
      };
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
        capacity: ['', Validators.required],
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

  getDeps() {
    this.admissionService.getDepartments().subscribe((res) => {
      this.departments = res.results;
      console.log(this.departments);
    });
  }

  getDoctorById(id: number) {
    this.staffService.getDoctorById(id).subscribe((data) => {
      let doctor = data.results;
      console.log(doctor);
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
        Price: doctor.price,
      });
      this.patchDoctorSchedules(doctor.doctorSchedules);
    });
  }

  patchDoctorSchedules(schedules: any[]) {
    const formArray = this.schedules;
    formArray.clear();

    if (schedules && schedules.length > 0) {
      schedules.forEach((schedule) => {
        formArray.push(
          this.fb.group({
            weekDay: [schedule.weekDay || '', Validators.required],
            startTime: [this.formatTime(schedule.startTime) || '', Validators.required],
            endTime: [this.formatTime(schedule.endTime) || '', Validators.required],
            capacity: [schedule.capacity || '', Validators.required],
          })
        );
      });
    } else {
      this.addSchedule();
    }
  }

  formatTime(timeString: string): string {
    if (!timeString) return '';
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
    this.buildFormData(formData, formValue);
    if (!this.doctorId) {
      this.staffService.postDoctor(formData).subscribe({
        next: (res) => {

          this.messageService.add({
            severity: 'success',
            summary: 'عملية ناجحة',
            detail: 'تمت إضافة الطبيب بنجاح',
          });
          setTimeout(() => {
            this.router.navigate(['/hms/settings/doctors-list']);
          }, 1000);
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
      this.staffService.putDoctor(formData, this.doctorId).subscribe({
        next: (res: any) => {
          console.log(res);
          console.log('Sent Data:' , formData);
          this.messageService.add({
            severity: 'success',
            summary: 'عملية ناجحة',
            detail: 'تم تحديث بيانات الطبيب بنجاح',
          });
          setTimeout(() => {
            this.router.navigate(['/hms/settings/doctors-list']);
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

  getAvailableDays(currentIndex: number): { value: string; label: string }[] {
    const selectedDays = this.schedules.controls
      .map((ctrl, index) => index !== currentIndex ? ctrl.get('weekDay')?.value : null)
      .filter(day => day);

    return this.days.filter(day => !selectedDays.includes(day.value));
  }
}
