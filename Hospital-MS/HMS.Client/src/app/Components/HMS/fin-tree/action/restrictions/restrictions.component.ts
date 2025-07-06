import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import Swal from 'sweetalert2';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { BanksService } from '../../../../../Services/HMS/banks.service';
import { SettingService } from '../../../../../Services/HMS/setting.service';
declare var bootstrap : any;

@Component({
  selector: 'app-restrictions',
  templateUrl: './restrictions.component.html',
  styleUrl: './restrictions.component.css'
})
export class RestrictionsComponent {
  TitleList = ['الإدارة المالية','القيود اليومية'];
  filterForm!:FormGroup;
  restrictionForm!:FormGroup
  // 
  restrictions:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  isFilter:boolean=true;
  constructor(private fb:FormBuilder , private financialService:FinancialService , private settingService : SettingService){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.restrictionForm = this.fb.group({
      restrictionNumber:['' , Validators.required],
      restrictionDate: ['', Validators.required],
      restrictionTypeId: ['', Validators.required],
      ledgerNumber: ['', Validators.required],
      description: ['', Validators.required],
      details: this.fb.array([
        // this.createdetilsGroup()
      ]),

    });
  }
  createdetilsGroup(): FormGroup {
    return this.fb.group({
      accountId: [null, Validators.required],
      debit: [null, Validators.required],
      credit: [null, Validators.required],
      costCenterId: [null, Validators.required],
      note: ['']
    });
  }
  get details(): FormArray {
    return this.restrictionForm.get('details') as FormArray;
  }
  addItemRow() {
    const index = this.details.length;
    this.details.push(this.createdetilsGroup());
    this.costCentersPerRow.push([]);
  
    const accountControl = this.details.at(index).get('accountId');
  
    if (accountControl) {
      accountControl.valueChanges.subscribe((selectedAccountId: number) => {
        const selectedAccount = this.accounts.find((acc:any) => acc.accountId === +selectedAccountId);
        console.log('Selected Account:', selectedAccount);
  
        if (selectedAccount?.children) {
          this.costCentersPerRow[index] = selectedAccount.children;
        } else {
          this.costCentersPerRow[index] = [];
        }
  
        this.details.at(index).get('costCenterId')?.setValue(null);
      });
    }
  }
  removeItemRow(index: number) {
    if (this.details.length > 1) {
      this.details.removeAt(index);
    }
  }
  
  ngOnInit(): void {
    this.getDailyRestrictions();
    this.getAccounts();
  }
  applyFilters(){
    this.total=this.restrictions.length;
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
  openRestriction(id:number){
    
  }
  // 
  isEditMode: boolean = false;
  currentRestrictionId: number | null = null;
  
  addRestriction() {
    if (this.restrictionForm.invalid) {
      this.restrictionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.restrictionForm.value;
    formData.restrictionNumber = String(formData.restrictionNumber);
    formData.ledgerNumber = String(formData.ledgerNumber);
    formData.restrictionTypeId = Number(formData.restrictionTypeId);
    formData.details = formData.details.map((item: any) => ({
      accountId: Number(item.accountId),
      debit: Number(item.debit),
      credit: Number(item.credit),
      costCenterId: Number(item.costCenterId),
      note: item.note
    }));
    if (this.isEditMode && this.currentRestrictionId !== null) {
      this.financialService.updateDailyRestriction(this.currentRestrictionId, formData).subscribe({
        next: () => {
          this.getDailyRestrictions();
          this.closeModal();
          this.restrictionForm.reset();
          this.isEditMode = false;
          this.currentRestrictionId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addDailyRestriction(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          console.log(formData);
          this.getDailyRestrictions();
          // this.closeModal();
          // this.restrictionForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  restriction!:any;
  editRestriction(id: number) {
    this.isEditMode = true;
    this.currentRestrictionId = id;
  
    this.financialService.getDailyRestrictionsById(id).subscribe({
      next: (data) => {
        this.restriction=data.results;
        console.log(this.restriction);
        this.restrictionForm.patchValue({
          restrictionNumber: this.restriction.restrictionNumber,
          restrictionDate: this.restriction.restrictionDate,
          restrictionTypeId: this.restriction.restrictionTypeId,
          ledgerNumber: this.restriction.ledgerNumber,
          details: this.restriction.details,
          
        });
        this.restriction.details.forEach(item => {
          this.details.push(this.fb.group({
            accountId: [item.accountId, Validators.required],
            debit: [item.debit, Validators.required],
            credit: [item.credit, Validators.required],
            costCenterId: [item.costCenterId, Validators.required],
            note: [item.note || '']
          }));
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addRestrictionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الخزنة:', err);
      }
    });
  }
  deleteRestriction(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا القيد؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteDailyRestriction(id).subscribe({
          next: () => {
            this.getDailyRestrictions();
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
  closeModal() {
    const modalElement = document.getElementById('addRestrictionModal')!;
    const modalInstance = bootstrap.Modal.getInstance(modalElement);
    modalInstance?.hide();
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
      backdrop.remove();
    }
  
    document.body.classList.remove('modal-open');
    document.body.style.overflow = '';
    document.body.style.paddingRight = '';
  }
  // 
  filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
      this.pagingFilterModel.searchText = '';
    this.getDailyRestrictions();
  }
  getDailyRestrictions(){
    this.financialService.getDailyRestrictions(this.pagingFilterModel).subscribe((res:any)=>{
      this.restrictions=res.results;
      this.total=res.totalCount;
      console.log(this.restrictions);
      this.applyFilters();
    })
  }
  // 
  accounts!:any;
  costCentersPerRow: any[][] = [];
  searchText = '';
  getAccounts(){
    this.settingService.GetAccountTreeHierarchicalData(this.searchText).subscribe(data => {
      this.accounts = data;
      console.log(this.accounts);
    }, (error) => {
      console.log(error);
    });
  }
}
