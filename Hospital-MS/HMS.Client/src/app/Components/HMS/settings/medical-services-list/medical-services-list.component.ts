import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../../../../Services/HMS/appointment.service';
import { FormBuilder, FormGroup , Validators } from '@angular/forms';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-medical-services-list',
  templateUrl: './medical-services-list.component.html',
  styleUrl: './medical-services-list.component.css'
})
export class MedicalServicesListComponent implements OnInit {
  services!:any[];
  total!:number;
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
  constructor(private appointmentService : AppointmentService , private fb : FormBuilder , private messageService : MessageService){
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
      name: ['' , [Validators.required , Validators.minLength(3)]],
      price: ['' , [Validators.required , Validators.pattern('^[0-9]*$') , Validators.min(1) , Validators.max(1000)] ],
      type: ['' , Validators.required],
    })
  }

  ngOnInit(): void {
    this.getServices();
    
  }
  getServices(){
    this.appointmentService.getServices(this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.searchText, this.pagingFilterModel.filterList).subscribe({
      next: (data) => {
        this.services = data.results.map((service: any) => {
          switch (service.serviceType) {
            case 'General':
              service.serviceType = 'عام';
              break;
            case 'Consultation':
              service.serviceType = 'استشارة';
              break;
            case 'Screening':
              service.serviceType = 'تحاليل';
              break;
            case 'Radiology':
              service.serviceType = 'أشعة';
              break;
            case 'Surgery':
              service.serviceType = 'عمليات';
              break;
          }
          return service;
        });
        this.total = data.totalCount;
        console.log('Services', this.services);
      },
      error: (err) => {
        console.log(err);
        this.services = [];
      }
    });
  }
  // 
  applyFilters(){
    this.getServices();
  }
  resetFilters() {
    this.filterForm.patchValue({ SearchText: '' }, { emitEvent: false });
    this.pagingFilterModel.searchText = '';
    this.getServices();
  }
  
  openServiceDetails(serviceId : number){
    this.appointmentService.getServices().subscribe({
      next: (data) => {
        this.serviceDetails = data.results.find((service: any) => service.serviceId === serviceId);
        console.log(this.serviceDetails);
        this.serviceForm.patchValue({
          name: this.serviceDetails.serviceName,
          price: this.serviceDetails.price,
          type: this.serviceDetails.serviceType
        });
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.getServices();
  }
  addService(){
    this.appointmentService.addService(this.serviceForm.value).subscribe({
      next: (data) => {
        this.getServices();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة الخدمة بنجاح' });
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة الخدمة' });
      }
    });
  }
  editService(){
    this.appointmentService.editService(this.serviceDetails.serviceId, this.serviceForm.value).subscribe({
      next: (data) => {
        this.getServices();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل الخدمة بنجاح' });
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل الخدمة' });
      }
    });
  }
}
