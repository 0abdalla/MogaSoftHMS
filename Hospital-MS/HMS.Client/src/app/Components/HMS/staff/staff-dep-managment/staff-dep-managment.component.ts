import { Component, OnInit } from '@angular/core';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-staff-dep-managment',
  templateUrl: './staff-dep-managment.component.html',
  styleUrl: './staff-dep-managment.component.css'
})
export class StaffDepManagmentComponent implements OnInit {
  TitleList = ['الموارد البشرية','بيانات الموظفين','الأقسام'];
  filterForm!:FormGroup;
  jobDepsForm!:FormGroup
  // 
  jobDeps!:any;
  jobDepartment!:any;
  // 
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  };
  total!:number;
  constructor(private staffService : StaffService , private fb : FormBuilder , private messageService : MessageService){
    this.filterForm = this.fb.group({
      SearchText: ['']
    });
    this.jobDepsForm=this.fb.group({
      name:['',[Validators.required , Validators.minLength(3)]],
      status:['Active',Validators.required],
      description:[''],
    })
    this.filterForm.get('SearchText')?.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe((value: string) => {
      this.pagingFilterModel.currentPage = 1;
      this.pagingFilterModel.searchText = value
      this.getDeps();
    });
  }    
  ngOnInit(): void {
    this.getDeps();
  }
  getDeps(){
    this.staffService.getJobDepartment(this.pagingFilterModel.searchText ,this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.pagingFilterModel.filterList).subscribe({
      next:(data:any)=>{
        this.jobDeps = data.results;
        this.total = data.totalCount;
      },error:(err)=>{
      }
    })
  }
  opendepDetails(id:number){
    this.staffService.getJobDepartmentById(id).subscribe({
      next:(data:any)=>{
        this.jobDepartment = data.results;
        this.jobDepsForm.patchValue(this.jobDepartment)
      }
    })
  }
  applyFilters(){
    this.pagingFilterModel.currentPage = 1;
    this.getDeps();
  }
  resetFilters(){
    this.filterForm.reset();
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.searchText = '';
    this.getDeps();
  }
  onPageChange(page: number) {
    this.pagingFilterModel.currentPage = page;
    this.getDeps();
  }
  // 
  addDep(){
    this.staffService.addJobDepartment(this.jobDepsForm.value).subscribe({
      next:(data:any)=>{
        this.getDeps();
        this.jobDepsForm.reset();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة الفئة بنجاح' });
      },error:(err)=>{
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة القسم' });

      }
    })
  }
  editDep(){
    this.staffService.updateJobDeprtment(this.jobDepartment.id,this.jobDepsForm.value).subscribe({
      next:(data:any)=>{
        this.getDeps();
        this.jobDepsForm.reset();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل القسم بنجاح' });

      },error:(err)=>{
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل القسم' });

      }
    })
  }
}
