import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
import { BanksService } from '../../../../../../Services/HMS/banks.service';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
import { SettingService } from '../../../../../../Services/HMS/setting.service';
import { todayDateValidator } from '../../../../../../validators/today-date.validator';
import html2pdf from 'html2pdf.js';
import { MessageService } from 'primeng/api';
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
  // 
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName')
  get today(): string {
    const date = new Date();
    const dateStr = date.toLocaleDateString('ar-EG', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  
    const timeStr = date.toLocaleTimeString('ar-EG', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
  
    return `${dateStr} - الساعة ${timeStr}`;
  }  
  constructor(private fb:FormBuilder , private banksService:BanksService , private financialService:FinancialService , private settingsService : SettingService , private messageService : MessageService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.discountNoticeGroup = this.fb.group({
      date: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
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
        next: (res:any) => {
          this.getDiscountNotifications();
          if(res.isSuccess){
            this.messageService.add({ severity: 'success', summary: 'تم التعديل بنجاح' , detail: 'تم تعديل الإشعار بنجاح' });
          }else{
            this.messageService.add({ severity: 'error', summary: 'فشل التعديل' , detail: 'فشل تعديل الإشعار' });
          }
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
          this.messageService.add({ severity: 'error', summary: 'فشل التعديل' , detail: 'فشل تعديل الإشعار' });
        }
      });
    } else {
      this.financialService.addDebitNotification(formData).subscribe({
        next: (res:any) => {
          this.getDiscountNotifications();
          this.discountNoticeGroup.reset();
          if(res.isSuccess){
            this.messageService.add({ severity: 'success', summary: 'تم الحفظ بنجاح' , detail: 'تم حفظ الإشعار بنجاح' });
          }else{
            this.messageService.add({ severity: 'error', summary: 'فشل الحفظ' , detail: 'فشل حفظ الإشعار' });
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
          this.messageService.add({ severity: 'error', summary: 'فشل الحفظ' , detail: 'فشل حفظ الإشعار' });
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
          next: (res:any) => {
            this.getDiscountNotifications();
            if(res.isSuccess){
              this.messageService.add({ severity: 'success', summary: 'تم الحذف بنجاح' , detail: 'تم حذف الإشعار بنجاح' });
            }else{
              this.messageService.add({ severity: 'error', summary: 'فشل الحذف' , detail: 'فشل حذف الإشعار' });
            }
          },
          error: (err) => {
            console.error('فشل حذف إشعار إضافة:', err);
            this.messageService.add({ severity: 'error', summary: 'فشل الحذف' });
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
  getAccounts() {
    this.settingsService.GetAccountTreeHierarchicalData('').subscribe({
      next: (res: any[]) => {
        this.accounts = this.extractLeafAccounts(res);
        console.log("Accs:", this.accounts);
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
  
  extractLeafAccounts(nodes: any[]): any[] {
    let result: any[] = [];
  
    for (const node of nodes) {
      if (node.isGroup === false) {
        result.push(node);
      }
      if (node.children && node.children.length > 0) {
        result = result.concat(this.extractLeafAccounts(node.children));
      }
    }
    return result;
  }
  // 
  printedDiscount!:any;
  printDiscountNotification(id: number) {
    this.financialService.getDebitNotificationById(id).subscribe(res => {
      if (res?.isSuccess) {
        this.printedDiscount = res.results;
        console.log(this.printedDiscount);
        
        setTimeout(() => {
          const element = document.getElementById('printableDiscount');
          const options = {
            margin:       0.5,
            filename:     `إشعار-خصم-رقم-${this.printedDiscount.id}.pdf`,
            image:        { type: 'jpeg', quality: 0.98 },
            html2canvas:  { scale: 2 },
            jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
          };
          html2pdf().from(element).set(options).save();
        }, 300);
      }
    });
  }
  resetForm(){
    this.discountNoticeGroup.reset();
    this.isEditMode=false;
    this.currentAdditionNotificationId=null;
  }
}
