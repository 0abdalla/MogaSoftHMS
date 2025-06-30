import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import { BanksService } from '../../../../../../Services/HMS/banks.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
export declare var bootstrap:any;

@Component({
  selector: 'app-add-notice',
  templateUrl: './add-notice.component.html',
  styleUrl: './add-notice.component.css'
})
export class AddNoticeComponent {
  TitleList = ['الإدارة المالية','حركة البنك','إشعار إضافة'];
  filterForm!:FormGroup;
  addNoticeGroup!:FormGroup
  // 
  additionNotifications:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  banks:any[]=[];
  accounts:any[]=[]
  // 
  isEditMode:boolean=false;
  currentAdditionNotificationId:number|null=null;
  // 
  isFilter:boolean=true;
  constructor(private fb:FormBuilder , private financialService:FinancialService , private banksService:BanksService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addNoticeGroup = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      bankId: ['' , Validators.required],
      accountId: ['' , Validators.required],
      checkNumber: ['' , Validators.required],
      amount: [0 , Validators.required],
      notes: ['']
    });    
    this.getAdditionNotifications();
    this.getBanks();
    this.getAccounts();
  }
  applyFilters(){
    this.total=this.additionNotifications.length;
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
  addAdditionNotification() {
    if (this.addNoticeGroup.invalid) {
      this.addNoticeGroup.markAllAsTouched();
      return;
    }
  
    const formData = this.addNoticeGroup.value;
  
    if (this.isEditMode && this.currentAdditionNotificationId) {
      this.financialService.updateAdditionNotification(this.currentAdditionNotificationId, formData).subscribe({
        next: () => {
          this.getAdditionNotifications();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addAdditionNotification(formData).subscribe({
        next: () => {
          this.getAdditionNotifications();
          this.addNoticeGroup.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  order!:any;
  editAdditionNotification(id: number) {
    this.isEditMode = true;
    this.currentAdditionNotificationId = id;
  
    this.financialService.getAdditionNotificationsById(id).subscribe({
      next: (data) => {
        this.order=data.results;
        console.log(this.order);
        this.addNoticeGroup.patchValue({
          date: this.order.date,
          bankId: this.order.bankId,
          accountId: this.order.accountId,
          checkNumber: this.order.checkNumber,
          amount: this.order.amount,
          notes: this.order.notes
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addnoticePermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات إشعار إضافة:', err);
      }
    });
  }
  deleteAdditionNotification(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف إشعار إضافة؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteAdditionNotification(id).subscribe({
          next: () => {
            this.getAdditionNotifications();
          },
          error: (err) => {
            console.error('فشل حذف إشعار إضافة:', err);
          }
        });
      }
    });
  }
  getAdditionNotifications(){
    this.financialService.getAdditionNotifications(this.pagingFilterModel).subscribe((res:any)=>{
      this.additionNotifications=res.results;
      this.total=res.count;
      this.applyFilters();
    })
  }
  // 
  filterChecked(filters: FilterModel[]) {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.filterList = filters;
        if (filters.some(i => i.categoryName == 'SearchText'))
          this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
        else
          this.pagingFilterModel.searchText = '';
        this.getAdditionNotifications();
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
