import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FilterModel } from '../../Models/Generics/PagingFilterModel';
import { FormDropdownModel } from '../../Models/Generics/FormDropdownModel';

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
  @Input() showDateInput: boolean = false;
  @Input() searchPlaceholder: string = '';
  @Input() categoryName: string = '';
  @Output() filterChanged = new EventEmitter<FilterModel[]>();
  SelectedFilter: FilterModel[] = [];
  selectedDate: any;
  SearchText: string = '';
  selectedName = '';
  filterSearch: string = '';

  constructor() { }

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
    this.selectedDate = null;
    this.selectedName = this.filterPlaceholder;
    this.filterChanged.emit(this.SelectedFilter);
  }

  changeDate(range: any) {
    this.SelectedFilter = [];
    if (range && range.endDate) {
      const dateFilter: FilterModel = {
        categoryName: 'Date',
        fromDate: range.startDate.format('YYYY-MM-DD'),
        toDate: range.endDate.format('YYYY-MM-DD'),
      }
      this.SelectedFilter.push(dateFilter);
      this.filterChanged.emit(this.SelectedFilter);
    }
  }
}
