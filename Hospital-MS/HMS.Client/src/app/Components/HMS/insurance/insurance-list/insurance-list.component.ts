import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
import { InsuranceCompany } from '../../../../Models/HMS/insurance';
import { trigger, transition, style, animate } from '@angular/animations';
import { Router } from '@angular/router';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
declare var bootstrap: any;

@Component({
  selector: 'app-insurance-list',
  templateUrl: './insurance-list.component.html',
  styleUrl: './insurance-list.component.css',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-in', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        animate('200ms ease-out', style({ opacity: 0 })),
      ])
    ])
  ],
})
export class InsuranceListComponent implements OnInit {
  filterForm!: FormGroup;
  TitleList = ['إعدادات النظام' , 'وكلاء التأمين'];
  // 
  insurnaces!: InsuranceCompany[];
  // 
  pageSize = 16;
  currentPage = 1;
  total = 0;
  fixed = Math.ceil(this.total / this.pageSize);
  // 
  selectedInsurance!: any;
  // 
  isFilter = true;
  pagingFilterModel: PagingFilterModel = {
      searchText: '',
      currentPage: 1,
      pageSize: 16,
      filterList: []
    };
  constructor(private fb: FormBuilder, private insuranceService: InsuranceService, private router: Router) {
    this.filterForm = this.fb.group({
      Search: ['', Validators.required],
    });
  }
  ngOnInit(): void {
    this.getAllInsurances();
  }
  applyFilters() { }
  resetFilters() { }
  // 
  getAllInsurances() {
    this.insuranceService.getAllInsurances().subscribe({
      next: (res: any) => {
        this.insurnaces = res.results;
        this.total = res.totalCount;
        console.log(res);
        console.log(this.insurnaces);
      },
      error: (err) => {
      }
    })
  }
  getInsuranceById(id: number) {
    this.insuranceService.getInsuranceById(id).subscribe({
      next: (res: any) => {
        this.selectedInsurance = res.results;
      },
      error: (err) => {
      }
    })
  }
  openInsuranceModal(id: number) {
    this.getInsuranceById(id);
  }

  editInsurance(companyId: number): void {
    const modalElement = document.getElementById('insuranceModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      if (modal) {
        modal.hide();
      }
    }

    this.router.navigate(['hms/insurance/add-insurance', companyId]);
  }
  // 
  onPageChange(event: number) {
    this.currentPage = event;
  }
  print() {
    const printContents = document.getElementById('pdfContent')?.innerHTML;
    const originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
  }
  exportToPDF() {
    const printContents = document.getElementById('pdfContent')?.innerHTML;
    const originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
  }
  filterChecked(filters: FilterModel[]) {
        this.pagingFilterModel.currentPage = 1;
        this.pagingFilterModel.filterList = filters;
        if (filters.some(i => i.categoryName == 'SearchText'))
          this.pagingFilterModel.searchText = filters.find(i => i.categoryName == 'SearchText')?.itemValue;
        else
          this.pagingFilterModel.searchText = '';
        this.getAllInsurances();
  }
}
