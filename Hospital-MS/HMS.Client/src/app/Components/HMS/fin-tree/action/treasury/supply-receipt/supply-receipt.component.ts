import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import { SettingService } from '../../../../../../Services/HMS/setting.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
declare var bootstrap:any;
@Component({
  selector: 'app-supply-receipt',
  templateUrl: './supply-receipt.component.html',
  styleUrl: './supply-receipt.component.css'
})
export class SupplyReceiptComponent implements OnInit {
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
  TitleList = ['الإدارة المالية','حركة الخزينة','ايصال استلام نقدية'];
  // 
  adds:any[]=[];
  isFilter:boolean=true;
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  // 
  treasuries:any[]=[];
  costCenters:any[]=[];
  constructor(private fb:FormBuilder , private financialService:FinancialService , private settingService:SettingService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      treasuryId: ['' , Validators.required],
      receivedFrom: ['' , Validators.required],
      accountCode: ['' , Validators.required],
      amount: ['' , Validators.required],
      costCenterId: [''],
      description: ['']
    });    
  }
  ngOnInit(): void {
    this.getTreasury();
    this.getSupplyReceipts();
    this.getCostCenters();
  }
  getSupplyReceipts(){
    this.financialService.getSupplyReceipts(this.pagingFilterModel).subscribe((res:any)=>{
      this.adds=res.results;
      console.log('Supply Receipts : ',this.adds);
    })
  }
  applyFilters(){
    this.total=this.adds.length;
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
  isEditMode: boolean = false;
  currentSupplyReceiptId: number | null = null;
  
  addSupplyReceipt() {
    if (this.addPermissionForm.invalid) {
      this.addPermissionForm.markAllAsTouched();
      return;
    }
    if (this.isEditMode && this.currentSupplyReceiptId !== null) {
      this.financialService.updateSupplyReceipt(this.currentSupplyReceiptId, this.addPermissionForm.value).subscribe({
        next: () => {
          this.getSupplyReceipts();
          this.addPermissionForm.reset();
          this.isEditMode = false;
          this.currentSupplyReceiptId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addSupplyReceipt(this.addPermissionForm.value).subscribe({
        next: (res:any) => {
          console.log(res);
          console.log(this.addPermissionForm.value);
          this.getSupplyReceipts();
          // this.closeModal();
          // this.addPermissionForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  supplyReceipt!:any;
  editSupplyReceipt(id: number) {
    this.isEditMode = true;
    this.currentSupplyReceiptId = id;
  
    this.financialService.getSupplyReceiptsById(id).subscribe({
      next: (data) => {
        this.supplyReceipt=data.results;
        console.log(this.supplyReceipt);
        this.addPermissionForm.patchValue({
          date: this.supplyReceipt.date,
          treasuryId: this.supplyReceipt.treasuryId,
          receivedFrom: this.supplyReceipt.receivedFrom,
          accountCode: this.supplyReceipt.accountCode,
          amount: this.supplyReceipt.amount,
          costCenterId: this.supplyReceipt.costCenterId,
          description: this.supplyReceipt.description,
          
        });
        const modal = new bootstrap.Modal(document.getElementById('addSupplyReceiptModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الإيصال:', err);
      }
    });
  }
  deleteSupplyReceipt(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا الإيصال؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteSupplyReceipt(id).subscribe({
          next: () => {
            this.getSupplyReceipts();
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
  // 
  getTreasury(){
    this.financialService.getTreasuries(this.pagingFilterModel).subscribe((res:any)=>{
      this.treasuries=res.results;
      console.log('Treasuries : ',this.treasuries);
    })
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
  filterChecked(filters: FilterModel[]){
    this.pagingFilterModel.currentPage = 1;
    this.pagingFilterModel.filterList = filters;
    if (filters.some(i => i.categoryName == 'SearchText'))
      this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
    else
    this.pagingFilterModel.searchText = '';
    this.getSupplyReceipts();
  }
}
