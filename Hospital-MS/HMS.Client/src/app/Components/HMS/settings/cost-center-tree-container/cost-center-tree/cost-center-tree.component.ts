import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { CostCenterTreeModel } from '../../../../../Models/Generics/CostCenter';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SettingService } from '../../../../../Services/HMS/setting.service';

@Component({
  selector: 'app-cost-center-tree',
  templateUrl: './cost-center-tree.component.html',
  styleUrl: './cost-center-tree.component.css'
})
export class CostCenterTreeComponent {
  @ViewChild('deleteModal') deleteModal: HTMLElement;
  @Input() isParentCostCenter: boolean = false;
  @Input() reloadData: boolean = false;
  @Output() selectedCostCenter = new EventEmitter<any>();

  CostCenterTreeData: any[] = [];
  CostCenterData: any[] = [];
  showLoader: boolean;
  SearchText = '';
  isSearchMode = false;
  parentCostCentersList: any[] = [];
  CostCenterTypes: any[] = [];
  selectedCostCenterId: number;
  showDeleteLoader: boolean = false;

  costCenterTreeModel: CostCenterTreeModel = {} as CostCenterTreeModel;

  constructor(private settingsService: SettingService, private modalService: NgbModal) { }


  ngOnInit(): void {
    this.loadData();

  }
  ngOnChanges(changes): void {
    if (changes && changes.reloadData && !changes.reloadData.firstChange) {
      this.loadData();
    }
  }


  selectCostCenter(costCenter: CostCenterTreeModel) {

    if (costCenter.isDeleteAction) {
      this.openDeleteModal(costCenter.costCenterId);
      return
    }
    this.selectedCostCenter.emit(costCenter);
  }

  loadData() {

    this.showLoader = true;
    this.settingsService.GetCostCenterTreeHierarchicalData(this.SearchText).subscribe(data => {
      this.showLoader = false;
      this.isSearchMode = true;
      this.CostCenterTreeData = data;

    }, (error) => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });
  }

  openDeleteModal(costCenterId: number) {
    this.selectedCostCenterId = costCenterId;
    this.modalService.open(this.deleteModal, { centered: true, size: 'md' });
  }

  deleteCostCenter() {
    this.showDeleteLoader = true;
    this.settingsService.DeleteCostCenterTree(this.selectedCostCenterId).subscribe(data => {

      if (data?.isSuccess) {
        this.modalService?.dismissAll();
        this.loadData();
      }
      else {
      }
      this.showDeleteLoader = false;
    }, err => {
      this.showDeleteLoader = false;
    }, () => {
      this.showDeleteLoader = false;
    });
  }

  changeSearchType(event) {

  }
}
