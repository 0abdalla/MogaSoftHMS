import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SettingService } from '../../../../../Services/HMS/setting.service';
import { AccountTreeModel } from '../../../../../Models/HMS/AccountTree';

@Component({
  selector: 'app-account-tree',
  templateUrl: './account-tree.component.html',
  styleUrl: './account-tree.component.css'
})
export class AccountTreeComponent {
  @ViewChild('deleteModal') deleteModal: HTMLElement;
  @Input() isParentAccount: boolean = false;
  @Input() reloadData: boolean = false;
  @Output() selectedAccount = new EventEmitter<any>();
  selectedAccountId: number;
  accountTreeData: any[] = [];
  AccountData: any[] = [];
  showLoader: boolean = false;
  showDeleteLoader: boolean = false;
  searchText = '';
  isSearchMode = false;
  parentAccountsList: any[] = [];
  accountTypes: any[] = [];
  currencyType: any[] = [{ currencyId: 1, nameAR: 'جنيه' }, { currencyId: 1, nameAR: 'ريال' }]
  accountTreeModel: AccountTreeModel = {} as AccountTreeModel;

  constructor(private modalService: NgbModal, private settingService: SettingService) { }

  ngOnInit(): void {
    this.loadData();
  }

  ngOnChanges(changes): void {
    if (changes && changes.reloadData && !changes.reloadData.firstChange) {
      this.loadData();
    }
  }

  selectAccount(account: AccountTreeModel) {
    if (account.isDeleteAction) {
      this.openDeleteModal(account.accountId);
      return
    }
    this.selectedAccount.emit(account);
  }

  loadData() {
    debugger;
    this.showLoader = true;
    this.settingService.GetAccountTreeHierarchicalData(this.searchText).subscribe(data => {
      this.showLoader = false;
      this.isSearchMode = true;
      this.accountTreeData = data;
    }, (error) => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });
  }

  openDeleteModal(accountId: number) {
    this.selectedAccountId = accountId;
    this.modalService.open(this.deleteModal, { centered: true, size: 'md' });
  }

  deleteAccount() {
    this.showDeleteLoader = true;
    this.settingService.DeleteAccountTree(this.selectedAccountId).subscribe(data => {

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

  dataUpdated(isUpdate: boolean) {
    if (isUpdate) {
      this.loadData();
    }
  }
}
