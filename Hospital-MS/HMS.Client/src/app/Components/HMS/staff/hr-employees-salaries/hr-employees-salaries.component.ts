import { Component } from '@angular/core';
import { GeneralSelectorModel } from '../../../../Models/Generics/GeneralSelectorModel';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { EmployeeSalarySummaryModel } from '../../../../Models/HMS/Staff/EmployeeSalarySummaryModel';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { StaffService } from '../../../../Services/HMS/staff.service';

@Component({
  selector: 'app-hr-employees-salaries',
  templateUrl: './hr-employees-salaries.component.html',
  styleUrl: './hr-employees-salaries.component.css'
})
export class HrEmployeesSalariesComponent {
  TitleList = ['الموارد البشرية', 'الحضور والانصراف'];
  URLs: any[] = [];
  Branches: any[] = [];
  branchSelectorData: GeneralSelectorModel[] = [];
  employeeSelectorData: GeneralSelectorModel[] = [];
  yearsSelectorData: GeneralSelectorModel[] = [];
  arabicMonths = [
    "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
    "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
  ];
  monthsSelectorData: GeneralSelectorModel[] = this.arabicMonths.map((name, index) => ({
    value: index + 1,
    name
  }));
  filterList: FilterModel[] = [];
  showLoader: boolean = false;

  selectedYear: number;
  selectedMonth: number;
  selectedEmployeeId: number;
  selectedBranchId: number;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    pageSize: 10,
    currentPage: 1,
    searchText: ''
  }
  pagedResponseModel: PagedResponseModel<EmployeeSalarySummaryModel[]> = {
    results: []
  };

  constructor(private modalService: NgbModal,
    private staffService: StaffService,
    private router: Router) { }

  ngOnInit(): void {
    this.loadSelectors();
  }

  loadSelectors() {
    this.staffService.GetActiveEmployeesSelector().subscribe((data: GeneralSelectorModel[]) => {
      this.employeeSelectorData = data;
    });
    const currentYear = new Date().getFullYear();
    this.selectedYear = currentYear;
    for (let i = currentYear - 5; i <= currentYear; i++) {
      this.yearsSelectorData.push({ value: i, name: i });
    }
    this.yearsSelectorData.reverse();
  }
  search() {
    this.pagedResponseModel.results = [];
    this.PagingFilter.currentPage = 1;
    this.pagedResponseModel.totalCount = 0;
    this.getSalaries_Data();
  }
  getSalaries_Data() {
    this.mapFilters();
    if (!this.selectedYear || !this.selectedMonth) {
      alert('يرجى تحديد السنة والشهر');
      return;
    }
    this.showLoader = true;
    this.staffService.GetEmployeeSalarySummary(this.selectedYear, this.selectedMonth, this.PagingFilter).subscribe(data => {
      this.pagedResponseModel.results = data?.results;
      this.pagedResponseModel.totalCount = data?.totalCount;
      this.showLoader = false;
    }, err => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });
  }

  mapFilters() {
    this.PagingFilter.filterList = [];
    if (this.selectedEmployeeId) {
      this.PagingFilter.filterList.push({ categoryName: 'EmployeeId', itemId: this.selectedEmployeeId?.toString() })
    }
  }
  pageChanged(obj: any) {
    this.PagingFilter.currentPage = obj.page;
    this.getSalaries_Data();
  }
}
