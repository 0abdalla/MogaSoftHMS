import { Component } from '@angular/core';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
export declare var bootstrap: any;

@Component({
  selector: 'app-purchase-request',
  templateUrl: './purchase-request.component.html',
  styleUrl: './purchase-request.component.css'
})
export class PurchaseRequestComponent {
  purchaseRequests : any[] = [];
  pagingFilterModel : PagingFilterModel = {
    currentPage : 1,
    pageSize : 16,
    filterList : []
  };
  total : number = 0;
  // 
  purchaseRequestForm!:FormGroup;
  isEditMode : boolean = false;
  currentPurchaseRequestId: number | null = null;
  allItems: any[] = [];
  TitleList = ['المشتريات','طلبات شراء'];

  constructor(private financialService : FinancialService , private fb : FormBuilder){
    this.purchaseRequestForm=this.fb.group({
      purpose:[null,Validators.required],
      storeId:[null,Validators.required],
      notes:[null],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]],
      notes: ['']
    });
  }
  
  get items(): FormArray {
    return this.purchaseRequestForm.get('items') as FormArray;
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
    this.getpurchaseRequests();
    this.getItems();
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res : any)=>{
      this.allItems = res.results;
      this.total = res.totalCount;
      console.log(this.allItems);
    })
  }



  getpurchaseRequests(){
    this.financialService.getPurchaseRequests(this.pagingFilterModel).subscribe((res : any)=>{
      this.purchaseRequests = res.results;
      this.total = res.totalCount;
      console.log(this.purchaseRequests);
    })
  }

  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.pagingFilterModel.pageSize=event.itemsPerPage;
    this.applyFilters();
  }

  applyFilters(){
    this.getpurchaseRequests();
  }

  addPurchaseRequest() {
    if (this.purchaseRequestForm.invalid) {
      this.purchaseRequestForm.markAllAsTouched();
      return;
    }
  
    const formData = this.purchaseRequestForm.value;
  
    if (this.isEditMode && this.currentPurchaseRequestId) {
      this.financialService.updatePurchaseRequest(this.currentPurchaseRequestId, formData).subscribe({
        next: () => {
          this.getpurchaseRequests();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addPurchaseRequest(formData).subscribe({
        next: () => {
          this.getpurchaseRequests();
          this.purchaseRequestForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  request!:any;
  editPurchaseRequest(id: number) {
    this.isEditMode = true;
    this.currentPurchaseRequestId = id;
  
    this.financialService.getPurchaseRequestsById(id).subscribe({
      next: (data) => {
        this.request=data.results;
        console.log(this.request);
        this.purchaseRequestForm.patchValue({
          purpose: this.request.purpose,
          storeId: this.request.storeId,
          notes: this.request.notes,
          items: this.request.items
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addPurchaseRequestModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
      }
    });
  }
  deletePurchaseRequest(id: number) {
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
        this.financialService.deletePurchaseRequest(id).subscribe({
          next: () => {
            this.getpurchaseRequests();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
  // 
  getStatusName(type: string): string {
    const map: { [key: string]: string } = {
      Approved: 'تم الموافقة',
      Pending: 'قيد الانتظار',
      Rejected: 'مرفوض'
    };
    return map[type] || type;
  }
  getStatusColor(type: string): string {
    const map: { [key: string]: string } = {
      Approved: '#00FF00',
      Pending: '#FFA500',
      Rejected: '#FF0000'
    };
    return map[type] || '#000000';
  }
}
