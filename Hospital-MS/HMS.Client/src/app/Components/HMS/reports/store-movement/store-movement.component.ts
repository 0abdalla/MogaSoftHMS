import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FinancialService } from '../../../../Services/HMS/financial.service';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { ReportService } from '../../../../Services/HMS/report.service';
declare var bootstrap : any;
import html2pdf from 'html2pdf.js';

@Component({
  selector: 'app-store-movement',
  templateUrl: './store-movement.component.html',
  styleUrl: './store-movement.component.css'
})
export class StoreMovementComponent implements OnInit {
  @ViewChild('printSection') printSection!: ElementRef;

  storeMovementForm!:FormGroup;
  stores!:any;
  mainGroups!:any;
  itemGroups!:any;
  filteredItemGroups!:any;
  // 
  pagingFilterModel:any={
    pageSize:100,
    searchText: '',
    currentPage:1,
  }
  constructor(private finService : FinancialService , private reportService : ReportService , private fb : FormBuilder){
    this.storeMovementForm = this.fb.group({
      storeId:['' , Validators.required],
      mainGroupId:[''],
      itemGroupId:[''],
      from:['' ,Validators.required],
      to:['' , Validators.required]
    })
  }
  ngOnInit(): void {
    this.getStores();
    this.getMainGroups();
    this.getItemGroups();
  }
  getStores(){
    this.finService.getStores(this.pagingFilterModel).subscribe({
      next:(res:any)=>{
        this.stores = res.results;
        console.log('Stores : ' , this.stores);
      },error:(err:any)=>{
        console.error('Error : ' , err)
      }
    })
  }
  getMainGroups(){
    this.finService.getMainGroups(this.pagingFilterModel).subscribe({
      next:(res:any)=>{
        this.mainGroups = res.results;
        console.log('Main Groups : ' , this.mainGroups);
      },error:(err:any)=>{
        console.error('Error : ' , err)
      }
    })
  }
  getItemGroups(){
    this.finService.getItemsGroups(this.pagingFilterModel).subscribe({
      next:(res:any)=>{
        this.itemGroups = res.results;
        this.filteredItemGroups = this.itemGroups;
        console.log('Item Groups : ' , this.itemGroups);
      },error:(err:any)=>{
        console.error('Error : ' , err)
      }
    })
  }
  onMainGroupChange(mainGroupId: number): void {
    if (mainGroupId) {
      this.filteredItemGroups = this.itemGroups.filter(item => item.mainGroupId === mainGroupId);
      this.storeMovementForm.get('itemGroupId')?.reset();
    } else {
      this.filteredItemGroups = this.itemGroups;
      this.storeMovementForm.get('itemGroupId')?.reset();
    }
  }
  // 
  storeMovementData!:any;
  selectedStore!:any;
  selectedMainGroup!:any;
  selectedItemGroup!:any;
  materialIssuePermissionNumber!:any;
  lastReceiptPermissionNumber!:any;
  showReport() {
    const { storeId, from, to, mainGroupId, itemGroupId } = this.storeMovementForm.value;
    this.reportService.getStoreMovement(storeId, from, to, mainGroupId, itemGroupId).subscribe({
      next: (res: any) => {
        this.selectedStore = this.stores.find((s:any) => s.id === storeId)?.name;
        this.selectedMainGroup = this.mainGroups.find((mg:any) => mg.id === mainGroupId)?.name;
        this.selectedItemGroup = this.itemGroups.find((ig:any) => ig.id === itemGroupId)?.name;
        this.materialIssuePermissionNumber = res.results[0].materialIssuePermissionNumber
        this.lastReceiptPermissionNumber = res.results[0].lastReceiptPermissionNumber
        console.log(this.materialIssuePermissionNumber);
        console.log(this.lastReceiptPermissionNumber);
        this.processStoreMovementData(res.results);
        console.log(res.results);
        const reportModal = new bootstrap.Modal(document.getElementById('storeMovementReportModal')!);
        reportModal.show();
      },
      error: (err) => {
        console.error('فشل تحميل حركة المخزن:', err);
        alert('حدث خطأ أثناء تحميل التقرير');
      }
    });
  }
  groupedItemGroups!:any;
  processStoreMovementData(rawData: any[]) {
    const mergedGroupsMap = new Map<number, any>(); 
  
    rawData.forEach(mainGroup => {
      mainGroup.itemGroups.forEach(itemGroup => {
        const existing = mergedGroupsMap.get(itemGroup.itemGroupId);
  
        if (existing) {
          existing.items.push(...itemGroup.items);
        } else {
          mergedGroupsMap.set(itemGroup.itemGroupId, {
            itemGroupId: itemGroup.itemGroupId,
            itemGroupName: itemGroup.itemGroupName,
            items: [...itemGroup.items]
          });
        }
      });
    });
    this.groupedItemGroups = Array.from(mergedGroupsMap.values());
  }
  printReport() {
    setTimeout(() => {
      const section = this.printSection.nativeElement as HTMLElement;
      const modalContent = document.querySelector('#storeMovementReportModal .modal-content');
      if (modalContent && section) {
        const clonedContent = modalContent.cloneNode(true) as HTMLElement;
        clonedContent.querySelectorAll('.modal-footer, .btn-close').forEach(el => el.remove());
        section.appendChild(clonedContent);
        const options = {
          margin: 0.5,
          filename: `تقرير حركة المخزن - ${this.selectedStore}.pdf`,
          image: { type: 'jpeg', quality: 0.98 },
          html2canvas: { scale: 2 },
          jsPDF: { unit: 'in', format: 'a4', orientation: 'landscape' }
        };
        html2pdf().from(section).set(options).save().then(() => {
          section.style.display = 'none';
        });
      }
    }, 300);
  }
  
  
  // printReport() {
  //   const element = document.getElementById('storeMovementReportModal');
  //   if (!element) {
  //     console.error('العنصر غير موجود');
  //     return;
  //   }
  //   element.style.display = 'block';
  //   const opt = {
  //     margin: 0.5,
  //     filename: 'تقرير حركة المخزن - ' + this.selectedStore + '.pdf',
  //     image: { type: 'jpeg', quality: 0.98 },
  //     html2canvas: { scale: 2 },
  //     jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
  //   };
  //   html2pdf().set(opt).from(element).save().then(() => {
  //     element.style.display = 'none';
  //   }).catch(err => {
  //     console.error('حدث خطأ أثناء توليد PDF:', err);
  //     element.style.display = 'none';
  //   });
  // }
}
