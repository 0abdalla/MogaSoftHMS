import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-bank-index',
  templateUrl: './bank-index.component.html',
  styleUrl: './bank-index.component.css'
})
export class BankIndexComponent {
  bankReportForm!:FormGroup;
  bankLockForm!:FormGroup
  constructor(private fb : FormBuilder){
    this.bankReportForm = this.fb.group({
      dateFrom : [''],
      dateTo : [''],
      bank : ['']
    })
    this.bankLockForm = this.fb.group({
      dateFrom : [''],
      dateTo : [''],
      bank : ['']
    })
  }
  submitClosebank(){
    console.log(this.bankReportForm.value);
  }
}
