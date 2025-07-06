import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
export declare var bootstrap:any;

@Component({
  selector: 'app-stores',
  templateUrl: './stores.component.html',
  styleUrl: './stores.component.css'
})
export class StoresComponent {
  TitleList = ['إعدادات النظام','إعدادات المخازن','المخازن'];
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
      code:['' , Validators.required],
      name: ['', Validators.required],
      storeTypeId: ['', Validators.required]
    });
  }
  ngOnInit(): void {
    this.getStores();
    this.getStoreTypes();
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
  currentStoreId: number | null = null;
  
  addStore() {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }
  
    const formData = this.accountForm.value;
  
    if (this.isEditMode && this.currentStoreId !== null) {
      this.financialService.updateStore(this.currentStoreId, formData).subscribe({
        next: () => {
          this.getStores();
          this.closeModal();
          this.accountForm.reset();
          this.isEditMode = false;
          this.currentStoreId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addStore(formData).subscribe({
        next: () => {
          this.getStores();
          this.closeModal();
          this.accountForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  store!:any;
  editStore(id: number) {
    this.isEditMode = true;
    this.currentStoreId = id;
  
    this.financialService.getStoresById(id).subscribe({
      next: (data) => {
        this.store=data.results;
        this.accountForm.patchValue({
          name: this.store.name,
          code: this.store.code,
          storeTypeId: this.store.storeTypeId,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addItemModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الخزنة:', err);
      }
    });
  }
  deleteStore(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا المخزن؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteStore(id).subscribe({
          next: () => {
            this.getStores();
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
    this.getStores();
  }
  getStores(){
    this.financialService.getStores(this.pagingFilterModel).subscribe((res:any)=>{
      this.items=res.results;
      this.total=res.count;
      console.log(this.items);
      this.applyFilters();
    })
  }
  storeTypes!:any[]
  getStoreTypes(){
    this.financialService.getStoresTypes(this.pagingFilterModel).subscribe((res:any)=>{
      this.storeTypes=res.results;
      this.total=res.totalCount;
      console.log(this.items);
      this.applyFilters();
    })
  }
}
