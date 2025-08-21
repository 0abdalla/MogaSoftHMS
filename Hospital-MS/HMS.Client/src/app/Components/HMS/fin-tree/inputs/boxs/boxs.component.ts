import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import Swal from 'sweetalert2';
import { StaffService } from '../../../../../Services/HMS/staff.service';
import { MessageService } from 'primeng/api';
export declare var bootstrap:any;
@Component({
  selector: 'app-boxs',
  templateUrl: './boxs.component.html',
  styleUrl: './boxs.component.css'
})
export class BoxsComponent implements OnInit{
  TitleList = ['إعدادات النظام','إعدادات الإدارة المالية','الخزائن'];
  filterForm!:FormGroup;
  accountForm!:FormGroup
  // 
  items:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  isFilter:boolean=true;
  branches:any[]=[];
  constructor(private fb:FormBuilder , private financialService:FinancialService , private staffService : StaffService , private messageService : MessageService){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.accountForm = this.fb.group({
      code:['' , Validators.required],
      name: ['', Validators.required],
      branchId: ['', Validators.required],
      currency: ['', Validators.required],
      openingBalance: ['', Validators.required]
    });
  }
  ngOnInit(): void {
    this.getTreasuries();
    this.getBranches();
  }
  applyFilters(){
    this.total=this.items.length;
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
  currentTreasuryId: number | null = null;
  
  addAccount() {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }
  
    const formData = this.accountForm.value;
  
    if (this.isEditMode && this.currentTreasuryId !== null) {
      this.financialService.updateTreasury(this.currentTreasuryId, formData).subscribe({
        next: () => {
          this.getTreasuries();
          // this.closeModal();
          this.accountForm.reset();
          this.isEditMode = false;
          this.currentTreasuryId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل الوحدة بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addBoxModal');
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
      this.financialService.addTreasury(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          this.getTreasuries();
          // this.closeModal();
          this.accountForm.reset();
          if(res.isSuccess == true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة الخزنة بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة الخزنة',
            });
          } 
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  account!:any;
  editAccount(id: number) {
    this.isEditMode = true;
    this.currentTreasuryId = id;
  
    this.financialService.getTreasuriesById(id).subscribe({
      next: (data) => {
        this.account=data.results;
        console.log(this.account);
        this.accountForm.patchValue({
          name: this.account.name,
          code: this.account.code,
          branchId: this.account.branchId,
          currency: this.account.currency,
          openingBalance: this.account.openingBalance
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addBoxModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الخزنة:', err);
      }
    });
  }
  deleteAccount(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذه الخزنة؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteTreasury(id).subscribe({
          next: (res:any) => {
            this.getTreasuries();
            if(res.isSuccess == true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف الخزنة بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف الخزنة',
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
    const modalElement = document.getElementById('addBoxModal')!;
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
    this.getTreasuries();
  }
  getTreasuries(){
    this.financialService.getTreasuries(this.pagingFilterModel).subscribe((res:any)=>{
      this.items=res.results;
      this.total=res.count;
      console.log(this.items);
      this.applyFilters();
    })
  }
  getBranches() {
    this.staffService.GetBranches({
      searchText: "",
      currentPage: 1,
      pageSize: 16,
      filterList: []
    }).subscribe((res: any) => {
      this.branches = res.results ?? [];
      console.log("Branches:", this.branches);
    });    
  }
  resetFormOnClose() {
    this.accountForm.reset();
    this.isEditMode = false;
    this.currentTreasuryId = null;
  }
}
