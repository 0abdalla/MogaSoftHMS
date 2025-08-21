import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { BanksService } from '../../../../../Services/HMS/banks.service';
import { MessageService } from 'primeng/api';
export declare var bootstrap:any;
@Component({
  selector: 'app-banks',
  templateUrl: './banks.component.html',
  styleUrl: './banks.component.css'
})
export class BanksComponent implements OnInit {
  TitleList = ['إعدادات النظام','إعدادات الإدارة المالية','البنوك'];
  filterForm!:FormGroup;
  accountForm!:FormGroup
  // 
  banks!:any;
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  // 
  isFilter:boolean = true;
  constructor(private fb:FormBuilder , private financialService : BanksService , private messageService : MessageService){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.accountForm = this.fb.group({
      name: ['', Validators.required],
      code: ['', Validators.required],
      accountNumber: ['', Validators.required],
      currency: ['', Validators.required],
      initialBalance: ['', Validators.required]
    });
  }
  ngOnInit():void{
    this.getBanks();
  }
  applyFilters(){
    this.total=this.banks.length;
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
  openItem(id:number){
    
  }
  // 
  isEditMode: boolean = false;
  currentBankId: number | null = null;
  
  addBank() {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }
  
    const formData = this.accountForm.value;
  
    if (this.isEditMode && this.currentBankId !== null) {
      this.financialService.updateBank(this.currentBankId, formData).subscribe({
        next: (res:any) => {
          this.getBanks();
          this.accountForm.reset();
          this.isEditMode = false;
          this.currentBankId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل البنك بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addItemModal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement);
              modalInstance?.hide();
            }
            this.resetFormOnClose();
            }, 1000);
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addBank(formData).subscribe({
        next: (res) => {
          console.log(res);
          console.log(this.accountForm.value);
          
          this.getBanks();
          // this.accountForm.reset();
          if(res.isSuccess == true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة البنك بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة البنك',
            });
          } 
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  Bank!:any;
  editBank(id: number) {
    this.isEditMode = true;
    this.currentBankId = id;
  
    this.financialService.getBankById(id).subscribe({
      next: (data) => {
        this.Bank=data.results;
        this.accountForm.patchValue({
          name: this.Bank.name,
          code: this.Bank.code,
          accountNumber: this.Bank.accountNumber,
          currency: this.Bank.currency,
          initialBalance: this.Bank.initialBalance,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addItemModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات البنك:', err);
      }
    });
  }
  deleteBank(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا البنك؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteBank(id).subscribe({
          next: (res:any) => {
            this.getBanks();
            if(res.isSuccess == true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف البنك بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف البنك',
              });
            }          
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
  closeModal() {
    const modalElement = document.getElementById('addItemModal')!;
    const modalInstance = bootstrap.Modal.getInstance(modalElement);
    modalInstance?.hide();
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
      backdrop.remove();
    }
  
    document.body.classList.remove('modal-open');
    document.body.style.overflow = '';
    document.body.style.paddingRight = '';
  }
  // 
  filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
    this.getBanks();
  }
  getBanks(){
    this.financialService.getBanks(this.pagingFilterModel).subscribe((res:any)=>{
      this.banks=res.results;
      this.total=res.count;
      console.log(this.banks);
      this.applyFilters();
    })
  }
  // 
  resetFormOnClose() {
    this.accountForm.reset();
    this.isEditMode = false;
    this.currentBankId = null;
  }
}
