import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
import { BanksService } from '../../../../../../Services/HMS/banks.service';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
declare var bootstrap:any;

@Component({
  selector: 'app-discount-notice',
  templateUrl: './discount-notice.component.html',
  styleUrl: './discount-notice.component.css'
})
export class DiscountNoticeComponent implements OnInit {
   TitleList = ['الإدارة المالية','حركة البنك','إشعار خصم'];
  filterForm!:FormGroup;
  discountNoticeGroup!:FormGroup
  // 
  discounts:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  // 
  isFilter:boolean=true;
  banks!:any[]
  accounts!:any[]
  constructor(private fb:FormBuilder , private banksService:BanksService , private financialService:FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.discountNoticeGroup = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      bankId: ['' , Validators.required],
      accountId: ['' , Validators.required],
      checkNumber: ['' , Validators.required],
      amount: [0 , Validators.required],
      notes: ['']
    });    
  }
  ngOnInit(){
    this.getDiscountNotifications();
    this.getBanks();
    this.getAccounts();
  }
  applyFilters(){
    this.total=this.discounts.length;
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
  getDiscountNotifications(){
    this.financialService.getDebitNotification(this.pagingFilterModel).subscribe((res:any)=>{
      this.discounts=res.results;
      this.total=res.count;
      this.applyFilters();
    })
  }
  // 
  isEditMode:boolean=false;
  currentAdditionNotificationId:number|null=null;
  addDiscountNotification() {
    if (this.discountNoticeGroup.invalid) {
      this.discountNoticeGroup.markAllAsTouched();
      return;
    }
  
    const formData = this.discountNoticeGroup.value;
  
    if (this.isEditMode && this.currentAdditionNotificationId) {
      this.financialService.updateDebitNotification(this.currentAdditionNotificationId, formData).subscribe({
        next: () => {
          this.getDiscountNotifications();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addDebitNotification(formData).subscribe({
        next: () => {
          this.getDiscountNotifications();
          this.discountNoticeGroup.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  order!:any;
  editDiscountNotification(id: number) {
    this.isEditMode = true;
    this.currentAdditionNotificationId = id;
  
    this.financialService.getDebitNotificationById(id).subscribe({
      next: (data) => {
        this.order=data.results;
        console.log(this.order);
        this.discountNoticeGroup.patchValue({
          date: this.order.date,
          bankId: this.order.bankId,
          accountId: this.order.accountId,
          checkNumber: this.order.checkNumber,
          amount: this.order.amount,
          notes: this.order.notes
        });
  
        const modal = new bootstrap.Modal(document.getElementById('discountNoticePermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات إشعار خصم:', err);
      }
    });
  }
  deleteDiscountNotification(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف إشعار خصم؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteDebitNotification(id).subscribe({
          next: () => {
            this.getDiscountNotifications();
          },
          error: (err) => {
            console.error('فشل حذف إشعار إضافة:', err);
          }
        });
      }
    });
  }
  // 
  filterChecked(filters: FilterModel[]) {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.filterList = filters;
        if (filters.some(i => i.categoryName == 'SearchText'))
          this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
        else
          this.pagingFilterModel.searchText = '';
        this.getDiscountNotifications();
      }
  // 
  getBanks(){
    this.banksService.getBanks(this.pagingFilterModel).subscribe((res:any)=>{
      this.banks=res.results;
    })
  }
  getAccounts(){
    this.banksService.getAccounts(this.pagingFilterModel).subscribe((res:any)=>{
      this.accounts=res.results;
    })
  }
}
