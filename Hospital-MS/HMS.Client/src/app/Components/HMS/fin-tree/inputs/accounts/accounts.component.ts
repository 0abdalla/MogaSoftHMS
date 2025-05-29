import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrl: './accounts.component.css'
})
export class AccountsComponent {
  filterForm!:FormGroup;
  accountForm!:FormGroup
  // 
  accounts:any[] = [];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  constructor(private fb:FormBuilder){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.accountForm = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      level: ['', Validators.required],
      mainAccount: [''],
      isMain: [false],
      type: ['', Validators.required],
      nature: ['', Validators.required],
      openingBalance: [0],
      notes: ['']
    });
    
  }
  applyFilters(){
    this.total=this.accounts.length;
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
  openItem(id:number){
    
  }
  // 
  addAccount(){
    this.accountForm.reset();
  }
}
