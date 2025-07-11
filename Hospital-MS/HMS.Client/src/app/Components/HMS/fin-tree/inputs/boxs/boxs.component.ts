import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import Swal from 'sweetalert2';
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
  constructor(private fb:FormBuilder , private financialService:FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.accountForm = this.fb.group({
      accountCode:['' , Validators.required],
      name: ['', Validators.required],
      branchId: ['', Validators.required],
      currency: ['', Validators.required],
      openingBalance: ['', Validators.required]
    });
  }
  ngOnInit(): void {
    this.getTreasuries();
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
          this.closeModal();
          this.accountForm.reset();
          this.isEditMode = false;
          this.currentTreasuryId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addTreasury(formData).subscribe({
        next: () => {
          this.getTreasuries();
          this.closeModal();
          this.accountForm.reset();
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
        this.accountForm.patchValue({
          name: this.account.name,
          accountCode: this.account.accountCode,
          branchId: this.account.branchId,
          currency: this.account.currency,
          openingBalance: this.account.openingBalance
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addItemModal')!);
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
          next: () => {
            this.getTreasuries();
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
}
