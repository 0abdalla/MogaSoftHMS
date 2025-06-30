import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';

@Component({
  selector: 'app-issue-items',
  templateUrl: './issue-items.component.html',
  styleUrl: './issue-items.component.css'
})
export class IssueItemsComponent implements OnInit {
  filterForm!:FormGroup;
  addPermissionForm!:FormGroup
   TitleList = ['المخازن','إذن صرف'];
  // 
  adds:any[]=[];
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  allItems: any[] = [];

  constructor(private fb:FormBuilder , private financialService:FinancialService){

    this.filterForm=this.fb.group({
      SearchText:[],
      type:[''],
      responsible:[''],
    })
    this.addPermissionForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      documentNumber: ['0'],
      warehouseName: [''],
      supplierName: [''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      notes: ['']
    });    
  }
  ngOnInit(): void {
    this.getItems();
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      quantity: [1, Validators.required],
      notes: ['']
    });
  }
  get items(): FormArray {
    return this.addPermissionForm.get('items') as FormArray;
  }
  
  addItemRow() {
    this.items.push(this.createItemGroup());
  }
  
  removeItemRow(index: number) {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
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
    console.log(this.addPermissionForm.value);
    this.addPermissionForm.reset();
  }
  getItems(){
    this.financialService.getItems(this.pagingFilterModel).subscribe((res:any)=>{
      this.allItems=res.results;
      this.total=res.count;
    })
  }
}
