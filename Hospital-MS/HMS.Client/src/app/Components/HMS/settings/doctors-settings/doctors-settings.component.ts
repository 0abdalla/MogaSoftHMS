import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
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
  clinics!: any;
  jobLevels!:any;
  doctorForm!: FormGroup;
  // 
  services!:any;
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
    const minBirthDate = new Date(1920, 0, 1);
    const maxBirthDate = new Date();
    this.doctorForm = this.fb.group({
      FullName: ['', Validators.required],
      NationalId: ['', [Validators.required , Validators.pattern(/^[0-9]{14}$/)]],
      Gender: ['', Validators.required],
      DateOfBirth: ['', [Validators.required, this.minDateValidator(minBirthDate), this.maxDateValidator(maxBirthDate)]],
      MaritalStatus: ['', Validators.required],
      Phone: ['', [Validators.required , Validators.pattern(/^01[0125][0-9]{8}$/)]],
      Email: ['', [Validators.required, Validators.email]],
      Address: ['', Validators.required],
      MedicalServiceIds: [[], Validators.required],
      Degree: ['', Validators.required],
      Status: ['Active', Validators.required],
      StartDate: ['', Validators.required],
      Notes: [''],
      DoctorSchedules: this.fb.array([], this.scheduleValidator.bind(this)),
    });
  }

  ngOnInit(): void {
    this.getClinics();
    this.getDepartments();
    this.addSchedule();
    this.getServices();
    this.getJobLevels();
  }

  getClinics() {
    this.appointmentService.getClinics().subscribe({
      next: (data) => {
        this.clinics = data.results;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  getDepartments() {
    this.admissionService.getDepartments().subscribe({
      next: (data:any) => {
        this.departments = data.results;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  getServices() {
    const filterParams = {
    };
  
    this.appointmentService.getServices(1, 100, '', filterParams).subscribe({
      next: (data) => {
        this.services = data.results.filter((service:any) => service.type !== 'Radiology' && service.type !== 'Screening') || [];
        console.log('Services', this.services);
      },
      error: (err) => {
        console.log(err);
        this.services = [];
      }
    });
  }

  getJobLevels() {
    this.doctorService.getJobLevels('', 1, 100, []).subscribe({
      next: (data:any) => {
        this.jobLevels = data.results;
        console.log('Job Levels', this.jobLevels);
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
    const scheduleGroup = this.fb.group(
      {
        weekDay: ['', Validators.required],
        startTime: ['', Validators.required],
        endTime: ['', Validators.required],
        capacity: ['', [Validators.required, Validators.min(1), Validators.max(60)]],
      },
      { validators: [this.timeRangeValidator.bind(this)] }
    );
  
    this.schedules.push(scheduleGroup);
    scheduleGroup.valueChanges.subscribe(() => {
      this.schedules.updateValueAndValidity();
      if (scheduleGroup.hasError('invalidTimeRange')) {
        this.messageService.add({
          severity: 'warn',
          summary: 'تحذير',
          detail: 'وقت الانتهاء يجب أن يكون بعد وقت البدء',
        });
      }
      if (this.schedules.hasError('overlappingSchedule')) {
        this.messageService.add({
          severity: 'warn',
          summary: 'تحذير',
          detail: 'لا يمكن اختيار نفس اليوم مع نفس المواعيد',
        });
      }
    });
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
    if (this.doctorForm.valid && !this.photoError && !this.doctorForm.hasError('overlappingSchedule')) {
      const formData = new FormData();
  
      formData.append('FullName', this.doctorForm.value.FullName);
      formData.append('NationalId', this.doctorForm.value.NationalId);
      formData.append('Gender', this.doctorForm.value.Gender);
      formData.append('DateOfBirth', this.doctorForm.value.DateOfBirth);
      formData.append('MaritalStatus', this.doctorForm.value.MaritalStatus);
      formData.append('Phone', this.doctorForm.value.Phone);
      formData.append('Email', this.doctorForm.value.Email);
      formData.append('Address', this.doctorForm.value.Address);
      formData.append('Degree', this.doctorForm.value.Degree);
      formData.append('Status', this.doctorForm.value.Status);
      formData.append('StartDate', this.doctorForm.value.StartDate);
      formData.append('Notes', this.doctorForm.value.Notes);

      this.doctorForm.value.MedicalServiceIds.forEach((id: number, index: number) => {
        formData.append(`MedicalServiceIds[${index}]`, id.toString());
      });

      if (this.photoFile) {
        formData.append('Photo', this.photoFile);
      }

      this.doctorForm.value.DoctorSchedules.forEach((schedule: any, index: number) => {
        if (schedule.weekDay && schedule.startTime && schedule.endTime) {
          formData.append(`DoctorSchedules[${index}].weekDay`, schedule.weekDay);
          formData.append(`DoctorSchedules[${index}].startTime`, schedule.startTime);
          formData.append(`DoctorSchedules[${index}].endTime`, schedule.endTime);
          formData.append(`DoctorSchedules[${index}].capacity`, schedule.capacity);
        }
      });

      this.doctorService.postDoctor(formData).subscribe({
        next: (res) => {
          console.log('Doctor added successfully', res);
          console.log('Data Sent:', this.doctorForm.value);
  
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
          console.log('Form Value:', this.doctorForm.value);
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
  // 
  scheduleValidator(control: AbstractControl): ValidationErrors | null {
  const schedules = (control as FormArray).controls as FormGroup[];
  const overlapping = schedules.find((currentSchedule, i) => {
    const { weekDay: currentDay, startTime: currentStart, endTime: currentEnd } = currentSchedule.value;
    if (!currentDay || !currentStart || !currentEnd) return false;
    return schedules.some((otherSchedule, j) => {
      if (i === j) return false;
      const { weekDay: otherDay, startTime: otherStart, endTime: otherEnd } = otherSchedule.value;
      if (!otherDay || !otherStart || !otherEnd) return false;
      if (currentDay !== otherDay) return false;

      return this.hasTimeOverlap(
        this.timeToMinutes(currentStart),
        this.timeToMinutes(currentEnd),
        this.timeToMinutes(otherStart),
        this.timeToMinutes(otherEnd)
      );
    });
  });
  return overlapping ? { overlappingSchedule: true } : null;
  }

  timeRangeValidator(control: AbstractControl): ValidationErrors | null {
  const { startTime, endTime } = control.value;
  if (startTime && endTime) {
    const start = this.timeToMinutes(startTime);
    const end = this.timeToMinutes(endTime);
    
    return end <= start ? { invalidTimeRange: true } : null;
  }
  return null;
  }

  hasTimeOverlap(start1: number, end1: number, start2: number, end2: number): boolean {
    return (
      (start1 === start2 && end1 === end2) ||
      (start1 < end2 && end1 > start2)
    );
  }

  timeToMinutes(time: string): number {
    const [hours, minutes] = time.split(':').map(Number);
    return hours * 60 + minutes;
  }
  // 
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
}
