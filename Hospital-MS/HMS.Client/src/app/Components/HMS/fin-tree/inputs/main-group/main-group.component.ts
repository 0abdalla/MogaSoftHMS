import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import Swal from 'sweetalert2';
declare var bootstrap:any;
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-main-group',
  templateUrl: './main-group.component.html',
  styleUrl: './main-group.component.css'
})
export class MainGroupComponent implements OnInit {
  filterForm!:FormGroup;
  mainGroupForm!:FormGroup
  // 
  mainGroups:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    searchText: '',
    currentPage:1,
  }
  // 
  isFilter:boolean = true;
  TitleList = ['إعدادات النظام','إعدادات الإدارة المالية','المجموعات الرئيسية'];
  constructor(private fb:FormBuilder , private financialService : FinancialService){
    this.filterForm=this.fb.group({
      Search:[],
    })
    this.mainGroupForm=this.fb.group({
      name:['' , Validators.required],
      description:[],
    })
  }
  ngOnInit(): void {
    this.getMainGroups();
    this.filterForm.get('Search').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe((searchText) => {
      this.pagingFilterModel.searchText = searchText;
      this.pagingFilterModel.currentPage = 1;
      this.getMainGroups();
    });
  }
  applyFilters(){
    this.total=this.mainGroups.length;
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
  getMainGroups(){
    const requestModel = {
      searchText: this.pagingFilterModel.searchText,
      currentPage: this.pagingFilterModel.currentPage,
      pageSize: this.pagingFilterModel.pageSize,
      filterList: this.pagingFilterModel.filterList,
      filterType: this.pagingFilterModel.filterType,
      filterItems: this.pagingFilterModel.filterItems
    };
    this.financialService.getMainGroups(requestModel).subscribe({
      next:(res)=>{
        this.mainGroups = res.results;
        this.total = res.totalCount;
        console.log('Main Groups:' , this.mainGroups);
      }
    })
  }
  filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
    this.getMainGroups();
  }
  // 
  isEditMode: boolean = false;
  currentMainGroupId: number | null = null;
  
  addMainGroup() {
    if (this.mainGroupForm.invalid) {
      this.mainGroupForm.markAllAsTouched();
      return;
    }
  
    const formData = this.mainGroupForm.value;
  
    if (this.isEditMode && this.currentMainGroupId !== null) {
      this.financialService.updateMainGroups(this.currentMainGroupId, formData).subscribe({
        next: () => {
          this.getMainGroups();
          this.mainGroupForm.reset();
          this.isEditMode = false;
          this.currentMainGroupId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addMainGroups(formData).subscribe({
        next: (res) => {
          console.log(res);
          console.log(this.mainGroupForm.value);
          
          this.getMainGroups();
          // this.mainGroupForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  MainGroup!:any;
  editMainGroup(id: number) {
    this.isEditMode = true;
    this.currentMainGroupId = id;
  
    this.financialService.getMainGroupsById(id).subscribe({
      next: (data:any) => {
        this.MainGroup=data.results;
        this.mainGroupForm.patchValue({
          name: this.MainGroup.name,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addMainGroupModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات المجموعة:', err);
      }
    });
  }
  deleteMainGroup(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا المجموعة؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteMainGroups(id).subscribe({
          next: () => {
            this.getMainGroups();
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
}
