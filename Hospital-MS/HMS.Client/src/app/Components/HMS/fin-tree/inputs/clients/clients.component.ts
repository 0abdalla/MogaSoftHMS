import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrl: './clients.component.css'
})
export class ClientsComponent {
  filterForm!:FormGroup;
      clientForm!:FormGroup
      // 
      clients:any[]=[];
      total:number=0;
      pagingFilterModel:any={
        pageSize:16,
        currentPage:1,
      }
      constructor(private fb:FormBuilder){
        this.filterForm=this.fb.group({
          SearchText:[],
        })
        this.clientForm = this.fb.group({
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
          bankAccountNumber: [''],
          notes: [''],
          creditLimit: [0, [Validators.min(0)]],
          paymentRate: [''],
          accountManager: ['']
        });
      }
      applyFilters(){
        this.total=this.clients.length;
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
        this.clientForm.reset();
      }
}
