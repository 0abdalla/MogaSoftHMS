import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import Swal from 'sweetalert2';
declare var bootstrap :any;
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
@Component({
  selector: 'app-items-group',
  templateUrl: './items-group.component.html',
  styleUrl: './items-group.component.css'
})
export class ItemsGroupComponent {
  TitleList = ['إعدادات النظام','إعدادات المخازن','مجموعات الأصناف'];
  filterForm!:FormGroup;
  itemGroup!:FormGroup
  // 
  itemGroups:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    searchText: '',
    currentPage:1,
  }
  // 
  isFilter:boolean = true
  constructor(private fb:FormBuilder , private financialService : FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.itemGroup=this.fb.group({
      name:[],
      description:[],
    })
  }
  ngOnInit():void{
    this.getItemGroups();
    this.filterForm.get('Search').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe((searchText) => {
      this.pagingFilterModel.searchText = searchText;
      this.pagingFilterModel.currentPage = 1;
      this.getItemGroups();
    });
  }
  applyFilters(){
    this.total=this.itemGroups.length;
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
  getItemGroups(){
    this.financialService.getItemsGroups(this.pagingFilterModel).subscribe({
      next:(res:any)=>{
        this.itemGroups = res.results;
        this.total = res.totalCount;
        console.log('Item Groups:' , this.itemGroups);
      }
    })
  }
  isEditMode: boolean = false;
  currentItemGroupId: number | null = null;
  
  addItemGroup() {
    if (this.itemGroup.invalid) {
      this.itemGroup.markAllAsTouched();
      return;
    }
  
    const formData = this.itemGroup.value;
  
    if (this.isEditMode && this.currentItemGroupId !== null) {
      this.financialService.updateItemsGroups(this.currentItemGroupId, formData).subscribe({
        next: () => {
          this.getItemGroups();
          this.itemGroup.reset();
          this.isEditMode = false;
          this.currentItemGroupId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addItemsGroups(formData).subscribe({
        next: (res) => {
          console.log(res);
          console.log(this.itemGroup.value);
          
          this.getItemGroups();
          // this.itemGroup.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  itemGroupRow!:any;
  editItemGroup(id: number) {
    this.isEditMode = true;
    this.currentItemGroupId = id;
    
    this.financialService.getItemsGroupsById(id).subscribe({
        next: (data) => {
          this.itemGroupRow=data.results;
          this.itemGroup.patchValue({
            name: this.itemGroupRow.name,
            description: this.itemGroupRow.description,
          });
    
          const modal = new bootstrap.Modal(document.getElementById('addItemGropModal')!);
          modal.show();
        },
        error: (err) => {
          console.error('فشل تحميل بيانات المجموعة:', err);
        }
      });
    }
    deleteItemGroup(id: number) {
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل تريد حذف هذه المجموعة؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، حذف',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.financialService.deleteItemsGroups(id).subscribe({
            next: () => {
              this.getItemGroups();
            },
            error: (err) => {
              console.error('فشل الحذف:', err);
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
      this.getItemGroups();
    }
}
