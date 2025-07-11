import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
declare var bootstrap: any;

@Component({
  selector: 'app-providers',
  templateUrl: './providers.component.html',
  styleUrl: './providers.component.css'
})
export class ProvidersComponent {
  filterForm!:FormGroup;
    providerForm!:FormGroup
    TitleList = ['المشتريات','الموردين'];
    isFilter:boolean = true
    // 
    providers:any[]=[];
    total:number=0;
    pagingFilterModel:any={
      pageSize:16,
      currentPage:1,
    }
    // 
    isEditMode = false;
    currentProviderId: number | null = null;

    constructor(private fb:FormBuilder , private financialService:FinancialService){
      this.filterForm=this.fb.group({
        SearchText:[],
      })
      this.providerForm = this.fb.group({
        accountCode: ['', Validators.required],
        name: ['', Validators.required],
        address: [''],
        responsibleName1: ['' , [Validators.required]],
        responsibleName2: ['' , [Validators.required]],
        phone1: ['' , [Validators.required]],
        phone2: ['' , [Validators.required]],
        taxNumber: ['' , [Validators.required]],
        job: ['' , [Validators.required]],
        fax1: [''],
        fax2: [''],
        email: ['', [Validators.email]],
        website: [''],
        notes: [''],
      });      
    }
    ngOnInit(): void {
      this.getProviders();
    }
    getProviders(){
      this.financialService.getSuppliers(this.pagingFilterModel).subscribe((res:any)=>{
        this.providers=res.results;
        console.log(this.providers);
        
        this.total=res.total;
      })
    }
    applyFilters(){
      this.total=this.providers.length;
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
    addProvider() {
      if (this.providerForm.invalid) {
        this.providerForm.markAllAsTouched();
        return;
      }
    
      const formData = this.providerForm.value;
    
      if (this.isEditMode && this.currentProviderId) {
        this.financialService.updateSupplier(this.currentProviderId, formData).subscribe({
          next: () => {
            this.getProviders();
            this.providerForm.reset();
            this.isEditMode = false;
            this.currentProviderId = null;
          },
          error: (err) => {
            console.error('فشل التعديل:', err);
          }
        });
      } else {
        this.financialService.addSupplier(formData).subscribe({
          next: () => {
            this.getProviders();
            this.providerForm.reset();
          },
          error: (err) => {
            console.error('فشل الإضافة:', err);
          }
        });
      }
    }
    providerData!:any;
    editProvider(id: number) {
      this.isEditMode = true;
      this.currentProviderId = id;
    
      this.financialService.getSuppliersById(id).subscribe({
        next: (data:any) => {
          console.log(data);
          this.providerData=data.results;
          this.providerForm.patchValue(this.providerData);
          const modal = new bootstrap.Modal(document.getElementById('addProviderModal')!);
          modal.show();
        },
        error: (err) => {
          console.error('فشل تحميل بيانات المورد:', err);
        }
      });
    }
    deleteProvider(id:number){
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل أنت متأكد من حذف هذا المورد؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.financialService.deleteSupplier(id).subscribe({
            next: () => {
              this.getProviders();
            },
            error: (err) => {
              console.error('فشل الحذف:', err);
            }
          });
        }
      });
    }
  // 
  filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
    this.getProviders();
  }
}
