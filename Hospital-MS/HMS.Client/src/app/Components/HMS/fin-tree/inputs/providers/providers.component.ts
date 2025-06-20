import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-providers',
  templateUrl: './providers.component.html',
  styleUrl: './providers.component.css'
})
export class ProvidersComponent {
  filterForm!:FormGroup;
    providerForm!:FormGroup
    TitleList = ['المشتريات','الموردين'];
    // 
    providers:any[]=[];
    total:number=0;
    pagingFilterModel:any={
      pageSize:16,
      currentPage:1,
    }
    constructor(private fb:FormBuilder){
      this.filterForm=this.fb.group({
        SearchText:[],
      })
      this.providerForm = this.fb.group({
        accountCode: ['', Validators.required],
        name: ['', Validators.required],
        nameEn: ['', Validators.required],
        managerName1: [''],
        managerName2: [''],
        job: [''],
        type: [''],
        region: [''],
        phone1: [''],
        phone2: [''],
        taxNumber: [''],
        phoneAlt1: [''],
        fax1: [''],
        email: ['', [Validators.email]],
        networkLink: [''],
        notes: [''],
        creditLimit: [0, [Validators.min(0)]],
        paymentRate: ['100'],
        accountManager: ['']
      });      
    }
    applyFilters(){
      this.total=this.providers.length;
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
    addProvider(){
      this.providerForm.reset();
    }
}
