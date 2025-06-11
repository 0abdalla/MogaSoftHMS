import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';

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
  constructor(private fb:FormBuilder , private financialService:FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.exPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      fromStoreId: ['' , Validators.required],
      toStoreId: ['' , Validators.required],
      quantity: ['' , Validators.required],
      itemId: ['' , Validators.required],
      notes: ['']
    });    
    this.getExchanges();
  }
  getExchanges(){
    this.financialService.getDispensePermission(this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize).subscribe((res:any)=>{
      console.log(res);
      this.exchanges=res.results;
      console.log(this.exchanges);
      this.applyFilters();
    })
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
    this.financialService.addDispensePermission(this.exPermissionForm.value).subscribe((res:any)=>{
      console.log(res);
      this.getExchanges();
    })
    this.exPermissionForm.reset();
  }
}
