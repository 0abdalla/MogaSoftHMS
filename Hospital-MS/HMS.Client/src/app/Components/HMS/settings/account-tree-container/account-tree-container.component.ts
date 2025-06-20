import { Component } from '@angular/core';
import { AccountTreeModel } from '../../../../Models/HMS/AccountTree';

@Component({
  selector: 'app-account-tree-container',
  templateUrl: './account-tree-container.component.html',
  styleUrl: './account-tree-container.component.css'
})
export class AccountTreeContainerComponent {
selectedAccountTreeModel: AccountTreeModel = {} as AccountTreeModel;
  isUpdate: boolean = false;
  reloadData: boolean = false;
  showExportLoader: boolean = false;
  TitleList = ['إعدادات النظام', 'إعدادات الإدارة المالية','شجرة الحسابات'];

  constructor() { }

  ngOnInit(): void {
  }

  dataUpdated(event) {

    this.isUpdate = false;
    this.selectedAccountTreeModel = null;
    this.reloadData = !this.reloadData;
  }

  selectedAccount(account: AccountTreeModel) {
    this.selectedAccountTreeModel = account;
    this.isUpdate = true;
  }


  exportData() {
    // this.showExportLoader = true;
    // this._GeneralAccountService.ExportAccountTreeList("").subscribe((data: ActionsResponseModel) => {
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
