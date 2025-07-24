import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { FilterModel, PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import Swal from 'sweetalert2';
import html2pdf from 'html2pdf.js';
import { todayDateValidator } from '../../../../../validators/today-date.validator';
import { MessageService } from 'primeng/api';
export declare var bootstrap:any;

@Component({
  selector: 'app-add-items',
  templateUrl: './add-items.component.html',
  styleUrl: './add-items.component.css'
})
export class AddItemsComponent implements OnInit {
  @ViewChild('printSection') printSection!: ElementRef;
  @ViewChild('printEntrySection') printEntrySection!: ElementRef;
  data: any = {};
  username = sessionStorage.getItem('firstName') + '' + sessionStorage.getItem('lastName') ; 
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

  // 
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
  TitleList = ['المخازن','إذن إستلام'];
  // 
  adds:any[]=[];
  total = 0;
  pagingFilterModel : PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 16,
    filterList: []
  }
  pagingFilterModelSelect : PagingFilterModel = {
    currentPage : 1,
    pageSize : 100,
    filterList : []
  };
  // 
  allItems: any[] = [];
  suppliers:any[]=[];
  stores:any[]=[];
  purchaseRequests:any[]=[];
  purchaseOrders:any[]=[];
  // 
  isFilter:boolean=true;
  constructor(private fb:FormBuilder , private financialService : FinancialService , private cdr : ChangeDetectorRef , private toastrService : MessageService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      documentNumber: ['' , Validators.required],
      permissionDate: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
      notes: [''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      storeId: [''],
      supplierId: [''],
      purchaseOrderId: [''],
    });    
  }
  ngOnInit(): void {
    this.getReceiptPermissions();
    this.getItems();
    this.getSuppliers();
    this.getStores();
    this.getPurchaseRequests();
    this.getPurchaseOrders();
    this.setupQuotationSelectionListener();
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      id: ['', Validators.required],
      unit: ['', Validators.required],
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

  setupQuotationSelectionListener() {
    this.addPermissionForm.get('purchaseOrderId')?.valueChanges.subscribe((id: number) => {
      if (id) {
        this.financialService.getPurchaseOrdersById(id).subscribe((res: any) => {
          const order = res.results;
          console.log(order);
          
          this.addPermissionForm.patchValue({
            supplierId: order.supplierId,
            notes: order.description || ''
          });
          this.items.clear();
          order.items.forEach((item: any) => {
            this.items.push(this.fb.group({
              id: [item.id, Validators.required],
              unit: [item.unit || '', Validators.required],
              quantity: [item.requestedQuantity || 1, Validators.required],
              unitPrice: [item.unitPrice || 0, Validators.required],
              totalPrice: [item.totalPrice || 0],
              notes: ['']
            }));
          });
        });
      }
    });
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
    this.financialService.getReceiptPermissions(this.pagingFilterModel).subscribe({
      next: (res:any) => {
        this.total = res.totalCount;
        this.adds = res.results;
        this.cdr.detectChanges();
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
    this.financialService.getItems(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.allItems=res.results;
      console.log('Items',this.allItems);
      this.total=res.count;
    })
  }
  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.suppliers=res.results;
      // console.log('Supps',this.suppliers);
      this.total=res.count;
    })
  }
  getStores(){
    this.financialService.getStores(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.stores=res.results;
      console.log('Stores',this.stores);
      this.total=res.count;
    })
  }
  getPurchaseRequests(){
    this.financialService.getPurchaseRequests(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.purchaseRequests=res.results;
      // console.log(this.purchaseRequests);
      this.total=res.count;
    })
  }
  getPurchaseOrders(){
    this.financialService.getPurchaseOrders(this.pagingFilterModelSelect).subscribe((res:any)=>{
      this.purchaseOrders=res.results;
      console.log('Orders:',this.purchaseOrders);
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
  currentpurchaseOrderId: number | null = null;
  savedOrderData!:any;
  addPermission() {
    if (this.addPermissionForm.invalid) {
      this.addPermissionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addPermissionForm.value;

    const supplierName = this.getSupplierName(formData.supplierId);
    const storeName = this.getStoreName(formData.storeId);
    
    const itemsWithNames = formData.items.map((item: any) => {
      const itemName = this.getItemName(item.id);
      const totalPrice = item.quantity * item.unitPrice;
      return { ...item, itemName, totalPrice };
    });
    
    const totalAmount = itemsWithNames.reduce((sum, i) => sum + i.totalPrice, 0);
    
    this.savedOrderData = {
      ...formData,
      supplierName,
      storeName,
      items: itemsWithNames,
      totalAmount
    };
      
    if (this.isEditMode && this.currentpurchaseOrderId) {
      this.financialService.updateReceiptPermission(this.currentpurchaseOrderId, formData).subscribe({
        next: () => this.getReceiptPermissions(),
        error: (err) => console.error('فشل التعديل:', err)
      });
    } else {
      this.financialService.addReceiptPermission(formData).subscribe({
        next: (res: any) => {
          const formData = this.addPermissionForm.value;
          if (!this.allItems?.length || !this.stores?.length || !this.suppliers?.length) {
            console.error('Required data not loaded');
            return;
          }
          this.getReceiptPermissions();
          const supplier = this.getSupplierName(formData.supplierId);
          const store = this.getStoreName(formData.storeId);
  
          const itemsWithNames = formData.items.map((item: any) => {
            const itemName = this.getItemName(item.id)
            const totalPrice = item.quantity * item.unitPrice;
            return { ...item, itemName, totalPrice };
          });
  
          const totalAmount = itemsWithNames.reduce((sum, i) => sum + i.totalPrice, 0);
  
          this.savedOrderData = {
            ...formData,
            supplierName: supplier,
            storeName: store,
            items: itemsWithNames,
            totalAmount
          };
  
          const modal = new bootstrap.Modal(document.getElementById('confirmationModal')!);
          modal.show();
          // this.addPermissionForm.reset();
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
    this.currentpurchaseOrderId = id;
  
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
          purchaseOrderId: this.permission.purchaseOrderId
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

  // 
  
  getTotal(): number {
    return this.items.value.reduce((sum: number, item: any) => sum + (+item.totalPrice || 0), 0);
  }
  
  getItemName(id: number): string {
    return this.allItems.find(i => i.id === id)?.nameAr || '---';
  }
  
  getSupplierName(id: number): string {
    return this.suppliers.find(s => s.id === id)?.name || '---';
  }
  
  getStoreName(id: number): string {
    return this.stores.find(s => s.id === id)?.name || '---';
  }

  // printPermission() {
  //   this.data = this.addPermissionForm.value;
  //   setTimeout(() => {
  //     const printContents = this.printSection.nativeElement.innerHTML;
  //     const win = window.open('', '', 'width=900,height=1000');
  //     win?.document.write(`
  //       <html>
  //         <head>
  //           <title>طباعة إذن الإضافة</title>
  //         </head>
  //         <body>${printContents}</body>
  //       </html>
  //     `);
  //     win?.document.close();
  //     win?.print();
  //   }, 200);
  // }

  // printEntry() {
  //   this.data = this.addPermissionForm.value;
  //   setTimeout(() => {
  //       const printContents = this.printEntrySection.nativeElement.innerHTML;
  //       const win = window.open('', '', 'width=900,height=1000');
  //       win?.document.write(`
  //           <html>
  //               <head>
  //                   <title>طباعة القيد</title>
  //               </head>
  //               <body>${printContents}</body>
  //           </html>
  //       `);
  //       win?.document.close();
  //       win?.print();
  //   }, 200);
  // }
  
  // printAdditionPermission(): void {
  //   const element = document.getElementById('printSection');
  //   const opt = {
  //     margin: 0.5,
  //     filename: `إذن-إضافة-${this.addPermissionForm.value.documentNumber}.pdf`,
  //     image: { type: 'jpeg', quality: 0.98 },
  //     html2canvas: { scale: 2 },
  //     jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
  //   };

  //   html2pdf().from(element).set(opt).save();
  // }
  // printJournalEntry(): void {
  //   const element = document.getElementById('printEntrySection');
  //   const opt = {
  //     margin: 0.5,
  //     filename: `قيد-${this.addPermissionForm.value.documentNumber}.pdf`,
  //     image: { type: 'jpeg', quality: 0.98 },
  //     html2canvas: { scale: 2 },
  //     jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
  //   };
  
  //   html2pdf().from(element).set(opt).save();
  // }
  printJournal(data: any) {
    const element = document.getElementById('journalPrintArea');
    if (element) {
      html2pdf().from(element).save('قيد.pdf');
    }
  }
  
  printReceipt(data: any) {
    const element = document.getElementById('receiptPrintArea');
    if (element) {
      html2pdf().from(element).save('إذن_استلام.pdf');
    }
  }
}
