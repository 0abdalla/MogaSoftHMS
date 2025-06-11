import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder , Validators } from '@angular/forms';
import { FinancialService } from '../../../../../../Services/HMS/financial.service';

@Component({
  selector: 'app-add-notice',
  templateUrl: './add-notice.component.html',
  styleUrl: './add-notice.component.css'
})
export class AddNoticeComponent implements OnInit {
  filterForm!:FormGroup;
  addNoticeGroup!:FormGroup
  // 
  adds:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  constructor(private fb:FormBuilder , private financialService:FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addNoticeGroup = this.fb.group({
      date: ['' , [Validators.required]],
      documentNumber: ['' , [Validators.required]],
      bankId: ['' , [Validators.required]],
      accountId: ['' , [Validators.required]],
      checkNumber: ['' , [Validators.required]],
      amount: [0 , [Validators.required]],
      notes: ['']
    });    
  }
  ngOnInit(): void {
    this.getAllAdditionNotifications();
  }
  getAllAdditionNotifications(){
    this.financialService.getAllAdditionNotifications(this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.filterForm.value.SearchText).subscribe((res:any)=>{
      this.adds=res.results;
      this.total=res.total;
      console.log(this.adds);
    })
  }
  applyFilters(){
    this.total=this.adds.length;
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
  submitPermission(){
    this.financialService.addAdditionNotification(this.addNoticeGroup).subscribe((res:any)=>{
      console.log(res);
    })
    this.addNoticeGroup.reset();
  }
}
