import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FilterModel } from '../../Models/Generics/PagingFilterModel';
import { FormDropdownModel } from '../../Models/Generics/FormDropdownModel';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrl: './filters.component.css'
})
export class FiltersComponent {
  @Input() filterData: FormDropdownModel[] = [];
  @Input() filterPlaceholder: string;
  @Input() showSearchText: boolean = true;
  @Input() showSelector: boolean = true;
  @Input() showDateRangeInput: boolean = false;
  @Input() showDateInput: boolean = false;
  @Input() searchPlaceholder: string = '';
  @Input() categoryName: string = '';
  @Output() filterChanged = new EventEmitter<FilterModel[]>();
  SelectedFilter: FilterModel[] = [];
  selectedDate: any;
  SearchText: string = '';
  selectedName = '';
  filterSearch: string = '';
  fromMonth: string = '';
  toMonth: string = '';
  monthValue: string = '';
  monthDifference: number = 0;

  constructor(private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.selectedName = this.filterPlaceholder;
  }

  InputSearchChange() {
    this.SelectedFilter = [];
    if (this.SearchText.length > 2) {
      const textFilter: FilterModel = {
        categoryName: 'SearchText',
        itemValue: this.SearchText,
        isChecked: true
      }
      this.SelectedFilter.push(textFilter);
      this.filterChanged.emit(this.SelectedFilter);
    }

    if (!this.SearchText)
      this.filterChanged.emit(this.SelectedFilter);

  }

  selectorFilterChange(item: FormDropdownModel) {
    this.SelectedFilter = [];
    const textFilter: FilterModel = {
      categoryName: this.categoryName,
      itemValue: item.value,
      isChecked: true
    }
    this.SelectedFilter.push(textFilter);
    this.selectedName = item.name;
    this.filterChanged.emit(this.SelectedFilter);
  }

  RemoveAllFilters() {
    this.SelectedFilter = [];
    this.SearchText = '';
    this.fromMonth = '';
    this.toMonth = '';
    this.monthValue = '';
    this.selectedDate = null;
    this.selectedName = this.filterPlaceholder;
    this.filterChanged.emit(this.SelectedFilter);
  }

  calculateDifference() {
    this.monthDifference = 0;

    if (this.fromMonth && this.toMonth) {
      const from = new Date(this.fromMonth);
      const to = new Date(this.toMonth);

    }
  }

  applyMonthRangeFilter() {
    this.SelectedFilter = [];
    if (this.fromMonth && this.toMonth) {
      const from = new Date(this.fromMonth);
      const to = new Date(this.toMonth);
      if (from > to) {
        alert('التاريخ من أصغر من التاريخ إلى');
        return;
      }
      const dateFilter: FilterModel = {
        categoryName: 'Date',
        fromDate: this.datePipe.transform(this.fromMonth, 'YYYY-MM'),
        toDate: this.datePipe.transform(this.toMonth, 'YYYY-MM'),
      }
      this.SelectedFilter.push(dateFilter);
      this.filterChanged.emit(this.SelectedFilter);
    }
  }

  applyMonthFilter() {
    this.SelectedFilter = [];
    if (this.monthValue) {
      const dateFilter: FilterModel = {
        categoryName: 'Date',
        itemValue: this.datePipe.transform(this.monthValue, 'YYYY-MM'),
      }
      this.SelectedFilter.push(dateFilter);
      this.filterChanged.emit(this.SelectedFilter);
    }
  }
}
