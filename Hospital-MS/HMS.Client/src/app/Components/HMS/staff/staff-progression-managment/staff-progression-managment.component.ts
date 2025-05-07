import { Component, OnInit } from '@angular/core';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup , Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-staff-progression-managment',
  templateUrl: './staff-progression-managment.component.html',
  styleUrl: './staff-progression-managment.component.css'
})
export class StaffProgressionManagmentComponent implements OnInit {
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
  constructor(private staffService:StaffService , private fb : FormBuilder){}
  ngOnInit(): void {
    this.jobTitleForm=this.fb.group({
      name:['',[Validators.required , Validators.minLength(3)]],
      status:['',Validators.required],
      description:[''],
    })
    this.getJobTitles();
    this.filterForm=this.fb.group({
          SearchText:['']
        });
        this.filterForm
              .get('SearchText')
              .valueChanges.pipe(
                debounceTime(300),
                distinctUntilChanged(),
              )
              .subscribe(() => {
                this.pagingFilterModel.currentPage = 1;
                this.getJobTitles();
              });
  }
  
  getJobTitles(){
    this.staffService.getJobTitles(this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.pagingFilterModel.filterList).subscribe({
      next:(res)=>{
        this.jobTitles=res.results;
        this.total=res.totalCount;
        console.log(this.jobTitles);
      },
      error:(err)=>{
        console.log(err);
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
    this.getJobTitles();
  }
  openjobTitleDetails(id:number){
    this.staffService.getJobTitleById(id).subscribe({
      next:(res)=>{
        this.selectedJobTitle=res.results;
        this.jobTitleForm.patchValue(this.selectedJobTitle);
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }
  addjobTitle(){
    
  }
  editjobTitle(){
    
  }
  onPageChange(event: any) {
    this.pagingFilterModel.currentPage = event.page;
    this.getJobTitles();
  }
}
