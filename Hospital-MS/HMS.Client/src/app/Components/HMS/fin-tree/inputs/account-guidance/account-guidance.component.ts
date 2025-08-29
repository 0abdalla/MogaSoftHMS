import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { MessageService } from 'primeng/api';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
declare var bootstrap : any;
@Component({
  selector: 'app-account-guidance',
  templateUrl: './account-guidance.component.html',
  styleUrl: './account-guidance.component.css'
})
export class AccountGuidanceComponent {
  filterForm!:FormGroup;
  accountGuidanceForm!:FormGroup
  accountsGuidances:any[]=[];
  //
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    searchText: '',
    currentPage:1,
  }
  // 
  isFilter:boolean = true;
  TitleList = ['إعدادات النظام','إعدادات الإدارة المالية','التوجيهات الحسابية'];
  constructor(private fb:FormBuilder , private finService : FinancialService , private messageService: MessageService){
    this.filterForm=this.fb.group({
      Search:[],
    })
    this.accountGuidanceForm=this.fb.group({
      name:['' , Validators.required],
    })
  }
  ngOnInit(): void {
    this.getAccountsGuidances();
  }
  applyFilters(){
    this.total=this.accountsGuidances.length;
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
  getAccountsGuidances(){
    const requestModel = {
        searchText: this.pagingFilterModel.searchText,
        currentPage: this.pagingFilterModel.currentPage,
        pageSize: this.pagingFilterModel.pageSize,
        filterList: this.pagingFilterModel.filterList,
        filterType: this.pagingFilterModel.filterType,
        filterItems: this.pagingFilterModel.filterItems
    };
    this.finService.getAccountsGuidances(requestModel).subscribe({
      next:(res)=>{
        this.accountsGuidances = res.results;
        this.total = res.totalCount;
        console.log('Accounts Guidances:' , this.accountsGuidances);
      }
    })
  }
  isEditMode: boolean = false;
  currentAccountGuidanceId: number | null = null;
  AccountGuidance!:any;
  addAccountGuidance() {
    if (this.accountGuidanceForm.invalid) {
      this.accountGuidanceForm.markAllAsTouched();
      return;
    }
      
    const formData = this.accountGuidanceForm.value;
      
    if (this.isEditMode && this.currentAccountGuidanceId !== null) {
      this.finService.updateAccountingGuidance(this.currentAccountGuidanceId, formData).subscribe({
        next: () => {
          this.getAccountsGuidances();
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل التوجيه المحاسبي بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addAccountGuidanceModal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement);
              modalInstance?.hide();
            }
            this.resetFormOnClose();
          }, 2000);
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'فشل التعديل',
            detail: 'فشل تعديل التوجيه المحاسبي',
          });
        }
      });
    } else {
      this.finService.addAccountingGuidance(formData).subscribe({
        next: (res) => {
          console.log(res);
          console.log(this.accountGuidanceForm.value);
          if (res.isSuccess == true) {
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة التوجيه المحاسبي بنجاح',
            });
            this.accountGuidanceForm.reset();
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة التوجيه المحاسبي',
            });
          }
          this.getAccountsGuidances();
          // setTimeout(() => {
          //   const modalElement = document.getElementById('addAccountGuidanceModal');
          //   if (modalElement) {
          //     const modalInstance = bootstrap.Modal.getInstance(modalElement);
          //     modalInstance?.hide();
          //   }
          //   this.resetFormOnClose();
          // }, 2000);
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'فشل الإضافة',
            detail: 'فشل إضافة التوجيه المحاسبي',
          });
        }
      });
    }
  }
  editAccountGuidance(id: number) {
    this.isEditMode = true;
    this.currentAccountGuidanceId = id;
    this.finService.getAccountingGuidanceById(id).subscribe({
      next: (data: any) => {
        this.AccountGuidance = data.results;
        this.accountGuidanceForm.patchValue({
          name: this.AccountGuidance.name,
        });
        const modal = new bootstrap.Modal(document.getElementById('addAccountGuidanceModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات التوجيه المحاسبي:', err);
      }
    });
  }
  deleteAccountGuidance(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا التوجيه المحاسبي؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
    if (result.isConfirmed) {
      this.finService.deleteAccountingGuidance(id).subscribe({
        next: () => {
          this.getAccountsGuidances();
          this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف التوجيه المحاسبي بنجاح',
              });
            },
            error: (err) => {
              console.error('فشل الحذف:', err);
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف التوجيه المحاسبي',
              });
            }
          });
        }
      });
  }
  resetFormOnClose(){
    this.accountGuidanceForm.reset();
    this.currentAccountGuidanceId = null;
    this.isEditMode = false;
  }
  filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
      this.getAccountsGuidances();
  }
}
