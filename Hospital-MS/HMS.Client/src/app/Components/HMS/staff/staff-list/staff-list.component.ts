import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';

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

export class StaffListComponent implements OnInit, OnDestroy {
  employees: any[] = [];
  employeesData: any[] = [];
  filterForm: FormGroup;
  pageSize = 16;
  currentPage = 1;
  total = 0;
  fixed = 0;
  selectedEmployee: any;

  private destroy$ = new Subject<void>();

  constructor(private staffService: StaffService, private fb: FormBuilder) {
    this.filterForm = this.fb.group({
      Search: [''],
      Type: ['']
    });
  }

  ngOnInit(): void {
    // this.getCounts();
    this.setupRealTimeSearch();
    this.getEmployees();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  setupRealTimeSearch() {
    this.filterForm.get('Search')?.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.currentPage = 1;
        this.getEmployees();
      });

    this.filterForm.get('Type')?.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.currentPage = 1;
        this.getEmployees();
      });
  }

  applyFilters() {
    this.currentPage = 1;
    this.getEmployees();
  }

  resetFilters() {
    this.filterForm.reset({ Search: '', Type: '' });
    this.currentPage = 1;
    this.getEmployees();
  }

  openStaffModal(id: number) {
    this.getEmployeeById(id);
  }

  getEmployeeColor(type: string): string {
    if (!this.employeesData || !type) {
      return 'linear-gradient(237.82deg, #ccc, #eee)';
    }
    const item = this.employeesData.find(e => e.name === type);
    return item ? item.back : 'linear-gradient(237.82deg, #ccc, #eee)';
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.getEmployees();
  }

  getEmployees() {
    const { Search, Type } = this.filterForm.value;
    const mappedType = this.mapType(Type);
    this.staffService.getAllStaff(this.currentPage, this.pageSize, Search, mappedType, undefined, undefined, []).subscribe({
      next: (res) => {
        const results = Array.isArray(res?.results) ? res.results : [];
        this.employees = results.map((employee: any) => {
          switch (employee.type) {
            case 'ممرض':
              employee.type = 'ممرضين';
              break;
            case 'اداري':
              employee.type = 'إداريين';
              break;
            case 'عامل':
              employee.type = 'عمال';
              break;
            default:
              employee.type = employee.type || 'غير محدد';
          }
          if (employee.status === 'نشط') {
            employee.status = 'متاح';
          } else {
            employee.status = 'غير متاح';
          }
          return employee;
        });
        this.total = res?.totalCount || 0;
        this.fixed = Math.ceil(this.total / this.pageSize) || 1;
        console.log('employees data', res, 'mapped employees', this.employees);
      },
      error: (err) => {
        console.error('GetEmployees Error:', err);
        this.employees = [];
        this.total = 0;
        this.fixed = 1;
      }
    });
  }

  getCounts() {
    this.staffService.getCounts().subscribe({
      next: (data: any) => {
        const results = Array.isArray(data?.results) ? data.results : [];
        this.employeesData = [
          {
            name: 'أطباء',
            count: results.find((r: any) => r.type === 'Doctor')?.count || 0,
            color: 'linear-gradient(237.82deg, #0D6EFD 30.69%, #B6D4FE 105.5%)',
            back: '#0D6EFD'
          },
          {
            name: 'ممرضين',
            count: results.find((r: any) => r.type === 'Nurse')?.count || 0,
            color: 'linear-gradient(236.62deg, #20B2AA 30.14%, #A3E4E0 83.62%)',
            back: '#20B2AA'
          },
          {
            name: 'إداريين',
            count: results.find((r: any) => r.type === 'Administrator')?.count || 0,
            color: 'linear-gradient(237.82deg, #FFC107 30.69%, #FFE082 105.5%)',
            back: '#FFC107'
          },
          {
            name: 'عمال',
            count: results.find((r: any) => r.type === 'Worker')?.count || 0,
            color: 'linear-gradient(237.82deg, #6C757D 30.69%, #CED4DA 105.5%)',
            back: '#6C757D'
          }
        ];
        console.log('counts data', this.employeesData, 'raw response', data);
      },
      error: (err) => {
        console.error('GetCounts Error:', err);
        this.employeesData = [
          { name: 'أطباء', count: 0, color: '...', back: '#0D6EFD' },
          { name: 'ممرضين', count: 0, color: '...', back: '#20B2AA' },
          { name: 'إداريين', count: 0, color: '...', back: '#FFC107' },
          { name: 'عمال', count: 0, color: '...', back: '#6C757D' }
        ];
      }
    });
  }

  getEmployeeById(id: number) {
    this.staffService.getStaffById(id).subscribe({
      next: (res) => {
        this.selectedEmployee = res;
        console.log('selected employee', this.selectedEmployee);
      },
      error: (err) => {
        console.error('GetEmployeeById Error:', err);
      }
    });
  }

  mapType(type: string): string | undefined {
    const typeMap: { [key: string]: string } = {
      'Doctor': 'أطباء',
      'Nurse': 'ممرض',
      'Staff': 'اداري',
      'Worker': 'عامل'
    };
    return typeMap[type] || undefined;
  }

  editEmployee() {
    if (this.selectedEmployee) {
      console.log('Editing employee:', this.selectedEmployee);
    }
  }

  print() {
    window.print();
  }

  exportToPDF() {
    console.log('Exporting to PDF...');
  }

  suspendEmployee() {
    if (this.selectedEmployee) {
      console.log('Suspending employee:', this.selectedEmployee);
    }
  }
}