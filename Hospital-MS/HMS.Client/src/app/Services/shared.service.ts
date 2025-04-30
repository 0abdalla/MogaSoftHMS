import { Injectable } from '@angular/core';
import { FilterModel } from '../Models/Generics/PagingFilterModel';
declare var bootstrap: any;

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor() { }

  CreateFilterList(name: string, value: any): FilterModel[] {
    let filterList: FilterModel[] = [];
    let filterModel: FilterModel = {
      categoryName: name,
      itemValue: value
    };
    filterList.push(filterModel);
    return filterList;
  }

  closeModal(ModalId: string) {
    const modalElement = document.getElementById(ModalId);
    if (modalElement) {
      const modalInstance = bootstrap.Modal.getInstance(modalElement) 
        || new bootstrap.Modal(modalElement);
      modalInstance.hide();
    }
  }
}
