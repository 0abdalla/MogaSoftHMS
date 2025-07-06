import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
declare var bootstrap:any;

@Component({
  selector: 'app-stores-types',
  templateUrl: './stores-types.component.html',
  styleUrl: './stores-types.component.css'
})
export class StoresTypesComponent {
  TitleList = ['إعدادات النظام','إعدادات المخازن','أنواع المخازن'];
  filterForm!:FormGroup;
  stotreTypeForm!:FormGroup
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
    this.stotreTypeForm = this.fb.group({
      name: ['', Validators.required],
    });
  }
  ngOnInit(): void {
    this.getStoresTypes();
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
  currentStoreTypeId: number | null = null;
  
  addStoreType() {
    if (this.stotreTypeForm.invalid) {
      this.stotreTypeForm.markAllAsTouched();
      return;
    }
  
    const formData = this.stotreTypeForm.value;
  
    if (this.isEditMode && this.currentStoreTypeId !== null) {
      this.financialService.updateStoreType(this.currentStoreTypeId, formData).subscribe({
        next: () => {
          this.getStoresTypes();
          this.closeModal();
          this.stotreTypeForm.reset();
          this.isEditMode = false;
          this.currentStoreTypeId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addStoreType(formData).subscribe({
        next: () => {
          this.getStoresTypes();
          this.closeModal();
          this.stotreTypeForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  store!:any;
  editStoreType(id: number) {
    this.isEditMode = true;
    this.currentStoreTypeId = id;
  
    this.financialService.getStoresTypesById(id).subscribe({
      next: (data) => {
        this.store=data.results;
        this.stotreTypeForm.patchValue({
          name: this.store.name,
          accountCode: this.store.accountCode,
          branchId: this.store.branchId,
          currency: this.store.currency,
          openingBalance: this.store.openingBalance
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addItemModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات النوع:', err);
      }
    });
  }
  deleteStoreType(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا النوع؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteStoreType(id).subscribe({
          next: () => {
            this.getStoresTypes();
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
    this.getStoresTypes();
  }
  getStoresTypes(){
    this.financialService.getStoresTypes(this.pagingFilterModel).subscribe((res:any)=>{
      this.items=res.results;
      this.total=res.totalCount;
      console.log(this.items);
      this.applyFilters();
    })
  }
  getServiceColor(status: boolean): string {
    return status ? '#198654' : '#dc3545';
  }
  
  getServiceName(status: boolean): string {
    return status ? 'نشط' : 'غير نشط';
  }  
}
