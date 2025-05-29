import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-main-group',
  templateUrl: './main-group.component.html',
  styleUrl: './main-group.component.css'
})
export class MainGroupComponent {
  filterForm!:FormGroup;
  mainGroupForm!:FormGroup
  // 
  mainGroups:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  constructor(private fb:FormBuilder){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.mainGroupForm=this.fb.group({
      name:[],
      description:[],
    })
  }
  applyFilters(){
    this.total=this.mainGroups.length;
  }
  resetFilters(){
    this.filterForm.reset();
    this.applyFilters();
  }
  // 
  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.pagingFilterModel.pageSize=event.itemsPerPage;
    this.applyFilters();
  }
  // 
  openMainGroup(id:number){
    
  }
  // 
  addMainGroup(){
    this.mainGroupForm.reset();
  }
}
