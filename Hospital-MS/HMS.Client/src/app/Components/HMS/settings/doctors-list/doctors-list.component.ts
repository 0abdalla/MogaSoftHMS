import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { StaffService } from '../../../../Services/HMS/staff.service';
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
  selectedDoctor!:any;
  constructor(private fb : FormBuilder , private doctorService : StaffService , private router : Router){
    this.filterForm = this.fb.group({
      Search: [null],
      Status: [''],
    });
  }
  ngOnInit(): void {
    this.getDoctors();
    this.getCounts();
  }
  getDoctors(){
    const { Search, Status } = this.filterForm.value;
    this.doctorService.getDoctors(this.currentPage, this.pageSize, Status, Search).subscribe({
      next:(res)=>{
        this.doctors = res.data.map((doctor:any) => {
          if (doctor.status === 'Active') {
            doctor.status = 'متاح';
          } else {
            doctor.status = 'غير متاح';
          }
          return doctor;
        });
        this.pageSize = res.pageSize;
        this.currentPage = res.pageIndex;
        this.total = res.count;
        this.fixed = Math.ceil(this.total / this.pageSize);
        console.log('data', res);
      },
      error:()=>{
        console.log('error');
      }
    })
  }
  // 
  applyFilters(){
    this.currentPage = 1;
    this.getDoctors();
  }
  resetFilters(){ 
    this.filterForm.reset();
    this.currentPage = 1;
    this.getDoctors();
  }
  // 
  openDoctorModal(id:number){
    this.getDoctorById(id);
  }
  getDoctorById(id: number) {
    this.doctorService.getDoctorById(id).subscribe(res => {
      this.selectedDoctor = {
        ...res,
        gender: this.translateGender(res.gender),
        maritalStatus: this.translateMaritalStatus(res.maritalStatus),
        degree: this.translateDegree(res.degree),
        doctorSchedules: res.doctorSchedules?.map((schedule: any) => ({
          ...schedule,
          weekDay: this.translateWeekDay(schedule.weekDay)
        }))
      };
      console.log(this.selectedDoctor);
    });
  }
  // 
  getCounts(){
    this.doctorService.getDoctorsCount().subscribe({
      next: (data:any) => {
        this.doctorsData = [
          {
            name: 'عدد الأطباء',
            count: data.totalDoctors,
            color: 'linear-gradient(237.82deg, #4A90E2 30.69%, #A2C7F2 105.5%)',
            back: '#4A90E2'
          },
          {
            name: 'عدد الأطباء النشطين',
            count: data.totalActiveDoctors,
            color: 'linear-gradient(236.62deg, #28A745 30.14%, #9BE7B2 83.62%)',
            back: '#28A745'
          },
          {
            name: 'عدد التخصصات',
            count: data.totalDepartments,
            color: 'linear-gradient(237.82deg, #FF9800 30.69%, #FFD180 105.5%)',
            back: '#FF9800'
          }
        ]        
      },
      error: (err) => {
        console.log(err);
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

    this.router.navigate(['/settings/doctors', doctorId]);
  }
  print(){}
  exportToPDF(){}
  susbendDoctor(){}
  // 
  onPageChange(page: number) {
    this.currentPage = page;
    this.getDoctors();
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
}
