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
  pagingFilterModelSelect:any={
    pageSize:100,
    currentPage:1,
  }
  isFilter:boolean=true;
  allItems: any[] = [];
  suppliers:any[]=[];
  stores:any[]=[];
  purchaseRequests:any[]=[];
  branchs:any[] = [];
  allRequests:any[]=[]
  constructor(private fb:FormBuilder , private financialService:FinancialService){

    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      permissionDate: [new Date().toISOString().substring(0, 10)],
      disbursementRequestId: [''],
      storeId: [''],
      branchId: [''],
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
    // this.getPurchaseRequests();
    this.getBranches();
    this.getAllRequests();

    // 
    this.addPermissionForm.get('disbursementRequestId')?.valueChanges.subscribe(id => {
      if (id) {
        this.financialService.getIssueRequestById(id).subscribe(res => {
          const data = res.results;
          this.addPermissionForm.patchValue({
            permissionDate: data.date,
            notes: data.notes
          });
          const itemsControl = this.items;
          itemsControl.clear();
          data.items.forEach((item: any) => {
            itemsControl.push(this.createItemGroup(item));
          });
        });
      }
    });
    
  }
  createItemGroup(item: any = null): FormGroup {
    const unitPriceValue = item?.priceAfterTax ?? 1;
    const quantityValue = item?.quantity ?? 1;
    const group = this.fb.group({
      itemId: [item?.itemId ?? '', Validators.required],
      unit: [item?.unit ?? '', Validators.required],
      quantity: [quantityValue, [Validators.required , Validators.min(1)]],
      unitPrice: [{ value: unitPriceValue, disabled: true }, Validators.required],
      totalPrice: [quantityValue * unitPriceValue, Validators.required]
    });
    group.get('quantity')?.valueChanges.subscribe(qty => {
      const price = group.get('unitPrice')?.value || 0;
      group.get('totalPrice')?.setValue(qty * price, { emitEvent: false });
    });
    return group;
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
      this.total=res.totalCount;
      console.log('Issues:',this.adds);
      this.applyFilters();
    })
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.allItems=res.results;
      console.log('Items:',this.allItems);
      this.total=res.count;
    })
  }
  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.suppliers=res.results;
      console.log('Suppliers:',this.suppliers);
      this.total=res.count;
    })
  }
  getStores(){
    this.financialService.getStores(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.stores=res.results;
      console.log('Stores:',this.stores);
      this.total=res.count;
    })
  }
  // getPurchaseRequests(){
  //   this.financialService.getPurchaseRequests(this.pagingFilterModelSelect).subscribe((res:any)=>{
  //     this.purchaseRequests=res.results;
  //     console.log('Purchase Requests:',this.purchaseRequests);
  //     this.total=res.count;
  //   })
  // }
  getAllRequests(){
    this.financialService.getIssueRequests(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.allRequests=res.results;
      console.log('Issue Requests:',this.allRequests);
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
  
    const formData = this.addPermissionForm.getRawValue();
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
        console.log('Permission:',this.permission);
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
  // 
  getBranches(){
    this.financialService.getBranches(this.pagingFilterModel).subscribe({
      next:(res:any)=>{
        this.branchs = res.results;
        console.log('Branchs' , this.branchs);
        
      }
    })
  }
}
