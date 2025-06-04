import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-exchange-premssion',
  templateUrl: './exchange-premssion.component.html',
  styleUrl: './exchange-premssion.component.css'
})
export class ExchangePremssionComponent {
  filterForm!:FormGroup;
  exPermissionForm!:FormGroup
  // 
  exchanges:any[]=[];
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
    this.exPermissionForm = this.fb.group({
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
    this.total=this.exchanges.length;
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
    console.log(this.exPermissionForm.value);
    this.exPermissionForm.reset();
  }
}
