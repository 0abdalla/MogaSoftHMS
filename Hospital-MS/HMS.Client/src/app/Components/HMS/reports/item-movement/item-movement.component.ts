import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FinancialService } from '../../../../Services/HMS/financial.service';
import { ReportService } from '../../../../Services/HMS/report.service';
declare var bootstrap : any;
import html2pdf from 'html2pdf.js';

@Component({
  selector: 'app-item-movement',
  templateUrl: './item-movement.component.html',
  styleUrl: './item-movement.component.css'
})
export class ItemMovementComponent implements OnInit {
  itemMovementForm!:FormGroup;
  stores!:any;
  items!:any;
  itemMovementData: any;
  pagingFilterModel:any={
    pageSize:100,
    searchText: '',
    currentPage:1,
  }
  constructor(private finService : FinancialService , private reportService : ReportService , private fb : FormBuilder){
    this.itemMovementForm = this.fb.group({
      id : ['' , Validators.required],
      StoreId:[''],
      FromDate : [''],
      ToDate : [''],
    })
  }
  ngOnInit(): void {
    this.getStores();
    this.getItems();
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
  getItems(){
    this.finService.getItems(this.pagingFilterModel).subscribe({
      next:(res:any)=>{
        this.items = res.results;
        console.log('Items : ' , this.items);
      },error:(err:any)=>{
        console.error('Error : ' , err)
      }
    })
  }
  showReport() {
    if (this.itemMovementForm.invalid) return;
  
    const { StoreId, id, FromDate, ToDate } = this.itemMovementForm.value;
  
    console.log('Form values:', { StoreId, id, FromDate, ToDate });
  
    this.reportService.getItemMovement(id, FromDate, ToDate, StoreId).subscribe(response => {
      if (response.isSuccess) {
        this.itemMovementData = response.results;
        const modal = new bootstrap.Modal(document.getElementById('storeMovementReportModal'));
        modal.show();
      }
    });
  }  
  printReport() {
    const element = document.getElementById('printSection');
    const options = {
      margin:       0.5,
      filename:     `تقرير حركة الصنف - ${this.itemMovementData?.itemName}.pdf`,
      image:        { type: 'jpeg', quality: 0.98 },
      html2canvas:  { scale: 2 },
      jsPDF:        { unit: 'in', format: 'a4', orientation: 'portrait' }
    };
    html2pdf().from(element).set(options).save();
  }
}
