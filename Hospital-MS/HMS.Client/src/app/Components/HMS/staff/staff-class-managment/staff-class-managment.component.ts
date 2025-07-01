import { Component, OnInit } from '@angular/core';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-staff-class-managment',
  templateUrl: './staff-class-managment.component.html',
  styleUrl: './staff-class-managment.component.css'
})
export class StaffClassManagmentComponent implements OnInit {
  TitleList = ['الإدارة المالية', 'حركة الخزينة', 'إغلاق حركة الخزينة'];
  jobTypes: any[] = [];
  total!: number;
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  filterForm!: FormGroup;
  jobTypeForm!: FormGroup;
  selectedJobType: any;
  destroy$: Subject<void> = new Subject<void>();
  isFilter = true;
  constructor(private staffService: StaffService, private fb: FormBuilder, private messageservice: MessageService) {

  }
  ngOnInit(): void {
    this.filterForm = this.fb.group({
      SearchText: ['']
    });
    this.filterForm
      .get('SearchText')
      .valueChanges.pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe((value: any) => {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.searchText = value
        this.getTypes();
      });
    this.jobTypeForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      status: ['Active', Validators.required],
      description: [''],
    })
    this.getTypes();
  }
  getTypes() {
    this.staffService.getJobTypes(this.pagingFilterModel.searchText, this.pagingFilterModel.currentPage, this.pagingFilterModel.pageSize, this.pagingFilterModel.filterList).subscribe({
      next: (res) => {
        this.jobTypes = res.results;
        this.total = res.totalCount;
      },
      error: (err) => {
      }
    })
  }
  applyFilters(filters: FilterModel[]) {
    this.pagingFilterModel.filterList = filters;
    this.pagingFilterModel.currentPage = 1;
    this.getTypes();
  }

  openjobTypeDetails(id: number) {
    this.staffService.getJobTypeById(id).subscribe({
      next: (res) => {
        this.selectedJobType = res.results;
        this.jobTypeForm.patchValue(this.selectedJobType);
      },
      error: (err) => {
      }
    })
  }
  onPageChange(event: any) {
    this.pagingFilterModel.currentPage = event.page;
    this.getTypes();
  }
  // 
  addJobType() {
    this.staffService.addJobType(this.jobTypeForm.value).subscribe({
      next: (res) => {
        this.jobTypeForm.reset();
        this.getTypes();
        this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة الفئة بنجاح' });
      },
      error: (err) => {
        this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة الفئة' });
      }
    })
  }
  editJobType() {
    this.staffService.updateJobType(this.selectedJobType.id, this.jobTypeForm.value).subscribe({
      next: (res) => {
        this.jobTypeForm.patchValue(res.results);
        this.getTypes();
        this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل الفئة بنجاح' });
      },
      error: (err) => {
        this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل الفئة' });
      }
    })
  }
}
