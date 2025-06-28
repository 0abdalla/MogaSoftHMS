import { Component, OnInit } from '@angular/core';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-staff-levels',
  templateUrl: './staff-levels.component.html',
  styleUrl: './staff-levels.component.css'
})
export class StaffLevelsComponent implements OnInit {
  TitleList = ['الموارد البشرية','بيانات الموظفين','المستويات الوظيفية'];
  jobLevels!:any;
  pagingFilterModel: PagingFilterModel = {
        searchText: '',
        currentPage: 1,
        pageSize: 16,
        filterList: []
  };
  total!:number;
  // 
  filterForm!:FormGroup;
  jobLevelForm!:FormGroup;
  // 
  selectedJobLevel!:any;
  constructor(private staffService : StaffService , private fb:FormBuilder , private messageservice : MessageService){}
  ngOnInit(): void {
    this.getJobLevels();
    this.filterForm = this.fb.group({
      SearchText: [''],
    });
    this.filterForm.get('SearchText')?.valueChanges.pipe(
          debounceTime(300),
          distinctUntilChanged()
        ).subscribe((value: string) => {
          this.pagingFilterModel.currentPage = 1;
          this.pagingFilterModel.searchText = value
          this.getJobLevels();
        });
    this.jobLevelForm = this.fb.group({
      name: ['' , [Validators.required , Validators.minLength(3)]],
      status: ['Active' , Validators.required],
      description: [''],
    });
  }
  getJobLevels(){
    this.staffService.getJobLevels( this.pagingFilterModel.searchText ,this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.pagingFilterModel.filterList).subscribe((res:any)=>{
      this.jobLevels=res.results;
      
    })
  }
  // 
  applyFilters(){
    this.getJobLevels();
  }
  resetFilters(){
    this.filterForm.reset();
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.searchText = '';
    this.getJobLevels();
  }
  // 
  openjobLevelDetails(id:number){
    this.staffService.getJobLevelById(id).subscribe({
      next:(res)=>{
        this.selectedJobLevel=res.results;
        this.jobLevelForm.patchValue(this.selectedJobLevel);
      },
      error:(err)=>{
      }
    })
  }
  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.getJobLevels();
  }
  // 
  addjobLevel(){
    this.staffService.addJobLevel(this.jobLevelForm.value).subscribe({
      next:(res)=>{
        this.jobLevelForm.reset();
        this.getJobLevels();
        this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة المستوى بنجاح' });
      },
      error:(err)=>{
        this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة المستوى' });
      }
    })
  }
  editjobLevel(){
    this.staffService.updateJobLevel(this.selectedJobLevel.id,this.jobLevelForm.value).subscribe({
      next:(res)=>{
        this.jobLevelForm.reset();
        this.getJobLevels();
        this.messageservice.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل المستوى بنجاح' });
      },
      error:(err)=>{
        this.messageservice.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل المستوى' });
      }
    })
  }
}
