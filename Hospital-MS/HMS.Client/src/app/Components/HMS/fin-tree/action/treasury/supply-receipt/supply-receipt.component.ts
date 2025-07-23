import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import { SettingService } from '../../../../../../Services/HMS/setting.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
declare var bootstrap:any;
import html2pdf from 'html2pdf.js';
import { todayDateValidator } from '../../../../../../validators/today-date.validator';

@Component({
  selector: 'app-supply-receipt',
  templateUrl: './supply-receipt.component.html',
  styleUrl: './supply-receipt.component.css'
})
export class SupplyReceiptComponent implements OnInit {
  @ViewChild('printSection') printSection!: ElementRef;

  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
  TitleList = ['الإدارة المالية','حركة الخزينة','إيصال توريد'];
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
  accounts:any[]=[];
  // 
  receiptNumber!:any;
  amountInWords!:any;
  get today(): string {
    const date = new Date();
    const dateStr = date.toLocaleDateString('ar-EG', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  
    const timeStr = date.toLocaleTimeString('ar-EG', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
  
    return `${dateStr} - الساعة ${timeStr}`;
  }  
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName')
  constructor(private fb:FormBuilder , private financialService:FinancialService , private settingService:SettingService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
      treasuryId: ['' , Validators.required],
      receivedFrom: ['' , Validators.required],
      accountId: ['' , Validators.required],
      amount: ['' , Validators.required],
      costCenterId: ['' , Validators.required],
      description: ['']
    });    
  }
  ngOnInit(): void {
    this.getTreasury();
    this.getSupplyReceipts();
    this.getCostCenters();
    this.getAccounts();
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
          this.receiptNumber = res.results;
          this.amountInWords = this.convertToArabicWords(this.addPermissionForm.value.amount);
          console.log(res);
          console.log(this.addPermissionForm.value);
          this.getSupplyReceipts();
          this.generateCombinedPDF();
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
          accountId: this.supplyReceipt.accountId,
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
  generateCombinedPDF() {
      const opt = {
        margin: 0.5,
        filename: `supply-receipt-${this.receiptNumber}.pdf`,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2 },
        jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
      };
    
      html2pdf().from(this.printSection.nativeElement).set(opt).save();
    }
    // 
    convertToArabicWords(amount: number): string {
      const ones = ["", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة"];
      const tens = ["", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون"];
      const teens = ["عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر"];
      const hundreds = ["", "مئة", "مئتان", "ثلاثمئة", "أربعمئة", "خمسمئة", "ستمئة", "سبعمئة", "ثمانمئة", "تسعمئة"];
      const scales = ["", "ألف", "مليون", "مليار"];
      const scalesPlural = ["", "آلاف", "ملايين", "مليارات"];
    
      function groupToWords(n: number): string {
        const h = Math.floor(n / 100);
        const t = Math.floor((n % 100) / 10);
        const u = n % 10;
        const parts = [];
    
        if (h > 0) parts.push(hundreds[h]);
    
        if (t > 1) {
          if (u > 0) parts.push(`${ones[u]} و${tens[t]}`);
          else parts.push(tens[t]);
        } else if (t === 1) {
          parts.push(teens[u]);
        } else if (u > 0) {
          parts.push(ones[u]);
        }
    
        return parts.join(" و ");
      }
    
      function integerToWords(n: number): string {
        if (n === 0) return "صفر";
    
        const str = n.toString();
        const groups = [];
        for (let i = str.length; i > 0; i -= 3) {
          const start = Math.max(i - 3, 0);
          const group = parseInt(str.substring(start, i), 10);
          groups.unshift(group); 
        }
    
        const parts: string[] = [];
    
        const groupCount = groups.length;
        for (let i = 0; i < groupCount; i++) {
          const group = groups[i];
          if (group === 0) continue;
    
          const words = groupToWords(group);
          const scaleIndex = groupCount - i - 1;
          let scaleWord = "";
    
          if (scaleIndex > 0) {
            if (group === 1) {
              scaleWord = scales[scaleIndex];
            } else if (group === 2) {
              scaleWord = scales[scaleIndex] + "ان";
            } else if (group >= 3 && group <= 10) {
              scaleWord = scalesPlural[scaleIndex];
            } else {
              scaleWord = scales[scaleIndex];
            }
          }
    
          parts.push(words + (scaleWord ? ` ${scaleWord}` : ""));
        }
    
        return parts.join(" و ");
      }
    
      const integerPart = Math.floor(amount);
      const fractionPart = Math.round((amount - integerPart) * 100);
    
      let result = `فقط وقدره ${integerToWords(integerPart)} جنيه`;
      if (fractionPart > 0) {
        result += ` و${integerToWords(fractionPart)} قرش`;
      }
    
      return result + " لا غير";
    }  
    getAccountName(id: number): string {
      if (!this.accounts || !Array.isArray(this.accounts)) return '---';
      return this.accounts.find(s => s.accountId === id)?.nameAR || '---';
    }
    
    getTreasuryName(id: number): string {
      if (!this.treasuries || !Array.isArray(this.treasuries)) return '---';
      return this.treasuries.find(s => s.id === id)?.name || '---';
    }
}
