import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { CostCenterTreeModel } from '../../../../../Models/Generics/CostCenter';
import { FormDropdownModel } from '../../../../../Models/Generics/FormDropdownModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SettingService } from '../../../../../Services/HMS/setting.service';

@Component({
  selector: 'app-add-edit-cost-center-tree',
  templateUrl: './add-edit-cost-center-tree.component.html',
  styleUrl: './add-edit-cost-center-tree.component.css'
})
export class AddEditCostCenterTreeComponent implements OnInit, OnChanges {
  @Input() isUpdate: boolean = false;
  @Input() costCenterModel: CostCenterTreeModel = {} as CostCenterTreeModel;
  @Output() dataUpdated = new EventEmitter<boolean>();

  showLoader: boolean = false;
  parentCostCenterSelectorData: FormDropdownModel[] = [];
  selectedCostCenterId: number = null;
  public formGroup: FormGroup;

  public formErrors = {
    costCenterId: '',
    costCenterNumber: '',
    nameAR: '',
    nameEN: '',
    parentId: '',
    isPost: '',
    isActive: '',
    notes: '',
  };
  constructor(private settingService: SettingService, private form: FormBuilder) { }



  ngOnInit(): void {
    this.initNewForm();
    this.loadSelectors();

  }

  ngOnChanges(changes): void {
    if (changes && !changes.costCenterModel.firstChange) {
      if (this.costCenterModel && this.costCenterModel != null) {
        this.initNewForm(this.costCenterModel);
      }
    }
  }


  initNewForm(costCenterModel: CostCenterTreeModel = null) {
    this.isUpdate = false;
    this.buildForm();
    if (costCenterModel)
      this.fillEditForm(costCenterModel);
    else {
      this.costCenterModel = {} as CostCenterTreeModel;
      this.generateCostCenterNumber();
    }

  }
  buildForm() {
    this.formGroup = this.form.group({
      costCenterId: [null],
      costCenterNumber: [null],
      nameAR: [null, [Validators.required]],
      nameEN: [null, [Validators.required]],
      parentId: [null],
      isPost: [false, [Validators.required]],
      isActive: [false, [Validators.required]],
      isGroup: [false, [Validators.required]],
      notes: [null],

    });

    this.formGroup.get('parentId').valueChanges.subscribe((parentId: number) => {
      if (!this.isUpdate) {
        this.generateCostCenterNumber(parentId ? parentId : 0);
      }
    });
  }

  saveCostCenter() {
    this.costCenterModel = this.formGroup.value;
    if (this.costCenterModel?.costCenterId)
      this.editCostCenter();
    else
      this.addNewCostCenter();
  }

  addNewCostCenter() {
    this.showLoader = true;
    this.settingService.CreateNewCostCenter(this.costCenterModel).subscribe(data => {
      if (data?.isSuccess) {
        this.formGroup?.reset();
        this.loadSelectors();
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

  editCostCenter() {
    this.showLoader = true;
    this.settingService.UpdateCostCenterTree(this.costCenterModel.costCenterId, this.costCenterModel).subscribe(data => {
      if (data?.isSuccess) {
        this.initNewForm();
        this.loadSelectors();
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

  generateCostCenterNumber(parentCostCenterId: number = 0) {
    this.settingService.GenerateCostCenterNumber(parentCostCenterId).subscribe(data => {
      if (data) {
        this.formGroup?.patchValue({ costCenterNumber: data });
      }

    }, err => {

    }, () => {

    });
  }

  fillEditForm(costCenterModel: CostCenterTreeModel) {
    this.isUpdate = true;
    this.formGroup.patchValue({
      costCenterId: costCenterModel.costCenterId,
      costCenterNumber: costCenterModel.costCenterNumber,
      nameAR: costCenterModel.nameAR,
      nameEN: costCenterModel.nameEN,
      parentId: costCenterModel.parentId,
      isPost: costCenterModel.isPost,
      isActive: costCenterModel.isActive,
      isGroup: costCenterModel.isGroup
    });
  }
  loadSelectors() {
    this.settingService.GetCostCenterSelector(true).subscribe(data => {
      this.parentCostCenterSelectorData = data;
    });
  }

  getSelectedParentCostCenter(costCenter: CostCenterTreeModel) {
    this.costCenterModel.parentId = costCenter.costCenterId;
    this.costCenterModel.costLevel = costCenter.costLevel + 1;
  }
}
