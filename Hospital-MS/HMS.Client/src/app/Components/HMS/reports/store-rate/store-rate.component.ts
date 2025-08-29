import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReportService } from '../../../../Services/HMS/report.service';
import { FinancialService } from '../../../../Services/HMS/financial.service';
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
  
    const { storeId, StoreName, fromDate, toDate } = this.storeRateForm.value;
  
    this.reportService.getStoreRate(storeId, fromDate, toDate).subscribe(response => {
      if (response.isSuccess) {
        const groupedData: any = {};
  
        response.results.forEach((group: any) => {
          if (!groupedData[group.itemGroupId]) {
            groupedData[group.itemGroupId] = {
              itemGroupId: group.itemGroupId,
              itemGroupName: group.itemGroupName,
              items: []
            };
          }
          groupedData[group.itemGroupId].items.push(...group.items);
        });
  
        this.storeRateData = Object.values(groupedData);
  
        this.selectedStoreName = StoreName;
        this.selectedFromDate = fromDate;
        this.selectedToDate = toDate;
  
        const modal = new bootstrap.Modal(document.getElementById('storeRateReport'));
        modal.show();
      }
    });
  }
  
}
