import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-staff-job-management',
  templateUrl: './staff-job-management.component.html',
  styleUrl: './staff-job-management.component.css'
})
export class StaffJobManagementComponent {
  jobTitles:any[]=[];
  total!:number;  
  pagingFilterModel: PagingFilterModel = {
        searchText: '',
        currentPage: 1,
        pageSize: 16,
        filterList: []
  };
  // 
  filterForm!:FormGroup
  jobTitleForm!:FormGroup
  selectedJobTitle:any;
  // 
  jobDeps!:any;
  constructor(private staffService:StaffService , private fb : FormBuilder , private messageService : MessageService){}
  ngOnInit(): void {
    this.jobTitleForm=this.fb.group({
      name:['',[Validators.required , Validators.minLength(3)]],
      status:['Active',Validators.required],
      jobDepartmentId:['' , Validators.required],
      description:[''],
    })
    this.getJobTitles();
    this.getDeps();
    this.filterForm=this.fb.group({
      SearchText:['']
    });
    this.filterForm
      .get('SearchText')
      .valueChanges.pipe(
        debounceTime(300),
        distinctUntilChanged(),
      )
      .subscribe((value:any) => {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.searchText = value
        this.getJobTitles();
    });
  }
  
  getJobTitles(){
    this.staffService.getJobTitles(this.pagingFilterModel.searchText , this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.pagingFilterModel.filterList).subscribe({
      next:(res)=>{
        this.jobTitles=res.results;
        this.total=res.totalCount;
      },
      error:(err)=>{
      }
    })
  }
  // 
  applyFilters(){
    this.pagingFilterModel.searchText = this.filterForm.value.SearchText;
    this.getJobTitles();
  }
  resetFilters(){
    this.filterForm.reset();
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.searchText = '';
    this.getJobTitles();
  }
  openjobTitleDetails(id:number){
    this.staffService.getJobTitleById(id).subscribe({
      next:(res)=>{
        this.selectedJobTitle=res.results;
        this.jobTitleForm.patchValue(this.selectedJobTitle);
      },
      error:(err)=>{
      }
    })
  }
  addjobTitle(){
    this.staffService.addJobTitle(this.jobTitleForm.value).subscribe({
      next:(data:any)=>{
        this.getJobTitles();
        this.jobTitleForm.reset();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم إضافة الوظيفة بنجاح' });
      },
      error:(err)=>{
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء إضافة الوظيفة' });
      }
    })
  }
  editjobTitle(){
    this.staffService.updateJobTitle(this.selectedJobTitle.id , this.jobTitleForm.value).subscribe({
      next:(data:any)=>{
        this.getJobTitles();
        this.jobTitleForm.reset();
        this.messageService.add({ severity: 'success', summary: 'عملية ناجحة', detail: 'تم تعديل الوظيفة بنجاح' });
      },
      error:(err)=>{
        this.messageService.add({ severity: 'error', summary: 'حدث خطأ', detail: 'حدث خطأ أثناء تعديل الوظيفة' });
      }
    })
  }
  onPageChange(event: any) {
    this.pagingFilterModel.currentPage = event.page;
    this.getJobTitles();
  }
  // 
  getDeps(){
    this.staffService.getJobDepartment('', 1 , 100).subscribe({
      next:(data:any)=>{
        this.jobDeps = data.results;
      }
    })
  }
}
