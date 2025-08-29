import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import Swal from 'sweetalert2';
import { PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { MessageService } from 'primeng/api';
export declare var bootstrap: any;
declare var html2pdf: any;


@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrl: './offers.component.css'
})
export class OffersComponent {
  offers : any[] = [];
  pagingFilterModel : PagingFilterModel = {
    currentPage : 1,
    pageSize : 16,
    filterList : []
  };
  pagingFilterModelForSelect : PagingFilterModel = {
    currentPage : 1,
    pageSize : 200,
    filterList : []
  };
  total : number = 0;
  // 
  offerForm!:FormGroup;
  requestForm!:FormGroup
  isEditMode : boolean = false;
  currentOfferId: number | null = null;
  allItems: any[] = [];
  allSuppliers: any[] = [];
  allPurchaseRequests: any[] = [];
  // 
  quotationData: any[] = [];
  supplierNames: string[] = [];
  uniqueItems: string[] = [];
  structuredTable: any[] = [];
  selectedPurchaseRequestId: number | null = null;

  TitleList = ['المشتريات','عروض أسعار'];
  constructor(private financialService : FinancialService , private fb : FormBuilder , private toastrService : MessageService){
    this.offerForm=this.fb.group({
      quotationDate:[new Date().toISOString()],
      supplierId:[null,Validators.required],
      purchaseRequestId:[null,Validators.required], 
      notes:[''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })

    this.requestForm = this.fb.group({
      purchaseRequestId: [null, Validators.required]
    });
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      unitPrice: [0, [Validators.required, Validators.min(1)]],
      quantity: [0, [Validators.required, Validators.min(1)]],
      notes: [''],
    });
  }
  
  get items(): FormArray {
    return this.offerForm.get('items') as FormArray;
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
    this.getOffers();
    this.getItems();
    this.getSuppliers();
    this.getPurchaseRequests();
    // 
    this.offerForm.get('purchaseRequestId')?.valueChanges.subscribe(id => {
      if (id) {
        this.purchaseRequestSelected(id);
      }
    });
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModelForSelect).subscribe((res : any)=>{
      this.allItems = res.results;
      this.total = res.totalCount;
      console.log(this.allItems);
    })
  }

  getSuppliers(){
    this.financialService.getSuppliers(this.pagingFilterModelForSelect).subscribe((res : any)=>{
      this.allSuppliers = res.results;
      this.total = res.totalCount;
      console.log(this.allSuppliers);
    })
  }

  getOffers(){
    this.financialService.getOffers(this.pagingFilterModel).subscribe((res : any)=>{
      this.offers = res.results;
      this.total = res.totalCount;
      console.log(this.offers);
    })
  }
  getPurchaseRequests(){
    this.financialService.getapprovedPurchaseRequests(this.pagingFilterModelForSelect).subscribe((res : any)=>{
      this.allPurchaseRequests = res.results;
      this.total = res.totalCount;
      console.log('Purchase Requests',this.allPurchaseRequests);
    })
  }

  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.pagingFilterModel.pageSize=event.itemsPerPage;
    this.applyFilters();
  }

  applyFilters(){
    this.getOffers();
  }

  addOffer() {
    if (this.offerForm.invalid) {
      this.offerForm.markAllAsTouched();
      return;
    }
  
    const formData = this.offerForm.value;
  
    if (this.isEditMode && this.currentOfferId) {
      this.financialService.updateOffer(this.currentOfferId, formData).subscribe({
        next: (res:any) => {
          this.getOffers();
          if(res.isSuccess === true){
            this.toastrService.add({
              severity: 'success',
              summary: 'تم التعديل',
              detail: `${res.message}`
            });
            // this.offerForm.reset();
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
      this.financialService.addOffer(formData).subscribe({
        next: (res : any) => {
          console.log(res);
          this.getOffers();
          if(res.isSuccess === true){
            this.toastrService.add({
              severity: 'success',
              summary: 'تمت الإضافة',
              detail: `${res.message}`
            });
            // this.offerForm.reset();
            
          }else{
            this.toastrService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: `${res.message}`
            });
          }
          // this.offerForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  order!:any;
  editOffer(id: number) {
    this.isEditMode = true;
    this.currentOfferId = id;
  
    this.financialService.getOffersById(id).subscribe({
      next: (data) => {
        this.order = data.results;
        const itemsArray = this.offerForm.get('items') as FormArray;
        itemsArray.clear();
        this.order.items.forEach((item: any) => {
          itemsArray.push(this.fb.group({
            itemId: [item.itemId ?? item.id, Validators.required],
            quantity: [item.quantity, Validators.required],
            unitPrice: [item.unitPrice, Validators.required],
            notes: [item.notes || '']
          }));
        });
        this.offerForm.patchValue({
          purchaseRequestId: this.order.purchaseRequestId,
          supplierId: this.order.supplierId,
          notes: this.order.notes
        });
        const modal = new bootstrap.Modal(document.getElementById('addOfferModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات عرض السعر:', err);
      }
    });
  }
  
  deleteOffer(id: number) {
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
        this.financialService.deleteOffer(id).subscribe({
          next: () => {
            this.getOffers();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
  // 
  purchaseRequestSelected(id: number) {
    this.financialService.getPurchaseRequestsById(id).subscribe(res => {
      const request = res.results;
      console.log(request);
      this.items.clear();
      request.items.forEach((item: any) => {
        this.items.push(this.fb.group({
          itemId: [item.itemId, Validators.required],
          quantity: [item.quantity, Validators.required],
          unitPrice: ['' , Validators.required],
          notes: [item.notes || '']
        }));
      });
      if (request.items.length === 0) {
        this.addItemRow();
      }
      this.offerForm.patchValue({
        notes: request.notes
      });
    });
  }
  //
  purNumber!:number;
  quotationDate!:string;
  loadQuotationsByRequestId(purchaseRequestId: number) {
    this.selectedPurchaseRequestId = purchaseRequestId;
  
    this.financialService.getPriceQuotationById(purchaseRequestId).subscribe((res: any) => {
      this.quotationDate = res.results[0]?.quotationDate;
      this.quotationData = res?.results || [];
      this.purNumber = res.results[0]?.purchaseRequestNumber;
  
      if (!this.quotationData.length) {
        Swal.fire({
          icon:"error",
          title:"حدث خطأ",
          text:"لا يوجد عروض أسعار لطلب الشراء المحدد",
          confirmButtonText:'حسنًا',
          confirmButtonColor:'#3085d6',
          timer:5000
        })
        // alert('لا توجد عروض أسعار لهذا الطلب');
        return;
      }
  
      this.supplierNames = this.quotationData.map(q => q.supplierName);
      const allnameArs = this.quotationData.flatMap(q => q.items.map((i: any) => i.nameAr));
      this.uniqueItems = Array.from(new Set(allnameArs));
  
      this.structuredTable = this.uniqueItems.map(nameAr => {
        const row: any = { nameAr };
        this.quotationData.forEach(quotation => {
          const item = quotation.items.find((i: any) => i.nameAr === nameAr);
          row[quotation.supplierName] = {
            quantity: item?.quantity || 0,
            unitPrice: item?.unitPrice || 0,
            total: item?.total || 0
          };
        });
        return row;
      });
      const selectModalEl = document.getElementById('selectRequestModal');
      const selectModal = bootstrap.Modal.getInstance(selectModalEl!);
      selectModal?.hide();
      setTimeout(() => {
        document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
        document.body.classList.add('modal-open');
        const detailsModalEl = document.getElementById('requestDetailsModal');
        const detailsModal = new bootstrap.Modal(detailsModalEl!);
        detailsModal.show();
      }, 300);
  
    }, error => {
      console.error('فشل تحميل عروض الأسعار:', error);
      alert('حدث خطأ أثناء تحميل عروض الأسعار.');
    });
  }  

  getTotalForSupplier(supplier: string): number {
    if (!this.structuredTable) return 0;
  
    return this.structuredTable.reduce((sum, row) => {
      return sum + (row[supplier]?.total || 0);
    }, 0);
  }
  
  getWinningSupplier(): string {
    if (!this.supplierNames || this.supplierNames.length === 0) return 'لا يوجد بيانات';
  
    const totals = this.supplierNames.map(name => {
      const total = this.getTotalForSupplier(name);
      return {
        name,
        total: total ?? Infinity
      };
    });
  
    if (totals.length === 0) return 'لا يوجد بيانات';
  
    const winner = totals.reduce((min, curr) => curr.total < min.total ? curr : min, totals[0]);
  
    return winner.total !== Infinity ? winner.name : 'لا يوجد بيانات';
  }
  postPrice(){
    this.financialService.putPriceQutataion(this.selectedPurchaseRequestId, '').subscribe({
      next: () => {
        this.getOffers();
      },
      error: (err) => {
        console.error('فشل حفظ العرض:', err);
      }
    });
  }

  getStatusName(type: string): string {
    const map: { [key: string]: string } = {
      Approved: 'تم الترسية',
      Pending: 'قيد الانتظار',
      Rejected: 'لم يتم الترسية'
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
  printOffers() {
    const element = document.getElementById('printableOffers');
    if (!element) {
      console.error('العنصر غير موجود');
      return;
    }
    element.style.display = 'block';
    const opt = {
      margin: 0.5,
      filename: 'تفريغ_العروض_طلب_شراء_رقم_ ' + this.purNumber + '.pdf',
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'a4', orientation: 'landscape' }
    };
    html2pdf().set(opt).from(element).save().then(() => {
      element.style.display = 'none';
    }).catch(err => {
      console.error('حدث خطأ أثناء توليد PDF:', err);
      element.style.display = 'none';
    });
  }
  resetForm(){
    this.offerForm.reset();
    this.items.clear();
    this.items.push(this.createItemGroup());
    this.isEditMode = false;
    this.currentOfferId = null;
  }
}
