import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import Swal from 'sweetalert2';
import { PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
export declare var bootstrap: any;

@Component({
  selector: 'app-purchase-order',
  templateUrl: './purchase-order.component.html',
  styleUrl: './purchase-order.component.css'
})
export class PurchaseOrderComponent {
  purchaseOrders : any[] = [];
  pagingFilterModel : PagingFilterModel = {
    currentPage : 1,
    pageSize : 16,
    filterList : []
  };
  total : number = 0;
  // 
  purchaseOrderForm!:FormGroup;
  isEditMode : boolean = false;
  currentPurchaseRequestId: number | null = null;
  allItems: any[] = [];
  allSuppliers: any[] = [];
  TitleList = ['المشتريات','أمور شراء'];
  constructor(private financialService : FinancialService , private fb : FormBuilder){
    this.purchaseOrderForm=this.fb.group({
      supplierId:[null,Validators.required],
      description:[''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      unit: [0, [Validators.required, Validators.min(1)]],
      requestedQuantity: [0, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(1)]],
    });
  }
  
  get items(): FormArray {
    return this.purchaseOrderForm.get('items') as FormArray;
  }
  
  addItemRow() {
    this.items.push(this.createItemGroup());
  }
  
  removeItemRow(index: number) {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
  }
  
  ngOnInit(): void {
    this.getpurchaseOrders();
    this.getItems();
    this.getSuppliers();
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res : any)=>{
      this.allItems = res.results;
      this.total = res.totalCount;
      console.log(this.allItems);
    })
  }

  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModel).subscribe((res : any)=>{
      this.allSuppliers = res.results;
      this.total = res.totalCount;
      console.log(this.allSuppliers);
    })
  }

  getpurchaseOrders(){
    this.financialService.getPurchaseOrders(this.pagingFilterModel).subscribe((res : any)=>{
      this.purchaseOrders = res.results;
      this.total = res.totalCount;
      console.log(this.purchaseOrders);
    })
  }

  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.pagingFilterModel.pageSize=event.itemsPerPage;
    this.applyFilters();
  }

  applyFilters(){
    this.getpurchaseOrders();
  }

  addPurchaseorder() {
    if (this.purchaseOrderForm.invalid) {
      this.purchaseOrderForm.markAllAsTouched();
      return;
    }
  
    const formData = this.purchaseOrderForm.value;
  
    if (this.isEditMode && this.currentPurchaseRequestId) {
      this.financialService.updatePurchaseOrder(this.currentPurchaseRequestId, formData).subscribe({
        next: () => {
          this.getpurchaseOrders();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addPurchaseOrder(formData).subscribe({
        next: () => {
          this.getpurchaseOrders();
          this.purchaseOrderForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  order!:any;
  editPurchaseorder(id: number) {
    this.isEditMode = true;
    this.currentPurchaseRequestId = id;
  
    this.financialService.getPurchaseOrdersById(id).subscribe({
      next: (data) => {
        this.order=data.results;
        console.log(this.order);
        this.purchaseOrderForm.patchValue({
          supplierId: this.order.supplierId,
          description: this.order.description,
          items: this.order.items
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addPurchaseorderModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
      }
    });
  }
  deletePurchaseorder(id: number) {
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
        this.financialService.deletePurchaseOrder(id).subscribe({
          next: () => {
            this.getpurchaseOrders();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
}
