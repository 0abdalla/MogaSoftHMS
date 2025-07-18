import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { DayToArabicPipe } from '../../../../Pipes/day-to-arabic.pipe';
declare var bootstrap: any;

@Component({
  selector: 'app-doctors-medical-service',
  templateUrl: './doctors-medical-service.component.html',
  styleUrl: './doctors-medical-service.component.css'
})
export class DoctorsMedicalServiceComponent implements OnInit {
  TitleList = ['إدارة الأطباء', 'نوع الخدمة'];
  DaysWeekSelected: string[] = [];
  RadiologyTypesList: any[] = []
  serviceForm!: FormGroup;
  RadiologyTypeForm!: FormGroup;
  isFilter = true;
  total = 0;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  pagedResponseModel: PagedResponseModel<any> = {};

  constructor(private fb: FormBuilder, private messageService: MessageService, private appointmentService: AppointmentService) {
    this.serviceForm = this.fb.group({
      id: 0,
      name: ['', Validators.required],
      price: [''],
      type: ['', Validators.required],
      radiologyBodyTypeName: [''],
      weekDays: ['', Validators.required]
    });

    this.RadiologyTypeForm = this.fb.group({
      id: 0,
      name: ['', Validators.required],
      medicalServiceId: ['', Validators.required],
    });

    this.serviceForm.get('type')?.valueChanges.subscribe(type => {
      const priceControl = this.serviceForm.get('price');

      if (type === 'Screening' || type === 'Radiology') {
        priceControl?.setValidators(Validators.required);
      } else {
        priceControl?.clearValidators();
        priceControl?.setValue(null);
      }

      priceControl?.updateValueAndValidity();
    });

    this.serviceForm.get('weekDays')?.valueChanges.subscribe(days => {
      let checked = this.DaysWeekSelected.find(i => i === days);
      if (!checked && days) {
        this.DaysWeekSelected.push(days);
      }
    });
  }

  ngOnInit(): void {
    this.getMedicalServices();
  }

  openMedicalServiceModalModal(item: any) {
    this.serviceForm.reset();
    this.DaysWeekSelected = [];
    const modal = new bootstrap.Modal(document.getElementById('MedicalServiceModal')!);
    modal.show();
    if (item)
      this.formInit(item);
  }

  openRadiologyBodyTypes(item: any) {
    this.RadiologyTypeForm.reset();
    const modal = new bootstrap.Modal(document.getElementById('RadiologyBodyTypesModal')!);
    modal.show();
  }

  formInit(item: any) {
    const pipe = new DayToArabicPipe();
    let days = item.medicalServiceSchedules.map(schedule => schedule.weekDay);
    this.DaysWeekSelected = days.map(day => pipe.transform(day));
    this.serviceForm.patchValue({
      id: item.id,
      name: item.name,
      price: item.price,
      type: item.type,
      weekDays: null
    });
  }

  getMedicalServices() {
    this.appointmentService.GetMedicalService(this.pagingFilterModel).subscribe(data => {
      this.pagedResponseModel.results = data.results;
      this.RadiologyTypesList = this.pagedResponseModel.results.filter(item => item.type === 'Radiology').map(i => { return { name: i.name, medicalServiceId: i.id } });
      this.total = data.totalCount;
      this.pagedResponseModel.results.forEach(item => {
        item.days = item.medicalServiceSchedules.map(schedule => schedule.weekDay).join(';;;');
      });
       this.pagedResponseModel.results.forEach(item => {
        item.bodyTypes = item.radiologyBodyTypes.map(i => i.name).join(';;;');
      });
    });
  }

  onPageChange(page: any) {
    this.pagingFilterModel.currentPage = page.page;
    this.getMedicalServices();
  }

  filterChecked(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getMedicalServices();
  }

  removeDay(index: number) {
    this.DaysWeekSelected.splice(index, 1);
  }

  AddMedicalService() {
    if (this.serviceForm.invalid) {
      this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'الرجاء ملء جميع الحقول المطلوبة' });
      return;
    }

    let formValue = this.serviceForm.value;
    formValue.weekDays = this.DaysWeekSelected;

    if (!formValue.id) {
      this.appointmentService.addService(formValue).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'نجاح', detail: 'تم إضافة الخدمة بنجاح' });
          this.getMedicalServices();
          this.serviceForm.reset();
          const modal = bootstrap.Modal.getInstance(document.getElementById('MedicalServiceModal')!);
          modal.hide();
        },
        error: (err) => {
          this.messageService.add({ severity: 'error', summary: 'خطأ', detail: err.error.message });
        }
      });
    } else {
      this.appointmentService.editService(formValue.id, formValue).subscribe({
        next: (res) => {
          this.messageService.add({ severity: 'success', summary: 'نجاح', detail: 'تم تحديث الخدمة بنجاح' });
          this.getMedicalServices();
          this.serviceForm.reset();
          const modal = bootstrap.Modal.getInstance(document.getElementById('MedicalServiceModal')!);
          modal.hide();
        },
        error: (err) => {
          this.messageService.add({ severity: 'error', summary: 'خطأ', detail: err.error.message });
        }
      });
    }
  }

  AddRadiologyBodyTypes() {
    debugger;
    if (this.RadiologyTypeForm.invalid) {
      this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'الرجاء ملء جميع الحقول المطلوبة' });
      return;
    }

    let formValue = this.RadiologyTypeForm.value;
    this.RadiologyTypeForm.patchValue({ id: 0, });
    this.appointmentService.CreateRadiologyBodyType(formValue).subscribe({
      next: (res) => {
        this.messageService.add({ severity: 'success', summary: 'نجاح', detail: 'تم إضافة الخدمة بنجاح' });
        this.getMedicalServices();
        this.RadiologyTypeForm.reset();
        const modal = bootstrap.Modal.getInstance(document.getElementById('RadiologyBodyTypesModal')!);
        modal.hide();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: err.error.message });
      }
    });
  }
}
