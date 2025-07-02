import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
export declare var bootstrap:any;

@Component({
  selector: 'app-issue-items',
  templateUrl: './issue-items.component.html',
  styleUrl: './issue-items.component.css'
})
export class IssueItemsComponent implements OnInit {
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
   TitleList = ['المخازن','إذن صرف'];
  // 
  adds:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  isFilter:boolean=true;
  allItems: any[] = [];
  suppliers:any[]=[];
  stores:any[]=[];
  purchaseRequests:any[]=[];

  constructor(private fb:FormBuilder , private financialService:FinancialService){

    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      permissionDate: [new Date().toISOString().substring(0, 10)],
      documentNumber: ['0'],
      storeId: [''],
      branchId: ['1'],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      notes: ['']
    });    
  }
  ngOnInit(): void {
    this.getItems();
    this.getMaterialIssuePermissions();
    this.getSuppliers();
    this.getStores();
    this.getPurchaseRequests();
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      unit: [1, Validators.required],
      quantity: [1, Validators.required],
      unitPrice: [1, Validators.required],
      totalPrice: [1, Validators.required]
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
  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.pagingFilterModel.pageSize=event.itemsPerPage;
    this.applyFilters();
  }
  // 
  openMainGroup(id:number){
    
  }
  // 
  getMaterialIssuePermissions(){
    this.financialService.getMaterialIssuePermissions(this.pagingFilterModel).subscribe((res:any)=>{
      this.adds=res.results;
      console.log(this.adds);
      this.total=res.count;
      this.applyFilters();
    })
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res:any)=>{
      this.allItems=res.results;
      console.log(this.allItems);
      this.total=res.count;
    })
  }
  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModel).subscribe((res:any)=>{
      this.suppliers=res.results;
      console.log(this.suppliers);
      this.total=res.count;
    })
  }
  getStores(){
    this.financialService.getStores(this.pagingFilterModel).subscribe((res:any)=>{
      this.stores=res.results;
      console.log(this.stores);
      this.total=res.count;
    })
  }
  getPurchaseRequests(){
    this.financialService.getPurchaseRequests(this.pagingFilterModel).subscribe((res:any)=>{
      this.purchaseRequests=res.results;
      console.log(this.purchaseRequests);
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
  currentMaterialIssuePermissionId: number | null = null;
  addPermission() {
    if (this.addPermissionForm.invalid) {
      this.addPermissionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addPermissionForm.value;
    if (this.isEditMode && this.currentMaterialIssuePermissionId) {
      
      this.financialService.updateMaterialIssuePermission(this.currentMaterialIssuePermissionId, formData).subscribe({
        next: () => {
          this.getMaterialIssuePermissions();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addMaterialIssuePermission(formData).subscribe({
        next: () => {
          this.getMaterialIssuePermissions();
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
    this.currentMaterialIssuePermissionId = id;
  
    this.financialService.getMaterialIssuePermissionsById(id).subscribe({
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
        this.financialService.deleteMaterialIssuePermission(id).subscribe({
          next: () => {
            this.getMaterialIssuePermissions();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
}
