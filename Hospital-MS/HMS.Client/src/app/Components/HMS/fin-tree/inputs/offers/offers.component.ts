import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import Swal from 'sweetalert2';
import { PagingFilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
export declare var bootstrap: any;

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
  total : number = 0;
  // 
  offerForm!:FormGroup;
  isEditMode : boolean = false;
  currentOfferId: number | null = null;
  allItems: any[] = [];
  allSuppliers: any[] = [];
  TitleList = ['المشتريات','عروض أسعار'];
  constructor(private financialService : FinancialService , private fb : FormBuilder){
    this.offerForm=this.fb.group({
      quotationDate:[Date.now()],
      supplierId:[null,Validators.required],
      notes:[''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })
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

  getOffers(){
    this.financialService.getOffers(this.pagingFilterModel).subscribe((res : any)=>{
      this.offers = res.results;
      this.total = res.totalCount;
      console.log(this.offers);
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
        next: () => {
          this.getOffers();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addOffer(formData).subscribe({
        next: () => {
          this.getOffers();
          this.offerForm.reset();
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
        this.order=data.results;
        console.log(this.order);
        this.offerForm.patchValue({
          supplierId: this.order.supplierId,
          notes: this.order.notes,
          items: this.order.items
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addOfferModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
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
}
