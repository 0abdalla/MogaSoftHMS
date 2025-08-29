import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import { BanksService } from '../../../../../../Services/HMS/banks.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
import { SettingService } from '../../../../../../Services/HMS/setting.service';
import { todayDateValidator } from '../../../../../../validators/today-date.validator';
export declare var bootstrap:any;
import html2pdf from 'html2pdf.js';
import { MessageService } from 'primeng/api';


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
  constructor(private fb:FormBuilder , private financialService:FinancialService , private banksService:BanksService , private settingsService : SettingService , private toastService : MessageService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addNoticeGroup = this.fb.group({
      date: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
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
  restrictionData!:any;
  addAdditionNotification() {
    if (this.addNoticeGroup.invalid) {
      this.addNoticeGroup.markAllAsTouched();
      return;
    }
  
    const formData = this.addNoticeGroup.value;
  
    if (this.isEditMode && this.currentAdditionNotificationId) {
      this.financialService.updateAdditionNotification(this.currentAdditionNotificationId, formData).subscribe({
        next: (res) => {
          this.getAdditionNotifications();
          if(res.isSuccess){
            this.toastService.add({ severity: 'success', summary: 'تم التعديل بنجاح', detail: 'تم تعديل الإشعار بنجاح' });
          }else{
            this.toastService.add({ severity: 'error', summary: 'فشل التعديل', detail: 'فشل تعديل الإشعار' });
          }
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addAdditionNotification(formData).subscribe({
        next: (res) => {
          this.getAdditionNotifications();
          console.log(this.addNoticeGroup.value);
          console.log(res);
          this.restrictionData = res.results
          this.addNoticeGroup.reset();
          if(res.isSuccess){
            this.toastService.add({ severity: 'success', summary: 'تم الحفظ بنجاح', detail: 'تم حفظ الإشعار بنجاح' });
          }else{
            this.toastService.add({ severity: 'error', summary: 'فشل الحفظ', detail: 'فشل حفظ الإشعار' });
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
          this.toastService.add({ severity: 'error', summary: 'فشل الحفظ', detail: 'فشل حفظ الإشعار' });
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
          next: (res:any) => {
            this.getAdditionNotifications();
            if(res.isSuccess){
              this.toastService.add({ severity: 'success', summary: 'تم الحذف بنجاح', detail: 'تم حذف الإشعار بنجاح' });
            }else{
              this.toastService.add({ severity: 'error', summary: 'فشل الحذف', detail: 'فشل حذف الإشعار' });
            }
          },
          error: (err) => {
            console.error('فشل حذف إشعار إضافة:', err);
            this.toastService.add({ severity: 'error', summary: 'فشل الحذف', detail: 'فشل حذف الإشعار' });
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
  // getAccounts(){
  //   this.banksService.getAccounts(this.pagingFilterModel).subscribe((res:any)=>{
  //     this.accounts=res.results;
  //   })
  // }
  // 
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
  printedAddition!:any;
  printAdditionNotification(id: number) {
    this.financialService.getAdditionNotificationsById(id).subscribe(res => {
      if (res?.isSuccess) {
        this.printedAddition = res.results;
        console.log(this.printedAddition);
        
        setTimeout(() => {
          const element = document.getElementById('printableAddition');
          const options = {
            margin:       0.5,
            filename:     `إشعار-إضافة-رقم-${this.printedAddition.id}.pdf`,
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
    this.addNoticeGroup.reset();
    this.isEditMode=false;
    this.currentAdditionNotificationId=null;
  }
}
