
export interface PagingFilterModel {
    searchText?: string;
    currentPage: number;
    pageSize: number;
    filterList: FilterModel[];
    filterType?: string;
    filterItems?: string[]
}


export interface FilterModel {
    categoryName: string;
    itemId?: string;
    itemKey?: string;
    itemValue?: string;
    isChecked?: boolean;
    fromDate?: string | null;
    toDate?: string | null;
    filterType?: string;
    isVisible?: boolean;
    filterItems?: FilterModel[];
    displayOrder?: number;
}