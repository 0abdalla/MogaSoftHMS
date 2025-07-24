import { Component } from '@angular/core';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { FilterModel, PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
export declare var bootstrap: any;
import html2pdf from 'html2pdf.js';
import { todayDateValidator } from '../../../../../validators/today-date.validator';
import { MessageService } from 'primeng/api';
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
    filterList : [],
    searchText: ''
  };
  total : number = 0;
  // 
  purchaseRequestForm!:FormGroup;
  isEditMode : boolean = false;
  currentPurchaseRequestId: number | null = null;
  allItems: any[] = [];
  stores: any[] = [];
  TitleList = ['المشتريات','طلبات شراء'];
  isFilter : boolean = true;
  // 
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName')
  get today(): string {
    const date = new Date();
    const dateStr = date.toLocaleDateString('ar-EG', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  
    const timeStr = date.toLocaleTimeString('ar-EG', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
  
    return `${dateStr} - الساعة ${timeStr}`;
  }  
  constructor(private financialService : FinancialService , private fb : FormBuilder , private toastrService : MessageService){
    this.purchaseRequestForm=this.fb.group({
      requestDate: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
      purpose:[null,Validators.required],
      storeId:[null,Validators.required],
      notes:[null],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })
  }
  onSearchInputChanged(value: any) {
    this.pagingFilterModel.searchText = value;
    this.getpurchaseRequests();
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
    this.getStores();
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res : any)=>{
      this.allItems = res.results;
      this.total = res.totalCount;
      console.log(this.allItems);
    })
  }

  getStores(){
    this.financialService.getStores(this.pagingFilterModel).subscribe((res : any)=>{
      this.stores = res.results;
      this.total = res.totalCount;
      console.log(this.stores);
    },error=>{
      console.log(error);
    })
  }

  getpurchaseRequests() {
    this.financialService.getPurchaseRequests(this.pagingFilterModel).subscribe((res : any)=>{
      console.log('Full API Response:', res);
      this.purchaseRequests = res.results;
      this.total = res.totalCount;
      console.log('Purchase Requests:', this.purchaseRequests);
        if (!res.results.length) {
          console.log('No results returned for SearchText:', this.pagingFilterModel.searchText);
        }
      },error=>{
        console.log(error);
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
  filterChecked(event: any) {
    console.log('Filter Changed Event:', event);
    this.pagingFilterModel.searchText = typeof event.searchText === 'string' ? event.searchText : '';
    this.pagingFilterModel.filterList = event.filterList || {};
    console.log('Updated Paging Filter Model:', this.pagingFilterModel);
    this.getpurchaseRequests();
  }
  purNumber!:number;
  addPurchaseRequest() {
    if (this.purchaseRequestForm.invalid) {
      this.purchaseRequestForm.markAllAsTouched();
      return;
    }
  
    const formData = this.purchaseRequestForm.value;
  
    if (this.isEditMode && this.currentPurchaseRequestId) {
      this.financialService.updatePurchaseRequest(this.currentPurchaseRequestId, formData).subscribe({
        next: (res:any) => {
          this.getpurchaseRequests();
          if(res.isSuccess === true){
            this.toastrService.add({
              severity: 'success',
              summary: 'تم التعديل',
              detail: `${res.message}`
            });
          }else{
            this.toastrService.add({
              severity: 'error',
              summary: 'فشل التعديل',
              detail: `${res.message}`
            });
          }
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addPurchaseRequest(formData).subscribe({
        next: (res:any) => {
          this.getpurchaseRequests();
          this.purNumber=res.results;
          // this.purchaseRequestForm.reset();
          if(res.isSuccess === true){
            this.toastrService.add({
              severity: 'success',
              summary: 'تم الإضافة',
              detail: `${res.message}`
            });
          }else{
            this.toastrService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: `${res.message}`
            });
          }
          this.generatePurchaseRequestPDF();
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
      Approved: '#198654',
      Pending: '#FFA500',
      Rejected: '#dc3545'
    };
    return map[type] || '#000000';
  }
  // 
  getItemName(itemId: number | string): string {
    const item = this.allItems.find(i => i.id == itemId);
    return item ? item.nameAr : '—';
  }
  
  getStoreName(storeId: number | string): string {
    const store = this.stores.find(s => s.id == storeId);
    return store ? store.name : '—';
  }
  
  generatePurchaseRequestPDF() {
    const element = document.getElementById('printablePurchaseRequest');
    if (!element) return;
    element.classList.remove('d-none');
    html2pdf().set({
      margin: 10,
      filename: `طلب-شراء-${this.purNumber}.pdf`,
      html2canvas: { scale: 2 },
      jsPDF: { orientation: 'portrait' }
    }).from(element).save().then(() => {
      element.classList.add('d-none');
    });
  }
  generatePurchaseRequestPDFById(id: number) {
    this.financialService.getPurchaseRequestsById(id).subscribe({
      next: (res: any) => {
        const request = res.results;
  
        if (!request) return;
  
        this.purchaseRequestForm.patchValue({
          requestDate: request.requestDate,
          storeId: request.storeId,
          purpose: request.purpose,
          notes: request.notes,
          items: request.items,
        });
  
        this.purNumber = request.requestNumber;
  
        setTimeout(() => {
          const element = document.getElementById('printablePurchaseRequest');
          if (!element) return;
  
          element.classList.remove('d-none');
          html2pdf().set({
            margin: 10,
            filename: `طلب-شراء-${this.purNumber}.pdf`,
            html2canvas: { scale: 2 },
            jsPDF: { orientation: 'portrait' }
          }).from(element).save().then(() => {
            element.classList.add('d-none');
          });
        }, 100);
      },
      error: (err) => {
        console.error('فشل تحميل الطلب للطباعة:', err);
      }
    });
  }
  
}
