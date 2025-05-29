import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrl: './items.component.css'
})
export class ItemsComponent {
  filterForm!:FormGroup;
  itemForm!:FormGroup
  // 
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
      constructor(private fb:FormBuilder){
        this.filterForm=this.fb.group({
          SearchText:[],
        })
        this.itemForm = this.fb.group({
          name: ['', Validators.required],
          nameEn: ['', Validators.required],
          category: ['', Validators.required],
          unit: ['', Validators.required],
          openingBalance: [0],
          cost: [0],
          reorderLimit: [10],
          vat: [14],
          priceAfterTax: [0],
          price: [0],
          hasBarcode: ['false'],
          groupHead: ['false', Validators.required],
          itemType: ['', Validators.required],
        });
        
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
      addItem(){
        this.itemForm.reset();
      }
}
