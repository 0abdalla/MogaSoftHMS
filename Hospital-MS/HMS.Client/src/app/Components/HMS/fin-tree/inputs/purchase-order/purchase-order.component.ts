import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import Swal from 'sweetalert2';
import { PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
export declare var bootstrap: any;
declare var html2pdf: any;

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
  pagingFilterModelSelect : PagingFilterModel = {
    currentPage : 1,
    pageSize : 16,
    filterList : []
  };
  total : number = 0;
  // 
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
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName')
  purchaseOrderForm!:FormGroup;
  isEditMode : boolean = false;
  currentPurchaseRequestId: number | null = null;
  allItems: any[] = [];
  allSuppliers: any[] = [];
  TitleList = ['المشتريات','أمور شراء'];
  // 
  approvedPrices!:any;
  constructor(private financialService : FinancialService , private fb : FormBuilder){
    this.purchaseOrderForm=this.fb.group({
      orderDate:[new Date().toISOString()],
      priceQuotationId:[null , Validators.required],
      supplierId:[null,Validators.required],
      referenceNumber:[null,Validators.required],
      description:[''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      unitId: [null, Validators.required],
      requestedQuantity: [0, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(1)]],
      totalPrice: [0, [Validators.required, Validators.min(1)]]
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
    this.getItems();
    this.getpurchaseOrders();
    this.getSuppliers();
    this.getPriceQutations();
    this.purchaseOrderForm.get('priceQuotationId')?.valueChanges.subscribe((id: number) => {
      if (id) {
        this.financialService.getOffersById(id).subscribe((res: any) => {
          const price = res.results;
          console.log(price);
          this.purchaseOrderForm.patchValue({
            supplierId: price.supplierId,
            description: price.notes
          });
        
          this.items.clear();
          price.items.forEach((item: any) => {
            this.items.push(this.fb.group({
              itemId: [item.id],
              requestedQuantity: [item.quantity],
              unitPrice: [item.unitPrice],
              totalPrice : [item.total]
            }));
          });
        });
        
      }
    });
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res : any)=>{
      this.allItems = res.results;
      this.total = res.totalCount;
      console.log(this.allItems);
      this.setupQuotationSelectionListener();
    })
  }
  setupQuotationSelectionListener() {
    this.purchaseOrderForm.get('priceQutationId')?.valueChanges.subscribe((id: number) => {
      if (id) {
        this.financialService.getOffersById(id).subscribe((res: any) => {
          const price = res.results;
  
          this.purchaseOrderForm.patchValue({
            supplierId: price.supplierId,
            description: price.notes
          });
  
          this.items.clear();
          price.items.forEach((item: any) => {
            this.items.push(this.fb.group({
              itemId: [item.id],
              requestedQuantity: [item.quantity],
              unitPrice: [item.unitPrice],
              totalPrice: [item.total]
            }));
          });
        });
      }
    });
  }

  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModelSelect).subscribe((res : any)=>{
      this.allSuppliers = res.results;
      this.total = res.totalCount;
      console.log(this.allSuppliers);
    })
  }

  getpurchaseOrders(){
    this.financialService.getPurchaseOrders(this.pagingFilterModelSelect).subscribe((res : any)=>{
      this.purchaseOrders = res.results;
      this.total = res.totalCount;
      console.log(this.purchaseOrders);
    })
  }

  getPriceQutations(){
    this.financialService.getApprovedPriceQutations().subscribe({
      next:(res)=>{
        this.approvedPrices = res.results;
        console.log('Approved : ' , this.approvedPrices);
        
      }
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
  savedOrderData: any;
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
        next: async() => {
          this.getpurchaseOrders();
          // this.savedOrderData = res;
          // const modal = new bootstrap.Modal(document.getElementById('confirmationModal')!);
          // modal.show();
          await this.onQuotationChange();
          this.printOffers();

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
  // 
  structuredTable: any[] = [];
  buildStructuredTable() {
    const selected = this.selectedQuotation;
    if (!selected || !selected.items) return;
  
    this.structuredTable = selected.items.map((item: any) => ({
      itemName: item.itemName,
      quantity: item.quantity,
      unitPrice: item.unitPrice,
      totalPrice: item.unitPrice * item.quantity
    }));
  }
  onQuotationChange() {
    this.buildStructuredTable();
  }
  
  get selectedQuotation() {
    if (!this.approvedPrices || !Array.isArray(this.approvedPrices)) return null;
    return this.approvedPrices.find(q => q.id === this.purchaseOrderForm.value.priceQuotationId);
  }  
  
  get selectedSupplier() {
    if (!this.allSuppliers || !Array.isArray(this.allSuppliers)) return null;
    return this.allSuppliers.find(s => s.id === this.purchaseOrderForm.value.supplierId);
  }
  
  getItemNameById(id: number) {
    return this.allItems.find(x => x.id === id)?.itemName || '';
  }

  getTotalOrderPrice(): number {
    const total = this.structuredTable?.reduce((acc, item) => acc + (+item.totalPrice || 0), 0) || 0;
    return total;
  }    

  printOffers() {
    const element = document.getElementById('printablePurchaseOrder');
    if (!element) {
      console.error('العنصر غير موجود');
      return;
    }
    element.style.display = 'block';
    const opt = {
      margin: 0.5,
      filename: 'أمر_شراء.pdf',
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
    html2pdf().set(opt).from(element).save().then(() => {
      element.style.display = 'none';
    }).catch(err => {
      console.error('حدث خطأ أثناء توليد PDF:', err);
      element.style.display = 'none';
    });
  }
  
  
}

