import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import Swal from 'sweetalert2';
export declare var bootstrap:any;

@Component({
  selector: 'app-add-items',
  templateUrl: './add-items.component.html',
  styleUrl: './add-items.component.css'
})
export class AddItemsComponent implements OnInit {
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
  TitleList = ['المخازن','إذن إستلام'];
  // 
  adds:any[]=[];
  total = 0;
  pagingFilterModel:any={
    searchText: '',
    currentPage: 1,
    pageSize: 1,
    filterList: []
  }
  // 
  allItems: any[] = [];
  suppliers:any[]=[];
  stores:any[]=[];
  purchaseRequests:any[]=[];
  // 
  isFilter:boolean=true;
  constructor(private fb:FormBuilder , private financialService : FinancialService , private cdr : ChangeDetectorRef){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      documentNumber: ['1'],
      permissionDate: [new Date().toISOString().substring(0, 10)],
      notes: [''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      storeId: [''],
      supplierId: [''],
      purchaseRequestId: [''],
    });    
  }
  ngOnInit(): void {
    this.getReceiptPermissions();
    this.getItems();
    this.getSuppliers();
    this.getStores();
    this.getPurchaseRequests();
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      id: [null, Validators.required],
      unit: [null, Validators.required],
      quantity: [1, Validators.required],
      unitPrice: [1, Validators.required],
      totalPrice: [''],
      notes: ['']
    });
  }
  get items(): FormArray {
      return this.addPermissionForm.get('items') as FormArray;
    }
    
  addItemRow() {
      this.items.push(this.createItemGroup());
    }
    
  removeItemRow(index: number) {
      if (this.items.length > 1) {
        this.items.removeAt(index);
      }
    }
  applyFilters(){
    this.total=this.adds.length;
  }
  resetFilters(){
    this.filterForm.reset();
    this.applyFilters();
  }
  
  // 
  openMainGroup(id:number){
    
  }
  // 
  getReceiptPermissions() {
    console.log('Sending to API:', this.pagingFilterModel);
    this.financialService.getReceiptPermissions(this.pagingFilterModel).subscribe({
      next: (res:any) => {
        this.total = res.totalCount;
        this.adds = res.results;
        this.cdr.detectChanges();
      
      console.log('Current Page:', this.pagingFilterModel.currentPage);
      console.log('Page Size:', this.pagingFilterModel.pageSize);
      console.log('Total Items:', this.total);
      console.log('Results:', this.adds);
      },
      error: (err) => {
        console.error('فشل جلب إذن الإستلام:', err);
      }
    });
  }
  // 
  onPageChange(page: any) {
    console.log('Page changed to:', page);
    this.pagingFilterModel.currentPage = page.page;
    this.getReceiptPermissions();
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res:any)=>{
      this.allItems=res.results;
      // console.log(this.allItems);
      this.total=res.count;
    })
  }
  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModel).subscribe((res:any)=>{
      this.suppliers=res.results;
      // console.log(this.suppliers);
      this.total=res.count;
    })
  }
  getStores(){
    this.financialService.getStores(this.pagingFilterModel).subscribe((res:any)=>{
      this.stores=res.results;
      // console.log(this.stores);
      this.total=res.count;
    })
  }
  getPurchaseRequests(){
    this.financialService.getPurchaseRequests(this.pagingFilterModel).subscribe((res:any)=>{
      this.purchaseRequests=res.results;
      // console.log(this.purchaseRequests);
      this.total=res.count;
    })
  }
  filterChecked(filters: FilterModel[]) {
          this.pagingFilterModel.currentPage = 1;
          this.pagingFilterModel.filterList = filters;
          if (filters.some(i => i.categoryName == 'SearchText'))
            this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
          else
            this.pagingFilterModel.searchText = '';
          this.getItems();
  }
  // 
  isEditMode : boolean = false;
  currentPurchaseRequestId: number | null = null;
  addPermission() {
    if (this.addPermissionForm.invalid) {
      this.addPermissionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addPermissionForm.value;
  
    if (this.isEditMode && this.currentPurchaseRequestId) {
      this.financialService.updateReceiptPermission(this.currentPurchaseRequestId, formData).subscribe({
        next: () => {
          this.getReceiptPermissions();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addReceiptPermission(formData).subscribe({
        next: () => {
          this.getReceiptPermissions();
          this.addPermissionForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  permission!:any;
  editPermission(id: number) {
    this.isEditMode = true;
    this.currentPurchaseRequestId = id;
  
    this.financialService.getReceiptPermissionsById(id).subscribe({
      next: (data) => {
        this.permission=data.results;
        console.log(this.permission);
        this.addPermissionForm.patchValue({
          supplierId: this.permission.supplierId,
          documentNumber: this.permission.documentNumber,
          permissionDate: this.permission.permissionDate,
          notes: this.permission.notes,
          items: this.permission.items,
          storeId: this.permission.storeId,
          purchaseRequestId: this.permission.purchaseRequestId
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addPermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
      }
    });
  }
  deletePermission(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف طلب الشراء؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteReceiptPermission(id).subscribe({
          next: () => {
            this.getReceiptPermissions();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
}
