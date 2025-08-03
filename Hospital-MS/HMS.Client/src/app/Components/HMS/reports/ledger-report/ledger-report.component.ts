import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FinancialService } from '../../../../Services/HMS/financial.service';
import { SettingService } from '../../../../Services/HMS/setting.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReportService } from '../../../../Services/HMS/report.service';
declare var bootstrap : any;
import html2pdf from 'html2pdf.js';



@Component({
  selector: 'app-ledger-report',
  templateUrl: './ledger-report.component.html',
  styleUrl: './ledger-report.component.css'
})
export class LedgerReportComponent implements OnInit {
  @ViewChild('printSection') printSection!: ElementRef;
  accounts!:any;
  ledgerForm!:FormGroup

  constructor(private settingService : SettingService , private fb : FormBuilder , private reportService : ReportService){
    this.ledgerForm = this.fb.group({
      accountId : ['' , Validators.required],
      from : ['' , Validators.required],
      to : ['' , Validators.required]
    })
  }
  ngOnInit(): void {
    this.getAccounts();
  }
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
  reportData: any[] = [];
  selectedAccountName = '';
  totalDebit = 0;
  totalCredit = 0;
  totalCarriedBalance = 0;

  showReport() {
    if (this.ledgerForm.invalid) return;

    const { accountId, from, to } = this.ledgerForm.value;
    this.selectedAccountName = this.accounts.find(a => a.accountId === accountId)?.nameAR || '';

    this.reportService.getLedgerReport(accountId, from, to).subscribe(response => {
      if (response.isSuccess && Array.isArray(response.results)) {
        const report = response.results;

        this.reportData = response.results.map(item => ({
          entryNumber: item.dailyRestrictionNumber,
          entryDate: item.dailyRestrictionDate,
          from: item.from,
          accountName: item.accountName,
          description: item.description,
          debit: item.debits,
          credit: item.credits,
          carriedBalance: item.balance,
          isSpecial: item.description === 'الرصيد السابق' || item.description === 'تسجيل قيد يومية'
        }));
        

        this.totalDebit = this.reportData.reduce((sum, item) => sum + (item.debit || 0), 0);
        this.totalCredit = this.reportData.reduce((sum, item) => sum + (item.credit || 0), 0);
        this.totalCarriedBalance = this.reportData.length
          ? this.reportData[this.reportData.length - 1].carriedBalance
          : 0;

        const modalEl = document.getElementById('ledgerReportModal');
        if (modalEl) {
          const modal = new bootstrap.Modal(modalEl);
          modal.show();
        }
      }
    });
  }
  printReport() {
    const section = this.printSection.nativeElement as HTMLElement;
    section.style.display = 'block';
  
    const options = {
      margin: 0.5,
      filename: `تقرير الأستاذ - ${this.selectedAccountName}.pdf`,
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
  
    html2pdf().from(section).set(options).save().then(() => {
      section.style.display = 'none'; 
    });
  }
  
}
