import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
declare var bootstrap:any;

@Component({
  selector: 'app-exchange-premssion',
  templateUrl: './exchange-premssion.component.html',
  styleUrl: './exchange-premssion.component.css'
})
export class ExchangePremssionComponent implements OnInit {
  filterForm!:FormGroup;
  TitleList = ['الإدارة المالية','حركة الخزينة','إذن صرف نقدي'];
  exPermissionForm!:FormGroup
  // 
  exchanges:any[]=[];
  treasuries:any[]=[];
  items:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  // 
  isFilter:boolean=true
  constructor(private fb:FormBuilder , private financialService:FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.exPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      fromStoreId: ['' , Validators.required],
      toStoreId: ['' , Validators.required],
      quantity: [1 , Validators.required],
      itemId: ['' , Validators.required],
      notes: ['']
    });    
  }
  ngOnInit(): void {
    this.getDispensePermissions();
    this.getTreasuries();
    this.getItems();
  }
  applyFilters(){
    this.total=this.exchanges.length;
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
  getDispensePermissions(){
    this.financialService.getDispensePermissions(this.pagingFilterModel).subscribe((res:any)=>{
      this.exchanges=res.results;
      console.log('exchanges',this.exchanges);
      this.total=res.count;
      this.applyFilters();
    })
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res:any)=>{
      this.items=res.results;
      console.log('items',this.items);
      this.total=res.count;
    })
  }
  getTreasuries(){
    this.financialService.getTreasuries(this.pagingFilterModel).subscribe((res:any)=>{
      this.treasuries=res.results;
      console.log('treasuries',this.treasuries);
      this.total=res.count;
    })
  }
  // getSuppliers(){
  //   this.financialService.getSuppliers(this.pagingFilterModel).subscribe((res:any)=>{
  //     this.suppliers=res.results;
  //     console.log(this.suppliers);
  //     this.total=res.count;
  //   })
  // }
  // getStores(){
  //   this.financialService.getStores(this.pagingFilterModel).subscribe((res:any)=>{
  //     this.stores=res.results;
  //     console.log(this.stores);
  //     this.total=res.count;
  //   })
  // }
  // getPurchaseRequests(){
  //   this.financialService.getPurchaseRequests(this.pagingFilterModel).subscribe((res:any)=>{
  //     this.purchaseRequests=res.results;
  //     console.log(this.purchaseRequests);
  //     this.total=res.count;
  //   })
  // }
  filterChecked(filters: FilterModel[]) {
          this.pagingFilterModel.currentPage = 1;
          this.pagingFilterModel.filterList = filters;
          if (filters.some(i => i.categoryName == 'SearchText'))
            this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
          else
            this.pagingFilterModel.searchText = '';
          this.getDispensePermissions();
          this.getTreasuries();
  }
  // 
  isEditMode : boolean = false;
  cuerrentExchangePermissionId: number | null = null;
  addPermission() {
    if (this.exPermissionForm.invalid) {
      this.exPermissionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.exPermissionForm.value;
    if (this.isEditMode && this.cuerrentExchangePermissionId) {
      
      this.financialService.updateDispensePermission(this.cuerrentExchangePermissionId, formData).subscribe({
        next: () => {
          this.getDispensePermissions();
          console.log('تم التعديل بنجاح');
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addDispensePermission(formData).subscribe({
        next: () => {
          this.getDispensePermissions();
          console.log('Data:',formData);
          console.log();
          
          // this.exPermissionForm.reset();
          console.log('تم الإضافة بنجاح');
        },
        error: (err) => {
          console.log('Data:',formData);
          console.log('Errors:' , this.exPermissionForm.errors);
          console.log('Form:' , this.exPermissionForm);
          
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  permission!:any;
  editPermission(id: number) {
    this.isEditMode = true;
    this.cuerrentExchangePermissionId = id;
  
    this.financialService.getDispensePermissionsById(id).subscribe({
      next: (data) => {
        this.permission=data.results;
        console.log(this.permission);
        this.exPermissionForm.patchValue({
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
        this.financialService.deleteDispensePermission(id).subscribe({
          next: () => {
            this.getDispensePermissions();
            console.log('تم الحذف بنجاح');
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
}
