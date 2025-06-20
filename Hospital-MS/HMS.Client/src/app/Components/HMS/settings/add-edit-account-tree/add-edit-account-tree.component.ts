import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountTreeModel } from '../../../../Models/HMS/AccountTree';
import { GeneralSelectorModel } from '../../../../Models/Generics/GeneralSelectorModel';
import { SettingService } from '../../../../Services/HMS/setting.service';

@Component({
  selector: 'app-add-edit-account-tree',
  templateUrl: './add-edit-account-tree.component.html',
  styleUrl: './add-edit-account-tree.component.css'
})
export class AddEditAccountTreeComponent {
  @Input() isUpdate: boolean = false;
  @Input() groupAccountId: number;
  @Input() accountModel: AccountTreeModel = {} as AccountTreeModel;
  @Output() dataUpdated = new EventEmitter<boolean>();
  showLoader: boolean = false;
  parentAccountsList: any[] = [];
  accountTypes: any[] = [];
  currencyType: any[] = [{ currencyId: 1, nameAR: 'جنيه' }, { currencyId: 1, nameAR: 'ريال' }]
  accountTypesSelectorData: GeneralSelectorModel[] = [];
  parentAccountsSelectorData: GeneralSelectorModel[] = [];
  currencyTypesSelectorData: GeneralSelectorModel[] = [];
  costCenterSelector: GeneralSelectorModel[] = [];
  selectedAccountId: number = null;
  public formGroup: FormGroup;

  constructor(private modalService: NgbModal, private form: FormBuilder, private settingService: SettingService) { }

  ngOnInit(): void { }

  openNewSidePanel(content: any) {
    this.loadSelectors();
    this.initNewForm();
    this.formGroup.patchValue({ parentAccountId: this.groupAccountId });
    this.modalService.open(content, { centered: true, size: 'lg', fullscreen: 'lg' });
  }


  initNewForm() {
    this.buildForm();
    if (this.isUpdate)
      this.fillEditForm(this.accountModel);
    else {
      this.accountModel = {} as AccountTreeModel;
      if (this.groupAccountId)
        this.generateAccountNumber(this.groupAccountId);
      else
        this.generateAccountNumber();
    }

  }
  buildForm() {
    this.formGroup = this.form.group({
      accountId: [null],
      accountNumber: [null],
      parentAccountId: [null],
      accountTypeId: [null, [Validators.required]],
      currencyTypeId: [null],
      nameAR: [null, [Validators.required]],
      isDisToCostCenter: [false, [Validators.required]],
      costCenterId: [null],
      isActive: [true, [Validators.required]],
      isGroup: [false, [Validators.required]],
      notes: [null],
    });

    this.formGroup.get('parentAccountId').valueChanges.subscribe((parentAccountId: number) => {
      if (!this.isUpdate) {
        this.generateAccountNumber(parentAccountId ? parentAccountId : 0);
      }
    });
  }

  saveAccount() {
    this.accountModel = this.formGroup.value;
    if (this.accountModel?.accountId)
      this.editAccount();
    else
      this.addNewAccount();
  }

  addNewAccount() {
    this.showLoader = true;
    this.settingService.AddNewAccount(this.accountModel).subscribe(data => {
      if (data?.isSuccess) {
        this.initNewForm();
        this.modalService?.dismissAll();
        this.dataUpdated.emit(true);
      }
      else {
      }
      this.showLoader = false;
    }, err => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });
  }

  editAccount() {
    this.showLoader = true;
    this.settingService.EditAccountTree(this.accountModel.accountId, this.accountModel).subscribe(data => {
      if (data?.isSuccess) {
        this.isUpdate = false;
        this.modalService?.dismissAll();
        this.initNewForm();
        this.dataUpdated.emit(true);
      }
      else {
      }
      this.showLoader = false;
    }, err => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });


  }

  generateAccountNumber(parentAccountId: number = 0) {
    this.settingService.GenerateAccountNumber(parentAccountId).subscribe(data => {
      if (data) {
        this.formGroup?.patchValue({ accountNumber: data });
      }
    }, err => {

    }, () => {

    });
  }


  fillEditForm(accountModel: AccountTreeModel) {
    this.isUpdate = true;
    this.formGroup.patchValue({
      accountId: accountModel.accountId,
      accountNumber: accountModel.accountNumber,
      accountTypeId: accountModel.accountTypeId,
      parentAccountId: accountModel.parentAccountId,
      currencyTypeId: accountModel.currencyTypeId,
      nameAR: accountModel.nameAR,
      nameEN: accountModel.nameAR,
      isDisToCostCenter: accountModel.isDisToCostCenter,
      costCenterId: accountModel.costCenterId,
      isActive: accountModel.isActive,
      isGroup: accountModel.isGroup,

    });
  }

  loadSelectors() {
    this.settingService.GetAccountTypes().subscribe(data => {
      this.accountTypesSelectorData = data;
    });
    this.settingService.GetCurrencySelector().subscribe(data => {
      this.currencyTypesSelectorData = data;
    });
    this.settingService.GetAccountsSelector(true).subscribe(data => {
      this.parentAccountsSelectorData = data;
    });

    this.settingService.GetCostCenterSelector(false).subscribe(data => {
      this.costCenterSelector = data;
    });

  }

  getSelectedParentAccount(account: AccountTreeModel) {
    this.accountModel.parentAccountId = account.accountId;
    this.accountModel.accountLevel = account.accountLevel + 1;
  }
}
