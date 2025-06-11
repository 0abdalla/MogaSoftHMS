import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import * as bootstrap from 'bootstrap';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-boxs',
  templateUrl: './boxs.component.html',
  styleUrl: './boxs.component.css'
})
export class BoxsComponent implements OnInit{
  filterForm!:FormGroup;
  accountForm!:FormGroup
  // 
  items:any[]=[];
  selectedItem:any;
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  constructor(private fb:FormBuilder , private financialService:FinancialService){
    this.filterForm=this.fb.group({
      search:[],
    })
    this.accountForm = this.fb.group({
      accountCode:['' , Validators.required],
      name: ['', Validators.required],
      branchId: ['', Validators.required],
      currency: ['', Validators.required],
      openingBalance: [0, Validators.required ]
    });
  }
  ngOnInit(): void {
    this.getTreasuries();
  }
  getTreasuries(){
    this.financialService.getAllTreasuries(this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.filterForm.value.search).subscribe((res:any)=>{
      this.items=res.results;
      console.log(this.items);
      this.applyFilters();
    })
  }
  getTreauryById(id:number){
    this.financialService.getTreauryById(id).subscribe((res:any)=>{
      this.selectedItem=res.results;
      console.log(this.selectedItem);
      const modal = new bootstrap.Modal(document.getElementById('viewItemModal')!);
      modal.show();
    })
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
    this.getTreauryById(id);
  }
  openForm(id:number){
    const modal = new bootstrap.Modal(document.getElementById('editItemModal')!);
    modal.show();
    if(id){
      this.getTreauryById(id);
      this.accountForm.patchValue(this.selectedItem);
    }
  }
  // 
  addAccount(){
    this.financialService.addTreaury(this.accountForm).subscribe((res:any)=>{
      console.log(this.accountForm.value);
      console.log(res);
      this.accountForm.reset();
      this.getTreasuries();
    })
  }
  updateAccount(){
    this.financialService.updateTreaury(this.selectedItem.id , this.accountForm).subscribe((res:any)=>{
      console.log(this.accountForm.value);
      console.log(res);
      this.accountForm.reset();
      this.getTreasuries();
    })
  }
  deleteAccount(id:number){
    Swal.fire({
      title: 'هل انت متأكد من حذف هذه الخزينة',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف هذه الخزينة',
      cancelButtonText: 'لا'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.deleteTreaury(id).subscribe((res:any)=>{
          console.log(res);
          this.getTreasuries();
        })
      }
    })
  }
}
