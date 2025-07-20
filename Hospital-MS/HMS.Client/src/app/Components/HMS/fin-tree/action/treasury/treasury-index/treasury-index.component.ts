import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import { PagingFilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
declare var bootstrap : any;
import html2pdf from 'html2pdf.js';
import { SettingService } from '../../../../../../Services/HMS/setting.service';

@Component({
  selector: 'app-treasury-index',
  templateUrl: './treasury-index.component.html',
  styleUrl: './treasury-index.component.css'
})
export class TreasuryIndexComponent {
  TitleList = ['الإدارة المالية','حركة الخزينة'];
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
  movements: any[] = [];
  enabledTreasuries:any;
  disabledTreasuries:any;
  // 
  accounts:any[]=[];
  treasuryReportData: any = null;
  constructor(private fb:FormBuilder,private financialService:FinancialService , private settingService:SettingService){
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
      movementId: ['', Validators.required],
      // toDate: ['', Validators.required],
      // fromDate: ['', Validators.required]
    });    
  }
  ngOnInit(): void {
    this.getTreasuries();
    this.getEnabledTreasuries();
    this.getDisabledTreasuries();
    this.getAccounts();
    
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
  onTreasuryChange(event: Event) {
    const treasuryId = +(event.target as HTMLSelectElement).value;
    const selectedTreasury = this.treasuries.find(t => t.id === treasuryId);
    this.movements = selectedTreasury?.movements || [];
    this.treasuryReportForm.patchValue({ movementId: '' });
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
  // showTreasuryReport() {
  //   const treasuryId = this.treasuryReportForm.value.treasuryId;
  //   const movementId = this.treasuryReportForm.value.movementId;
  
  //   if (!treasuryId) return;
  
  //   this.financialService.getTreasuriesMovements(treasuryId).subscribe(res => {
  //     if (res.isSuccess) {
  //       this.treasuryReportData = res.results;
  //       const modal = new bootstrap.Modal(document.getElementById('openTreasuryReportModal'));
  //       modal.show();
  //     }
  //   });
  // }
  showTreasuryReport() {
    const movementId = this.treasuryReportForm.value.movementId;
  
    if (!movementId) return;
  
    this.financialService.getTreasuriesMovements(movementId).subscribe({
      next: (res) => {
        this.treasuryReportData = res.results;
        const modal = new bootstrap.Modal(document.getElementById('openTreasuryReportModal')!);
        modal.show();
      },
      error: (err) => {
        console.error(err);
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
  printTreasuryReport() {
    const element = document.getElementById('treasury-report-print');
    const treasuryId = this.treasuryReportData?.treasuryId || 'بدون_رقم';
  
    const opt = {
      margin:       0.5,
      filename:     `كشف حركة خزينة ${treasuryId}.pdf`,
      image:        { type: 'jpeg', quality: 0.98 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'landscape' }
    };
  
    html2pdf().from(element).set(opt).save();
  }
  getAccounts(){
    this.settingService.GetAccountTreeHierarchicalData('').subscribe({
      next: (res: any[]) => {
        this.accounts = this.extractLeafAccounts(res);
        console.log("Accs:", this.accounts);
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
  getAccountName(id: number): string {
    if (!this.accounts || !Array.isArray(this.accounts)) return '---';
    return this.accounts.find(s => s.accountId === id)?.nameAR || '---';
  }
}
