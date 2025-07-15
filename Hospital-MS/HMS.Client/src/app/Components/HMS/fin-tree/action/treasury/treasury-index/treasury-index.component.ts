import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import { PagingFilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
declare var bootstrap : any;
@Component({
  selector: 'app-treasury-index',
  templateUrl: './treasury-index.component.html',
  styleUrl: './treasury-index.component.css'
})
export class TreasuryIndexComponent {
  TitleList = ['الإدارة المالية','حركة الخزينة','كشف حركة الخزينة'];
  closeTreasuryForm!:FormGroup;
  openTreasuryForm!:FormGroup;
  treasuryReportForm!:FormGroup;
  // 
  pagingFilterModel:PagingFilterModel={
    pageSize:16,
    currentPage:1,
    filterList:[]
  }
  treasuries:any[]=[];
  enabledTreasuries:any;
  disabledTreasuries:any;
  // 
  treasuryReportData: any = null;
  constructor(private fb:FormBuilder,private financialService:FinancialService){
    this.closeTreasuryForm=this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      treasuryId:['' , Validators.required],
      notes:['']
    });   
    this.openTreasuryForm=this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      treasuryId:['' , Validators.required],
      notes:['']
    }); 
    this.treasuryReportForm = this.fb.group({
      treasuryId: ['', Validators.required],
      toDate: ['', Validators.required],
      fromDate: ['', Validators.required]
    });    
  }
  ngOnInit(): void {
    this.getTreasuries();
    this.getEnabledTreasuries();
    this.getDisabledTreasuries();
  }
  submitCloseTreasury(){
    this.financialService.disableTreasury(this.closeTreasuryForm.value.treasuryId).subscribe({
      next: () => {
        this.getTreasuries();
        console.log(this.closeTreasuryForm.value.treasuryId);
        console.log('تم الإغلاق بنجاح');
      },
      error: (err) => {
        console.log(this.closeTreasuryForm.value.treasuryId);
        console.error('فشل الإغلاق:', err);
      }
    });
  }
  submitOpenTreasury(){
    this.financialService.enableTreasury(this.openTreasuryForm.value.treasuryId).subscribe({
      next: () => {
        this.getTreasuries();
        console.log(this.openTreasuryForm.value.treasuryId);
        console.log('تم فتح الخزينة بنجاح');
      },
      error: (err) => {
        console.log(this.openTreasuryForm.value.treasuryId);
        console.error('فشل فتح الخزينة:', err);
      }
    });
    this.openTreasuryForm.reset();
  }
  // 
  getTreasuries(){
    this.financialService.getTreasuries(this.pagingFilterModel).subscribe({
      next: (res) => {
        this.treasuries=res.results;
        console.log('Treasures',this.treasuries);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  getEnabledTreasuries(){
    this.financialService.getEnabledTreasuries().subscribe({
      next: (res) => {
        this.enabledTreasuries=res.results;
        console.log('Enabled Treasures',this.enabledTreasuries);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  getDisabledTreasuries(){
    this.financialService.getDisabledTreasuries().subscribe({
      next: (res) => {
        this.disabledTreasuries=res.results;
        console.log('Disabled Treasures',this.disabledTreasuries);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  // 
  showTreasuryReport() {
    const treasuryId = this.treasuryReportForm.value.treasuryId;
    const toDate = this.treasuryReportForm.value.toDate;
    const fromDate = this.treasuryReportForm.value.fromDate;
  
    if (!treasuryId || !toDate) return;
  
    const encodedDate = encodeURIComponent(toDate);
    const encodedFromDate = encodeURIComponent(fromDate);
    this.financialService.getTransactionsForTreasury(treasuryId + '?toDate=' + encodedDate + '&fromDate=' + encodedFromDate).subscribe(res => {
      if (res.isSuccess) {
        this.treasuryReportData = res.results;
        const modal = new bootstrap.Modal(document.getElementById('openTreasuryReportModal'));
        modal.show();
      }
    });
  }
  get creditTransactions() {
    return this.treasuryReportData?.transactions.filter(t => t.credit > 0) || [];
  }
  
  get debitTransactions() {
    return this.treasuryReportData?.transactions.filter(t => t.debit > 0) || [];
  }
  getAbsolute(value: number): number {
    return Math.abs(value);
  }  
}
