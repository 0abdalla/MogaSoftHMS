import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-boxs',
  templateUrl: './boxs.component.html',
  styleUrl: './boxs.component.css'
})
export class BoxsComponent {
  TitleList = ['إعدادات النظام','إعدادات الإدارة المالية','الخزائن'];
  filterForm!:FormGroup;
  accountForm!:FormGroup
  // 
  items:any[]=[];
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
      name: ['', Validators.required],
      safe: ['', Validators.required],
      branch: ['', Validators.required],
      currency: ['', Validators.required],
      openingBalance: ['', Validators.required]
    });
  }
  applyFilters(){
    this.total=this.items.length;
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
