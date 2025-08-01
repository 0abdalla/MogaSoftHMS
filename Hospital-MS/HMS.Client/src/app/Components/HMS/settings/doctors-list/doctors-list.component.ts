import { trigger, transition, style, animate, query } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { SharedService } from '../../../../Services/shared.service';
declare var bootstrap: any;
@Component({
  selector: 'app-doctors-list',
  templateUrl: './doctors-list.component.html',
  styleUrl: './doctors-list.component.css',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-in', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        animate('200ms ease-out', style({ opacity: 0 })),
      ])
    ])
  ],
})
export class DoctorsListComponent implements OnInit {
  TitleList = ['إدارة الأطباء', 'الأطباء'];
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  pagedResponseModel: PagedResponseModel<any> = {};
  doctorsData!: any[];
  // 
  doctors: any[] = [];
  filterForm!: FormGroup;
  // 
  pageSize = 16;
  currentPage = 1;
  total = 0;
  fixed = Math.ceil(this.total / this.pageSize);
  // 
  selectedDoctor!: any;
  isFilter = false;
  constructor(private fb: FormBuilder, private doctorService: StaffService, private router: Router, private sharedService: SharedService) {
    this.filterForm = this.fb.group({
      Search: [null],
      Status: [''],
    });
  }
  ngOnInit(): void {
    this.getDoctors();
    this.getCounts();
  }
  getDoctors() {
    this.doctorService.getDoctors(this.pagingFilterModel).subscribe({
      next: (res) => {
        this.doctors = res.results.map((doctor: any) => {

          if (doctor.status === 'Active') {
            doctor.status = 'متاح';
          } else {
            doctor.status = 'غير متاح';
          }
          return doctor;
        });

        this.total = res.totalCount;
      },
      error: () => {
      }
    })
  }


  filterChecked(filters: FilterModel[]) {
    debugger;
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getDoctors();

  }

  ApplyCardFilter(item: any) {
    debugger;
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = this.sharedService.CreateFilterList('Type', item.value);
    this.getDoctors();
  }
  // 
  openDoctorModal(id: number) {
    this.getDoctorById(id);
    const modal = new bootstrap.Modal(document.getElementById('doctorDetailsModal')!);
    modal.show();
  }
  getDoctorById(id: number) {
    this.doctorService.getDoctorById(id).subscribe(res => {
      this.selectedDoctor = {
        ...res.results,
        gender: this.translateGender(res.results.gender),
        maritalStatus: this.translateMaritalStatus(res.results.maritalStatus),
        degree: this.translateDegree(res.results.degree),
        doctorSchedules: res.results.doctorSchedules?.map((schedule: any) => ({
          ...schedule,
          weekDay: this.translateWeekDay(schedule.weekDay)
        }))

      };
    });
  }
  // 
  getCounts() {
    this.doctorService.getDoctorsCount().subscribe({
      next: (data: any) => {
        this.doctorsData = data.results;

      },
      error: (err) => {
      }
    });
  }
  getDoctorColor(type: string): string {
    const item = this.doctorsData.find(e => e.name === type);
    return item ? item.back : 'linear-gradient(237.82deg, #ccc, #eee)';
  }
  // 
  editDoctor(doctorId: number): void {
    const modalElement = document.getElementById('doctorDetailsModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      if (modal) {
        modal.hide();
      }
    }

    this.router.navigate(['/hms/settings/doctors', doctorId]);
  }
  print() { }
  exportToPDF() { }
  susbendDoctor() { }
  // 
  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.getDoctors();
  }

  goToEditDoctor(id: any, event: Event) {
    event.stopPropagation();
    this.router.navigate(['/hms/settings/doctors'], { queryParams: { id: id } });

  }
  // =======================================================================================

  private translateWeekDay(weekDay: string): string {
    const weekDayMap: { [key: string]: string } = {
      'Sunday': 'الأحد',
      'Monday': 'الإثنين',
      'Tuesday': 'الثلاثاء',
      'Wednesday': 'الأربعاء',
      'Thursday': 'الخميس',
      'Friday': 'الجمعة',
      'Saturday': 'السبت'
    };
    return weekDayMap[weekDay] || weekDay;
  }

  private translateGender(gender: string): string {
    const genderMap: { [key: string]: string } = {
      'Male': 'ذكر',
      'Female': 'أنثى'
    };
    return genderMap[gender] || gender;
  }

  private translateMaritalStatus(status: string): string {
    const maritalStatusMap: { [key: string]: string } = {
      'Single': 'أعزب',
      'Married': 'متزوج',
      'Divorced': 'مطلق',
      'Widowed': 'أرمل'
    };
    return maritalStatusMap[status] || status;
  }

  private translateDegree(degree: string): string {
    const degreeMap: { [key: string]: string } = {
      'PhD': 'دكتوراه',
      'Master': 'ماجستير',
      'Bachelor': 'بكالوريوس',
      'Diploma': 'دبلوم'
    };
    return degreeMap[degree] || degree;
  }
  getGeneralServiceName(medicalServices: any[]): string {
    const generalService = medicalServices.find(service => service.type === 'General');
    if (generalService) {
      return generalService.name.replace('كشف', '').trim();
    }
    return '---';
  }
}
