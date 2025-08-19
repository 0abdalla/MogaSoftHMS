import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { MessageService } from 'primeng/api';
declare var bootstrap: any;

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrl: './items.component.css'
})
export class ItemsComponent {
  TitleList = ['إعدادات النظام','إعدادات المخازن','الأصناف'];
  filterForm!:FormGroup;
  itemForm!:FormGroup
  // 
  isEditMode = false;
  currentItemId: number | null = null;
  // 
  isFilter:boolean = true;
  numericFields = [
    { name: 'openingBalance', label: 'رصيد أول المدة' },
    { name: 'cost', label: 'تكلفة الصنف' },
    { name: 'reorderLimit', label: 'حد الطلب' },
    { name: 'vat', label: 'ضريبة المبيعات' },
    { name: 'priceAfterTax', label: 'السعر بعد الضريبة' },
    { name: 'price', label: 'السعر' }
  ];  
      // 
      items:any[]=[];
      total:number=0;
      pagingFilterModel:any={
        pageSize:16,
        currentPage:1,
      }
      pagingFilterModelSelect:any={
        pageSize:100,
        currentPage:1,
      }
      groups:any[];
      units!:any[];
      constructor(private fb:FormBuilder , private financialService:FinancialService , private messageService: MessageService){
        this.filterForm=this.fb.group({
          SearchText:[],
        })
        this.itemForm = this.fb.group({
          nameAr: ['', Validators.required],
          nameEn: ['', Validators.required],
          unitId: [null, Validators.required],
          groupId: [null, Validators.required],
          orderLimit: [0],
          cost: [0],
          openingBalance: [0],
          salesTax: [0],
          price: [0],
          hasBarcode: [true],
          typeId: [null]
        });
        
        this.getItems();
        this.getGroups();
        this.getUnits();
      }
      getItems(){
        this.financialService.getItems(this.pagingFilterModel).subscribe((res : any)=>{
          this.items = res.results;
          this.total = res.totalCount;
          console.log(this.items);
        })
      }
      getGroups(){
        this.financialService.getItemsGroups(this.pagingFilterModelSelect).subscribe((res : any)=>{
          this.groups = res.results;
          console.log(this.groups);
        })
      }
      getUnits(){
        this.financialService.getUnits(this.pagingFilterModelSelect).subscribe((res : any)=>{
          this.units = res.results;
          console.log(this.units);
        })
      }
      applyFilters(){
        this.total=this.items.length;
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
      openItem(id:number){
        
      }
      // 
      addItem() {
        if (this.itemForm.invalid) {
          this.itemForm.markAllAsTouched();
          return;
        }
        const rawForm = this.itemForm.value;
        const formData = {
          nameAr: rawForm.nameAr,
          nameEn: rawForm.nameEn,
          unitId: rawForm.unitId,
          groupId: Number(rawForm.groupId),
          orderLimit: Number(rawForm.orderLimit),
          cost: Number(rawForm.cost),
          openingBalance: Number(rawForm.openingBalance),
          salesTax: Number(rawForm.salesTax),
          price: Number(rawForm.price),
          hasBarcode: rawForm.hasBarcode === true || rawForm.hasBarcode === 'true',
          typeId: null
        };
        if (this.isEditMode && this.currentItemId) {
          this.financialService.updateItem(this.currentItemId, formData).subscribe({
            next: (res) => {
              console.log('تم تعديل الصنف:', res);
              this.getItems();
              this.itemForm.reset();
              this.isEditMode = false;
              this.currentItemId = null;
              this.messageService.add({
                severity: 'success',
                summary: 'تم التعديل بنجاح',
                detail: 'تم تعديل الصنف بنجاح',
              });
              setTimeout(() => {
                const modalElement = document.getElementById('addItemModal');
                if (modalElement) {
                  const modalInstance = bootstrap.Modal.getInstance(modalElement);
                  modalInstance?.hide();
                }
                this.resetFormOnClose();
              }, 1000);
            },
            error: (err) => {
              console.error('خطأ أثناء التعديل:', err);
            }
          });
        } else {
          this.financialService.addItem(formData).subscribe({
            next: (res) => {
              console.log('تم إضافة الصنف:', res);
              this.getItems();
              console.log(JSON.stringify(formData));
              if(res.isSuccess == true){
                this.messageService.add({
                  severity: 'success',
                  summary: 'تم الإضافة بنجاح',
                  detail: 'تم إضافة الصنف بنجاح',
                });
              }else{
                this.messageService.add({
                  severity: 'error',
                  summary: 'فشل الإضافة',
                  detail: 'فشل إضافة الصنف',
                });
              }
              this.itemForm.reset();
            },
            error: (err) => {
              console.error('خطأ أثناء الإضافة:', err);
            }
          });
        }
      }
      itemRes:any;
      editItem(id: number) {
        this.isEditMode = true;
        this.currentItemId = id;
        this.financialService.getItemsById(id).subscribe({
          next: (itemData:any) => {
            console.log(itemData);
            this.itemRes = itemData.results;
            this.itemForm.patchValue({
              nameAr: this.itemRes.nameAr,
              nameEn: this.itemRes.nameEn,
              unitId: this.itemRes.unitId,
              groupId: this.itemRes.groupId,
              orderLimit: this.itemRes.orderLimit,
              cost: this.itemRes.cost,
              openingBalance: this.itemRes.openingBalance,
              salesTax: this.itemRes.salesTax,
              price: this.itemRes.price,
              hasBarcode: this.itemRes.hasBarcode,
              typeId: this.itemRes.typeId
            });
            const modal = new bootstrap.Modal(document.getElementById('addItemModal')!);
            modal.show();
          },
          error: (err) => {
            console.error('فشل تحميل بيانات الصنف:', err);
          }
        });
      }
      deleteItem(id:number){
        Swal.fire({
          title: 'هل أنت متأكد؟',
          text: 'هل أنت متأكد من حذف هذا الصنف؟',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'نعم',
          cancelButtonText: 'إلغاء'
        }).then((result) => {
          if (result.isConfirmed) {
            this.financialService.deleteItem(id).subscribe({
              next: (res) => {
                console.log('تم حذف الصنف:', res);
                this.getItems();
              },
              error: (err) => {
                console.error('فشل حذف الصنف:', err);
              }
            });
          }
        });
      }
    filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
    this.getItems();
  }
  resetFormOnClose() {
    this.itemForm.reset();
    this.isEditMode = false;
    this.currentItemId = null;
  }
}
