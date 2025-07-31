import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-medical-services-list',
  templateUrl: './medical-services-list.component.html',
  styleUrl: './medical-services-list.component.css'
})
export class MedicalServicesListComponent implements OnInit {
  TitleList = ['إعدادات النظام', 'إعدادات المواعيد والحجز'];
  services!: any[];
  total!: number;
  filterForm!: FormGroup;
  serviceForm!: FormGroup;
  serviceDetails!: any;
  // 
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  weekDays = [
    { day: 'الأحد', value: 'Sunday' },
    { day: 'الإثنين', value: 'Monday' },
    { day: 'الثلاثاء', value: 'Tuesday' },
    { day: 'الأربعاء', value: 'Wednesday' },
    { day: 'الخميس', value: 'Thursday' },
    { day: 'الجمعة', value: 'Friday' },
    { day: 'السبت', value: 'Saturday' }
  ];
  isFilter = true;
  constructor(private appointmentService: AppointmentService, private fb: FormBuilder, private messageService: MessageService) {
    this.filterForm = this.fb.group({
      SearchText: [''],
    });

    this.filterForm.get('SearchText')?.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe((searchText: string) => {
        this.pagingFilterModel.searchText = searchText;
        this.getServices();
      });

    this.serviceForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      price: ['', [Validators.required, Validators.pattern('^[0-9]*$'), Validators.min(1)]],
      type: ['', Validators.required],
      medicalServiceSchedules: this.fb.array([this.createSchedule()]),
    })
  }

  ngOnInit(): void {
    this.getServices();
  }
  get medicalServiceSchedules(): FormArray {
    return this.serviceForm.get('medicalServiceSchedules') as FormArray;
  }
  createSchedule(weekDay: string = ''): FormGroup {
    return this.fb.group({
      weekDay: [weekDay, Validators.required],
    });
  }

  addSchedule() {
    this.medicalServiceSchedules.push(this.createSchedule());
  }

  removeSchedule(index: number) {
    if (this.medicalServiceSchedules.length > 1) {
      this.medicalServiceSchedules.removeAt(index);
    } else {
      this.messageService.add({
        severity: 'warn',
        summary: 'لا يمكن الحذف',
        detail: 'يجب أن يبقى يوم واحد على الأقل',
      });
    }
  }
  getServices() {
    this.appointmentService.getServices(this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.searchText, this.pagingFilterModel.filterList).subscribe({
      next: (data) => {
        this.services = data.results.map((service: any) => {
          switch (service.type) {
            case 'General':
              service.serviceType = 'كشف';
              break;
            case 'Consultation':
              service.serviceType = 'استشارة';
              break;
            case 'Screening':
              service.serviceType = 'تحاليل';
              break;
            case 'MRI':
              service.serviceType = 'أشعة رنين';
              break;
            case 'Panorama':
              service.serviceType = 'أشعة بانوراما';
              break;
            case 'XRay':
              service.serviceType = 'أشعة عادية';
              break;
            case 'CTScan':
              service.serviceType = 'أشعة مقطعية';
              break;
            case 'Ultrasound':
              service.serviceType = 'أشعة سونار';
              break;
            case 'Echo':
              service.serviceType = 'أشعة إيكو';
              break;
            case 'Mammogram':
              service.serviceType = 'أشعة ماموجرام';
              break;
            case 'Surgery':
              service.serviceType = 'عمليات';
              break;
          }
          return service;
        });
        this.total = data.totalCount;
      },
      error: (err) => {
        this.services = [];
      }
    });
  }
  // 
  applyFilters(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getServices();
  }

  openServiceDetails(serviceId: number) {
    this.appointmentService.getServices().subscribe({
      next: (data) => {
        this.serviceDetails = data.results.find((service: any) => service.id === serviceId);
        this.medicalServiceSchedules.clear();
        const schedules = this.serviceDetails.medicalServiceSchedules || [];
        if (schedules.length === 0) {
          this.medicalServiceSchedules.push(this.createSchedule());
        } else {
          schedules.forEach((schedule: any) => {
            this.medicalServiceSchedules.push(this.createSchedule(schedule.weekDay));
          });
        }
        this.serviceForm.patchValue({
          name: this.serviceDetails.name,
          price: this.serviceDetails.price,
          type: this.serviceDetails.type,
        });
      },
      error: (err) => {
        console.error('Error fetching services:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'حدث خطأ',
          detail: 'حدث خطأ أثناء تحميل تفاصيل الخدمة',
        });
      },
    });
  }
  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.getServices();
  }
  addService() {
    const formValue = this.serviceForm.value;
    this.appointmentService.addService(formValue).subscribe({
      next: (data) => {
        this.getServices();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة الخدمة بنجاح' });
        this.serviceForm.reset();
        this.medicalServiceSchedules.clear();
        this.medicalServiceSchedules.push(this.createSchedule());
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة الخدمة' });
      },
    });
  }
  onDayChange(index: number) {
    const selectedDay = this.medicalServiceSchedules.at(index).get('weekDay')?.value;
    if (selectedDay && this.isDaySelected(selectedDay, index)) {
      this.messageService.add({
        severity: 'warn',
        summary: 'يوم مكرر',
        detail: 'هذا اليوم تم اختياره بالفعل، اختر يومًا آخر',
      });
      this.medicalServiceSchedules.at(index).get('weekDay')?.setValue('');
    }
  }
  isDaySelected(day: string, currentIndex: number): boolean {
    return this.medicalServiceSchedules.controls.some((control, index) => {
      return index !== currentIndex && control.get('weekDay')?.value === day;
    });
  }
  editService() {
    this.appointmentService.editService(this.serviceDetails.id, this.serviceForm.value).subscribe({
      next: (data) => {
        this.getServices();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل الخدمة بنجاح' });
        this.serviceForm.reset();
        this.medicalServiceSchedules.clear();
        this.medicalServiceSchedules.push(this.createSchedule());
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل الخدمة' });
      },
    });
  }
}
