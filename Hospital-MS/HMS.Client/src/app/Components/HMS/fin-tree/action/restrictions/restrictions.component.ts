import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, AbstractControl } from '@angular/forms';
import { FilterModel } from '../../../../../Models/Generics/PagingFilterModel';
import Swal from 'sweetalert2';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import { BanksService } from '../../../../../Services/HMS/banks.service';
import { SettingService } from '../../../../../Services/HMS/setting.service';
export declare var bootstrap : any;

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
  restrictionTypes:any[]=[];
  accountingGuidance:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  isFilter:boolean=true;
  // 
  totalDebit = 0;
  totalCredit = 0;
  isBalanced = true;
  constructor(private fb:FormBuilder , private financialService:FinancialService , private settingService : SettingService){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.restrictionForm = this.fb.group({
      restrictionDate: ['', Validators.required],
      restrictionTypeId: ['', Validators.required],
      accountingGuidanceId: ['', Validators.required],
      description: ['', Validators.required],
      details: this.fb.array([
        this.createdetilsGroup()
      ]),

    });
  }
  createdetilsGroup(): FormGroup {
    const group = this.fb.group({
      accountId: [null, Validators.required],
      debit: [0, Validators.required],
      credit: [0, Validators.required],
      costCenterId: [null, Validators.required],
      note: ['']
    });
  
    group.get('debit')?.valueChanges.subscribe(value => {
      if (value && value > 0) {
        group.get('credit')?.setValue(0);
        group.get('credit')?.disable({ emitEvent: false });
      } else {
        group.get('credit')?.enable({ emitEvent: false });
      }
      this.calculateTotals();
    });
  
    group.get('credit')?.valueChanges.subscribe(value => {
      if (value && value > 0) {
        group.get('debit')?.setValue(0);
        group.get('debit')?.disable({ emitEvent: false });
      } else {
        group.get('debit')?.enable({ emitEvent: false });
      }
      this.calculateTotals();
    });
  
    return group;
  }
  
  get details(): FormArray {
    return this.restrictionForm.get('details') as FormArray;
  }
  
  addItemRow() {
    this.details.push(this.createdetilsGroup());
    this.calculateTotals();
  }
  
  removeItemRow(index: number) {
    if (this.details.length > 1) {
      this.details.removeAt(index);
      this.calculateTotals();
    }
  }
  
  calculateTotals() {
    this.totalDebit = 0;
    this.totalCredit = 0;
  
    this.details.controls.forEach((group: AbstractControl) => {
      const debit = +group.get('debit')?.value || 0;
      const credit = +group.get('credit')?.value || 0;
      this.totalDebit += debit;
      this.totalCredit += credit;
    });
    this.isBalanced = this.totalDebit === this.totalCredit;
  }
  
  
  ngOnInit(): void {
    this.getDailyRestrictions();
    this.getRestrictionsTypes();
    this.getAccountingGuidance();
    this.getAccounts();
    this.getCostCenters();
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
  
    const formData = this.restrictionForm.getRawValue();
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
          this.restrictionForm.reset();
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
      console.log('Restrictions:',this.restrictions);
      this.applyFilters();
    })
  }
  getRestrictionsTypes(){
    this.financialService.getDailyRestrictionsTypes(this.pagingFilterModel).subscribe({
      next:(res)=>{
        this.restrictionTypes = res.results;
        this.total = res.totalCount;
        console.log('Types:' , this.restrictionTypes)
      }
    })
  }
  getAccountingGuidance(){
    this.financialService.getAccountingGuidance(this.pagingFilterModel).subscribe({
      next:(res)=>{
        this.accountingGuidance = res.results;
        this.total = res.totalCount;
        console.log('Accounting Guidance:' , this.accountingGuidance)
      }
    })
  }
  // 
  accounts!:any;
  costCenters: any;
  searchText = '';
  getAccounts() {
    this.settingService.GetAccountTreeHierarchicalData('').subscribe({
      next: (res: any[]) => {
        this.accounts = this.extractLeafAccounts(res);
        console.log("Filterated Accounts:", this.accounts);
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
  
  extractLeafAccounts(nodes: any[]): any[] {
    let result: any[] = [];
  
    for (const node of nodes) {
      if (node.isGroup === false) {
        result.push(node);
      }
      if (node.children && node.children.length > 0) {
        result = result.concat(this.extractLeafAccounts(node.children));
      }
    }
    return result;
  }
  
  getCostCenters() {
    this.settingService.GetCostCenterTreeHierarchicalData('').subscribe({
      next: (res: any[]) => {
        this.costCenters = this.extractLeafCostCenter(res); 
        console.log("Filterated Cost Centers:", this.costCenters);
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
  extractLeafCostCenter(nodes: any[]): any[] {
    let result: any[] = [];
  
    for (const node of nodes) {
      if (node.isParent === false) {
        result.push(node);
      }
      if (node.children && node.children.length > 0) {
        result = result.concat(this.extractLeafCostCenter(node.children));
      }
    }
    return result;
  }
  // 
  getStatusColor(status: string): string {
    switch (status) {
      case 'Approved': return 'green';
      case 'Rejected': return 'red';
      default: return 'gray';
    }
  }
  
  getStatusName(status: string): string {
    switch (status) {
      case 'Approved': return 'غير مرحل';
      case 'Rejected': return 'مرحل';
      default: return 'غير محدد';
    }
  }
}
