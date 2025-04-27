import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { StaffService } from '../../../../Services/HMS/staff.service';

@Component({
  selector: 'app-staff-list',
  templateUrl: './staff-list.component.html',
  styleUrl: './staff-list.component.css',
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
export class StaffListComponent implements OnInit {
  employees: any[] = [];
  employeesData!:any[];
  // 
  filterForm!:FormGroup;
  // 
  pageSize = 16;
  currentPage = 1;
  total = 0;
  fixed = Math.ceil(this.total / this.pageSize);
  // 
  selectedEmployee!:any;
  constructor(private staffService : StaffService , private fb : FormBuilder){
    this.filterForm = this.fb.group({
      Search: [''],
      Type: ['']
    })
  }
  ngOnInit(): void {
    this.getCounts();
    this.getEmployees();
  }

  applyFilters(){
    this.currentPage = 1;
    this.getEmployees();
  }

  resetFilters(){
    this.filterForm.reset();
    this.currentPage = 1;
    this.getEmployees();
  }
  
  openStaffModal(id:number){
    this.getEmployeeById(id);
  }

  getEmployeeColor(type: string): string {
    const item = this.employeesData.find(e => e.name === type);
    return item ? item.back : 'linear-gradient(237.82deg, #ccc, #eee)';
  }
  
  onPageChange(page: number) {
    this.currentPage = page;
    this.getEmployees();
  }

  getEmployees(){
    const { Search, Type } = this.filterForm.value;
    this.staffService.getAllStaff(this.currentPage, this.pageSize, Type, Search).subscribe({
      next: (res) => {
        this.employees = res.data.map((employee:any) => {
                  switch (employee.type) {
                    case 'Doctor':
                      employee.type = 'أطباء';
                      break;
                    case 'Nurse':
                      employee.type = 'ممرضين';
                      break;
                    case 'Admin':
                      employee.type = 'إداريين';
                      break;
                    case 'Staff':
                      employee.type = 'عمال';
                      break;
                    
                  }
                  if (employee.status === 'Active') {
                    employee.status = 'متاح';
                  } else {
                    employee.status = 'غير متاح';
                  }
                  return employee;
        });
        this.pageSize = res.pageSize;
        this.currentPage = res.pageIndex;
        this.total = res.count;
        this.fixed = Math.ceil(this.total / this.pageSize);
        console.log('data', res);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  getCounts(){
    this.staffService.getCounts().subscribe({
      next: (data:any) => {
        this.employeesData = [
            {
              name: 'أطباء',
              count: data.doctorsCount,
              color: 'linear-gradient(237.82deg, #0D6EFD 30.69%, #B6D4FE 105.5%)',
              back: '#0D6EFD'
            },
            {
              name: 'ممرضين',
              count: data.nursesCount,
              color: 'linear-gradient(236.62deg, #20B2AA 30.14%, #A3E4E0 83.62%)',
              back: '#20B2AA'
            },
            {
              name: 'إداريين',
              count: data.administratorsCount,
              color: 'linear-gradient(237.82deg, #FFC107 30.69%, #FFE082 105.5%)',
              back: '#FFC107'
            },
            {
              name: 'عمال',
              count: data.workersCount,
              color: 'linear-gradient(237.82deg, #6C757D 30.69%, #CED4DA 105.5%)',
              back: '#6C757D'
            }
        ];
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
  // 
  getEmployeeById(id: number){
    this.staffService.getStaffById(id).subscribe({
      next: (res) => {
        this.selectedEmployee = res;
        console.log(this.selectedEmployee);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  // 
  editEmployee(){}
  print(){}
  exportToPDF(){}
  susbendEmployee(){}
}
