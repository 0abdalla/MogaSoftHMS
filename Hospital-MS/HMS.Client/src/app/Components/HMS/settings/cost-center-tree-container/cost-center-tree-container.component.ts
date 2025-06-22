import { Component, OnInit } from '@angular/core';
import { CostCenterTreeModel } from '../../../../Models/Generics/CostCenter';
import { SettingService } from '../../../../Services/HMS/setting.service';

@Component({
  selector: 'app-cost-center-tree-container',
  templateUrl: './cost-center-tree-container.component.html',
  styleUrl: './cost-center-tree-container.component.css'
})
export class CostCenterTreeContainerComponent implements OnInit {
  selectedCostCenterTreeModel: CostCenterTreeModel = {} as CostCenterTreeModel;
  isUpdate: boolean = false;
  reloadData: boolean = false;
  showExportLoader: boolean = false;
  TitleList = ['إعدادات النظام', 'إعدادات الإدارة المالية', 'مراكز التكلفة'];

  constructor(private settingService: SettingService) { }


  ngOnInit(): void {
  }

  dataUpdated(event) {
    this.isUpdate = false;
    this.selectedCostCenterTreeModel = null;
    this.reloadData = !this.reloadData;
  }

  selectedCostCenter(costCenter: CostCenterTreeModel) {
    this.selectedCostCenterTreeModel = costCenter;
    this.isUpdate = true;
  }

  exportData() {
    // this.showExportLoader = true;
    // this._GeneralAccountService.ExportCostCenterTreeList("").subscribe((data:any) => {
    //   if (data.isSuccess) {
    //     this._SharedService.urlDownloadOrOpen(data.url);
    //     this.toaster.success(data.message);
    //   } else {
    //     this.toaster.error(data.message);
    //   }
    //   this.showExportLoader = false;
    // }, err => {
    //   this.showExportLoader = false;
    // }, () => {
    //   this.showExportLoader = false;
    // });
  }
}
