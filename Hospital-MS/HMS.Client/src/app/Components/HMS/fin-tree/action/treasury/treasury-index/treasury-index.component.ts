import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-treasury-index',
  templateUrl: './treasury-index.component.html',
  styleUrl: './treasury-index.component.css'
})
export class TreasuryIndexComponent {
  TitleList = ['الإدارة المالية','حركة الخزينة','كشف حركة الخزينة'];
  closeTreasuryForm!:FormGroup;
  openTreasuryForm!:FormGroup;
  treasuryReportForm!:FormGroup
  constructor(private fb:FormBuilder){
    this.closeTreasuryForm=this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      warehouseName:[''],
      notes:['']
    });   
    this.openTreasuryForm=this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      warehouseName:[''],
      notes:['']
    }); 
    this.treasuryReportForm=this.fb.group({
      warehouseName:[''],
      treasuryNumber:['']
    }); 
  }
  submitCloseTreasury(){
    console.log(this.closeTreasuryForm.value);
    this.closeTreasuryForm.reset();
  }
  submitOpenTreasury(){
    console.log(this.openTreasuryForm.value);
    this.openTreasuryForm.reset();
  }
}
