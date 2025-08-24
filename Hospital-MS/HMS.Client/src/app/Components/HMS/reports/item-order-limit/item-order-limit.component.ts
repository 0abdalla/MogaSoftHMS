import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReportService } from '../../../../Services/HMS/report.service';
import { FinancialService } from '../../../../Services/HMS/financial.service';
import html2pdf from 'html2pdf.js';
declare var bootstrap : any
@Component({
  selector: 'app-item-order-limit',
  templateUrl: './item-order-limit.component.html',
  styleUrl: './item-order-limit.component.css'
})
export class ItemOrderLimitComponent implements OnInit {
  stores:any;
  itemLimitData!:any;
  reportSummary!:any;
  itemOrderLimitForm!:FormGroup;
  selectedStoreName!:any;
  pagingFilterModel:any={
    pageSize:100,
    searchText: '',
    currentPage:1,
  }
  userName = sessionStorage.getItem('firstName') + ' ' + sessionStorage.getItem('lastName')
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
  constructor(private finService : FinancialService , private reportService : ReportService , private fb : FormBuilder){
    this.itemOrderLimitForm = this.fb.group({
      StoreId : ['' , Validators.required]
    })
  }
  ngOnInit(): void {
    this.getStores()
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
  showReport() {
    if (this.itemOrderLimitForm.invalid) return;
  
    const { StoreId, StoreName } = this.itemOrderLimitForm.value;
  
    this.reportService.getItemOrderLimit(StoreId).subscribe(response => {
      if (response.isSuccess) {
        const groupedData: any = {};
        let totalItems = 0;
        let belowLimitCount = 0;
  
        response.results.forEach((group: any) => {
          if (!groupedData[group.itemGroupId]) {
            groupedData[group.itemGroupId] = {
              itemGroupId: group.itemGroupId,
              itemGroupName: group.itemGroupName,
              items: []
            };
          }
  
          group.items.forEach((item: any) => {
            totalItems++;
            if (item.balance < item.orderLimit) belowLimitCount++;
            groupedData[group.itemGroupId].items.push(item);
          });
        });
  
        this.itemLimitData = Object.values(groupedData);
  
        this.reportSummary = {
          totalItems,
          belowLimitCount,
          affectedGroups: this.itemLimitData.filter((g: any) =>
            g.items.some((i: any) => i.balance < i.orderLimit)
          ).length
        };
  
        this.selectedStoreName = StoreName;
        const modal = new bootstrap.Modal(document.getElementById('itemOrderLimit'));
        modal.show();
      }
    });
  }
  printReport() {
    const element = document.getElementById('printSection');
    const options = {
      margin:       0.5,
      filename:     `تقرير حد الطلب - ${this.itemOrderLimitForm.value.StoreName}.pdf`,
      image:        { type: 'jpeg', quality: 0.98 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
    html2pdf().from(element).set(options).save();
  }
}
