import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-fetch-inventory',
  templateUrl: './fetch-inventory.component.html',
  styleUrl: './fetch-inventory.component.css'
})
export class FetchInventoryComponent {
  filterForm!:FormGroup;
  fetchInventoryForm!:FormGroup
  // 
  fetchInventories:any[]=[];
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
    this.fetchInventoryForm = this.fb.group({
      dateFrom: [new Date().toISOString().substring(0, 10)],
      dateTo: [new Date().toISOString().substring(0, 10)],
      InventoryName: [''],
    });    
  }
  applyFilters(){
    this.total=this.fetchInventories.length;
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
    console.log(this.fetchInventoryForm.value);
    this.fetchInventoryForm.reset();
  }
}
