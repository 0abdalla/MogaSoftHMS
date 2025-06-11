import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FinancialService } from '../../../../../Services/HMS/financial.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-banks',
  templateUrl: './banks.component.html',
  styleUrl: './banks.component.css'
})
export class BanksComponent implements OnInit {
  filterForm!:FormGroup;
  accountForm!:FormGroup
  // 
  banks!:any;
  total:number=0;
  pagingFilterModel:any={
    pageSize:16,
    currentPage:1,
  }
  constructor(private fb:FormBuilder , private finService : FinancialService){
    this.filterForm=this.fb.group({
      SearchText:[],
    })
    this.accountForm = this.fb.group({
      name: ['', Validators.required],
      code: ['', Validators.required],
      accountNumber: ['', Validators.required],
      currency: ['', Validators.required],
      initialBalance: ['', Validators.required]
    });
  }
  ngOnInit(): void {
    this.getAllBanks();
  }
  getAllBanks(){
    this.finService.getAllBanks(this.pagingFilterModel.currentPage,this.pagingFilterModel.pageSize,this.filterForm.value.search).subscribe((res:any)=>{
      this.banks=res.results;
      console.log(this.banks);
      this.applyFilters();
    })
  }
  applyFilters(){
    this.total=this.banks.length;
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
    this.finService.addBank(this.accountForm).subscribe((res:any)=>{
      console.log(res);
      this.accountForm.reset();
      this.getAllBanks();
    })
  }
  // updateAccount(){
  //   this.finService.updateBank(this.selectedItem.id , this.accountForm).subscribe((res:any)=>{
  //     console.log(res);
  //     this.accountForm.reset();
  //     this.getAllBanks();
  //   })
  // }
  deleteAccount(id:number){
    Swal.fire({
      title: 'هل انت متأكد من حذف هذه الحساب',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف هذه الحساب',
      cancelButtonText: 'لا'
    }).then((result) => {
      if (result.isConfirmed) {
        this.finService.deleteBank(id).subscribe((res:any)=>{
          console.log(res);
          this.getAllBanks();
        })
      }
    })
  }
}
