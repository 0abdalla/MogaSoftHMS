import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReportService } from '../../../../Services/HMS/report.service';
import { FinancialService } from '../../../../Services/HMS/financial.service';
import html2pdf from 'html2pdf.js';
declare var bootstrap : any;
@Component({
  selector: 'app-store-rate',
  templateUrl: './store-rate.component.html',
  styleUrl: './store-rate.component.css'
})
export class StoreRateComponent implements OnInit {
  storeRateForm : FormGroup;
  stores : any[] = [];
  storeRateData!:any;
  selectedStoreName!:any;
  selectedFromDate!:any;
  selectedToDate!:any;
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
  constructor(private reportService : ReportService , private finService : FinancialService , private fb : FormBuilder){
    this.storeRateForm = this.fb.group({
      storeId  : [null , Validators.required],
      fromDate : [null , Validators.required],
      toDate : [null , Validators.required]
    })
  }

  ngOnInit(): void {
    this.getStores();
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
  showStoreRateReport() {
    if (this.storeRateForm.invalid) return;
  
    const { storeId, fromDate, toDate } = this.storeRateForm.value;
  
    this.reportService.getStoreRate(storeId, fromDate, toDate).subscribe(response => {
      if (response.isSuccess) {
        const groupedData: any = {};
  
        response.results.forEach((mainGroup: any) => {
          if (!groupedData[mainGroup.mainGroupId]) {
            groupedData[mainGroup.mainGroupId] = {
              mainGroupId: mainGroup.mainGroupId,
              mainGroupName: mainGroup.mainGroupName,
              itemGroups: []
            };
          }
  
          mainGroup.itemGroups.forEach((itemGroup: any) => {
            groupedData[mainGroup.mainGroupId].itemGroups.push(itemGroup);
          });
        });
  
        this.storeRateData = Object.values(groupedData);
  
        this.selectedStoreName = this.stores.find((s: any) => s.id === storeId)?.name;
        this.selectedFromDate = fromDate;
        this.selectedToDate = toDate;
  
        const modal = new bootstrap.Modal(document.getElementById('storeRateReport')!);
        modal.show();
      }
    });
  }
  
  printReport() {
    const element = document.getElementById('printSection');
    const options = {
      margin:       0.5,
      filename:     `تقرير تقييم المخزون - ${this.selectedStoreName}.pdf`,
      image:        { type: 'jpeg', quality: 0.98 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
    html2pdf().from(element).set(options).save();
  }
}
