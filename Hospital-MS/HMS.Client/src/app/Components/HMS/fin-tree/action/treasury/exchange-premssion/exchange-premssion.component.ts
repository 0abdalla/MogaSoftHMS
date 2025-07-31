import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';
import { FilterModel } from '../../../../../../Models/Generics/PagingFilterModel';
import { SettingService } from '../../../../../../Services/HMS/setting.service';
import html2pdf from 'html2pdf.js';
import { todayDateValidator } from '../../../../../../validators/today-date.validator';
declare var bootstrap:any;

@Component({
  selector: 'app-exchange-premssion',
  templateUrl: './exchange-premssion.component.html',
  styleUrl: './exchange-premssion.component.css'
})
export class ExchangePremssionComponent implements OnInit {
  // @ViewChild('receiptSection') receiptSection!: ElementRef;
  // @ViewChild('journalSection') journalSection!: ElementRef;
  @ViewChild('printSection') printSection!: ElementRef;
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName')

  filterForm!:FormGroup;
  TitleList = ['الإدارة المالية','حركة الخزينة','إذن صرف نقدي'];
  exPermissionForm!:FormGroup
  // 
  exchanges:any[]=[];
  treasuries:any[]=[];
  items:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  // 
  isFilter:boolean=true
  receiptNumber!:any
  amountInWords!:any
  // 
  accounts!:any[]
  costCenters!:any[]
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
  constructor(private fb:FormBuilder , private financialService:FinancialService , private settingsService : SettingService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.exPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10) , [todayDateValidator]],
      treasuryId: ['' , Validators.required],
      dispenseTo: ['' , Validators.required],
      accountId:['' , Validators.required],
      costCenterId:['' , Validators.required],
      amount: [1 , [Validators.required , Validators.min(1)]],
      notes: ['']
    });    
  }
  ngOnInit(): void {
    this.getDispensePermissions();
    this.getTreasuries();
    this.getItems();
    this.getAccounts();
    this.getCostCenters();
  }
  applyFilters(){
    this.total=this.exchanges.length;
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
  getDispensePermissions(){
    this.financialService.getDispensePermissions(this.pagingFilterModel).subscribe((res:any)=>{
      this.exchanges=res.results;
      console.log('exchanges',this.exchanges);
      this.total=res.count;
      this.applyFilters();
    })
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res:any)=>{
      this.items=res.results;
      console.log('items',this.items);
      this.total=res.count;
    })
  }
  getTreasuries(){
    this.financialService.getTreasuries(this.pagingFilterModel).subscribe((res:any)=>{
      this.treasuries=res.results;
      console.log('treasuries',this.treasuries);
      this.total=res.count;
    })
  }
  // getSuppliers(){
  //   this.financialService.getSuppliers(this.pagingFilterModel).subscribe((res:any)=>{
  //     this.suppliers=res.results;
  //     console.log(this.suppliers);
  //     this.total=res.count;
  //   })
  // }
  // getStores(){
  //   this.financialService.getStores(this.pagingFilterModel).subscribe((res:any)=>{
  //     this.stores=res.results;
  //     console.log(this.stores);
  //     this.total=res.count;
  //   })
  // }
  // getPurchaseRequests(){
  //   this.financialService.getPurchaseRequests(this.pagingFilterModel).subscribe((res:any)=>{
  //     this.purchaseRequests=res.results;
  //     console.log(this.purchaseRequests);
  //     this.total=res.count;
  //   })
  // }
  filterChecked(filters: FilterModel[]) {
          this.pagingFilterModel.currentPage = 1;
          this.pagingFilterModel.filterList = filters;
          if (filters.some(i => i.categoryName == 'SearchText'))
            this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
          else
            this.pagingFilterModel.searchText = '';
          this.getDispensePermissions();
          this.getTreasuries();
  }
  // 
  isEditMode : boolean = false;
  cuerrentExchangePermissionId: number | null = null;
  restrictionNumber!:any;
  addPermission() {
    if (this.exPermissionForm.invalid) {
      this.exPermissionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.exPermissionForm.value;
    if (this.isEditMode && this.cuerrentExchangePermissionId) {
      
      this.financialService.updateDispensePermission(this.cuerrentExchangePermissionId, formData).subscribe({
        next: () => {
          this.getDispensePermissions();
          console.log('تم التعديل بنجاح');
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.financialService.addDispensePermission(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          this.receiptNumber = res.results.id;
          this.restrictionNumber = res.results.restrictionNumber
          this.amountInWords = this.convertToArabicWords(formData.amount);
          this.getDispensePermissions();
          this.generateCombinedPDF();
          // this.exPermissionForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  permission!:any;
  editPermission(id: number) {
    this.isEditMode = true;
    this.cuerrentExchangePermissionId = id;
  
    this.financialService.getDispensePermissionsById(id).subscribe({
      next: (data) => {
        this.permission=data.results;
        console.log(this.permission);
        this.exPermissionForm.patchValue({
          supplierId: this.permission.supplierId,
          documentNumber: this.permission.documentNumber,
          permissionDate: this.permission.permissionDate,
          notes: this.permission.notes,
          items: this.permission.items,
          storeId: this.permission.storeId,
          purchaseRequestId: this.permission.purchaseRequestId
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addPermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
      }
    });
  }
  deletePermission(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف طلب الشراء؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteDispensePermission(id).subscribe({
          next: () => {
            this.getDispensePermissions();
            console.log('تم الحذف بنجاح');
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
  // 
  getAccounts() {
    this.settingsService.GetAccountTreeHierarchicalData('').subscribe({
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

  getCostCenters() {
    this.settingsService.GetCostCenterTreeHierarchicalData('').subscribe({
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
  // generateReceiptPDF() {
  //   const options = {
  //     margin: 0.5,
  //     filename: `receipt-${new Date().getTime()}.pdf`,
  //     image: { type: 'jpeg', quality: 0.98 },
  //     html2canvas: { scale: 2 },
  //     jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
  //   };
  
  //   html2pdf().from(this.receiptSection.nativeElement).set(options).save();
  // }

  // generateJournalPDF() {
  //   const options = {
  //     margin: 0.5,
  //     filename: `journal-${this.receiptNumber}.pdf`,
  //     image: { type: 'jpeg', quality: 0.98 },
  //     html2canvas: { scale: 2 },
  //     jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
  //   };
  
  //   html2pdf().from(this.journalSection.nativeElement).set(options).save();
  // }
  generateCombinedPDF() {
    const opt = {
      margin: 0.5,
      filename: `exchange-permission-${this.receiptNumber}.pdf`,
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
      // 
      printedpermssion!:any;
        printpermsission(id: number) {
          this.financialService.getDispensePermissionsById(id).subscribe(res => {
            if (res?.isSuccess) {
              this.printedpermssion = res.results;
              this.amountInWords = this.convertToArabicWords(this.printedpermssion.amount)
              setTimeout(() => {
                const element = document.getElementById('printablePermsission');
                const options = {
                  margin:       0.5,
                  filename:     `إذن-صرف-رقم-${this.printedpermssion.id}.pdf`,
                  image:        { type: 'jpeg', quality: 0.98 },
                  html2canvas:  { scale: 2 },
                  jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
                };
                html2pdf().from(element).set(options).save();
              }, 300);
            }else{
              console.log('error');
              
            }
          });
        }
}
