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
  @Input() searchPlaceholder: string = '';
  @Input() categoryName: string = '';
  @Output() filterChanged = new EventEmitter<FilterModel[]>();
  SelectedFilter: FilterModel[] = [];
  SearchText: string = '';
  selectedName = '';
  filterSearch: string = '';

  constructor() { }

  ngOnInit(): void {
    this.selectedName = this.filterPlaceholder;
  }

  InputSearchChange() {
    debugger;
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
    this.selectedName = this.filterPlaceholder;
    this.filterChanged.emit(this.SelectedFilter);
  }
}
