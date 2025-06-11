import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-discount-notice',
  templateUrl: './discount-notice.component.html',
  styleUrl: './discount-notice.component.css'
})
export class DiscountNoticeComponent {
  filterForm!:FormGroup;
  discountNoticeGroup!:FormGroup
  // 
  discounts:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  constructor(private fb:FormBuilder,private financialService:FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.discountNoticeGroup = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      bankId: [1],
      accountId: [1],
      checkNumber: [''],
      amount: [0],
      notes: ['']
    });    
    this.getDiscounts();
  }
  getDiscounts(){
    this.financialService.getAllDiscountNotifications(this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.filterForm.value.SearchText,this.filterForm.value.type,this.filterForm.value.responsible).subscribe((res:any)=>{
      this.discounts=res.results;
      console.log('Data',this.discounts);
      this.total=res.total;
    })
  }
  applyFilters(){
    this.getDiscounts();
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
  submitPermission() {
    this.financialService.addDiscountNotification(this.discountNoticeGroup.value).subscribe({
      next: (res: any) => {
        console.log('Discount notification added successfully:', res);
        this.getDiscounts();
      },
      error: (err) => {
        console.error('Error adding discount notification:', err);
      }
    });
    this.discountNoticeGroup.reset();
  }
  deleteDiscount(id:number){
    Swal.fire({
      title: 'هل انت متأكد من الحذف؟',
      text: "لا يمكن استرجاع البيانات المحذوفة!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف البيانات!',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteDiscountNotification(id).subscribe({
          next: (res: any) => {
            console.log('Discount notification deleted successfully:', res);
            this.getDiscounts();
          },
          error: (err) => {
            console.error('Error deleting discount notification:', err);
          }
        });
      }
    });
  }
}
