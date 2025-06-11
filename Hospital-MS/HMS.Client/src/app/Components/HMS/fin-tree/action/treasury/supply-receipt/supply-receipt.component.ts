import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-supply-receipt',
  templateUrl: './supply-receipt.component.html',
  styleUrl: './supply-receipt.component.css'
})
export class SupplyReceiptComponent {
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
  // 
  supplyReceipts:any[]=[];
  selectedSupplyReceipt:any;
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
    this.addPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      treasuryId: [''],
      receivedFrom: [''],
      accountCode: [''],
      amount: [''],
      description: [''],
      costCenterId: [1],
    });    
    this.getSupplyReceipts();
  }
  getSupplyReceipts(){
    this.financialService.getSupplyReceipts(this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize).subscribe((res:any)=>{
      this.supplyReceipts=res.results;
      console.log(this.supplyReceipts);
      this.total=res.total;
    })
  }
  applyFilters(){
    this.total=this.supplyReceipts.length;
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
    this.financialService.getSupplyReceiptById(id).subscribe((res:any)=>{
      console.log(res.results);
      this.selectedSupplyReceipt=res.results.id;
      console.log(this.selectedSupplyReceipt);
      this.addPermissionForm.patchValue(res.results);
    })
  }
  // 
  submitPermission(){
    this.financialService.addSupplyReceipt(this.addPermissionForm.value).subscribe((res:any)=>{
      console.log(res);
      this.getSupplyReceipts();
    })
    this.addPermissionForm.reset();
  }
  updateSupplyReceipt(){
    this.financialService.updateSupplyReceipt(this.selectedSupplyReceipt,this.addPermissionForm.value).subscribe((res:any)=>{
      console.log(res);
      this.getSupplyReceipts();
    })
    this.addPermissionForm.reset();
  }
  deleteSupplyReceipt(id:number){
    Swal.fire({
      title: 'هل انت متأكد من الحذف؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذفه!',
      cancelButtonText: 'الغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteSupplyReceipt(id).subscribe((res:any)=>{
          console.log(res);
          this.getSupplyReceipts();
        })
      }
    })
  }
}
