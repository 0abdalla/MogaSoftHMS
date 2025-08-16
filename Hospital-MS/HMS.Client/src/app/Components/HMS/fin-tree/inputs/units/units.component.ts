import { Component } from '@angular/core';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import { MessageService } from 'primeng/api';
declare var bootstrap:any;

@Component({
  selector: 'app-units',
  templateUrl: './units.component.html',
  styleUrl: './units.component.css'
})
export class UnitsComponent {
  TitleList = ['إعدادات النظام','إعدادات المخازن','وحدات الأصناف'];
  pagingFilterModel:any={
    pageSize:16,
    searchText: '',
    currentPage:1,
  }
  isFilter:boolean = true;
  total!:any;
  units:any[]=[];
  // 
  filterForm!:FormGroup;
  unitForm!:FormGroup;
  constructor(private finService : FinancialService , private fb : FormBuilder , private messageService: MessageService){
    this.filterForm=this.fb.group({
      Search:[],
    })
    this.unitForm=this.fb.group({
      name:['' , Validators.required],
    })
  }
  ngOnInit() : void {
    this.getUnits();
    this.filterForm.get('Search').valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
    ).subscribe((searchText) => {
      this.pagingFilterModel.searchText = searchText;
      this.pagingFilterModel.currentPage = 1;
      this.getUnits();
    });
  }
  applyFilters(){
    this.total=this.units.length;
  }
  resetFilters(){
    this.filterForm.reset();
    this.applyFilters();
  }
  getUnits(){
    this.finService.getUnits(this.pagingFilterModel).subscribe({
      next:(res)=>{
        this.units = res.results;
        this.total = res.totalCount;
        console.log('Units:' , this.units);
      }
    })
  }
  
  onPageChange(event:any){
    this.pagingFilterModel.currentPage=event.page;
    this.pagingFilterModel.pageSize=event.itemsPerPage;
    this.applyFilters();
  }
  filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
      this.getUnits();
  }
  // 
  isEditMode: boolean = false;
  currentMainGroupId: number | null = null;
  
  addUnit() {
    if (this.unitForm.invalid) {
      this.unitForm.markAllAsTouched();
      return;
    }
  
    const formData = this.unitForm.value;
  
    if (this.isEditMode && this.currentMainGroupId !== null) {
      this.finService.updateUnit(this.currentMainGroupId, formData).subscribe({
        next: () => {
          this.getUnits();
          this.unitForm.reset();
          this.isEditMode = false;
          this.currentMainGroupId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل الوحدة بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addUnitModal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement);
              modalInstance?.hide();
            }
            this.resetFormOnClose();
          }, 1000);
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.finService.addUnit(formData).subscribe({
        next: (res) => {
          console.log(res);
          console.log(this.unitForm.value);
          this.unitForm.reset();
          this.getUnits();
          if(res.isSuccess == true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة الوحدة بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة الوحدة',
            });
          }
          this.unitForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  Unit!:any;
  editUnit(id: number) {
    this.isEditMode = true;
    this.currentMainGroupId = id;
  
    this.finService.getUnitsById(id).subscribe({
      next: (data:any) => {
        this.Unit=data.results;
        this.unitForm.patchValue({
          name: this.Unit.name,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addUnitModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات المجموعة:', err);
      }
    });
  }
  deleteUnit(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذه الوحدة؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.finService.deleteUnit(id).subscribe({
          next: () => {
            this.getUnits();
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
  resetFormOnClose() {
    this.unitForm.reset();
    this.isEditMode = false;
    this.currentMainGroupId = null;
  }
}
