import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-issue-items',
  templateUrl: './issue-items.component.html',
  styleUrl: './issue-items.component.css'
})
export class IssueItemsComponent {
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
   TitleList = ['المخازن','إذن صرف'];
  // 
  adds:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  constructor(private fb:FormBuilder){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      documentNumber: ['0'],
      warehouseName: [''],
      supplierName: [''],
      itemNumber: [''],
      quantity: [1],
      balance: [0],
      notes: ['']
    });    
  }
  applyFilters(){
    this.total=this.adds.length;
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
  submitPermission(){
    console.log(this.addPermissionForm.value);
    this.addPermissionForm.reset();
  }
}
