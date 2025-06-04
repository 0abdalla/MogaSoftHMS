import { Component } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-restrictions',
  templateUrl: './restrictions.component.html',
  styleUrl: './restrictions.component.css'
})
export class RestrictionsComponent {
  filterForm!:FormGroup;
  restrictionForm!:FormGroup
    // 
    restrictions:any[]=[];
    total:number=0;
    pagingFilterModel:any={
      pageSize:16,
      currentPage:1,
    }
    constructor(private fb:FormBuilder){
      this.filterForm=this.fb.group({
        SearchText:[],
      })
      this.restrictionForm =this.fb.group({
        date : [''],
        entryType : [''],
        direction : [''],
        description : [''],
        costCenter : [''],
        account1: [''],
        debit1: ['0'],
        credit1: ['0'],
        account2: [''],
        debit2: ['0'],
        credit2: ['0'],
      })
    }
    applyFilters(){
      this.total=this.restrictions.length;
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
    openRestriction(id:number){
      
    }
    // 
    addRestriction(){
      this.restrictionForm.reset();
    }
}
