import { Component, OnInit } from '@angular/core';
import { FormDropdownModel } from '../../../../Models/Generics/FormDropdownModel';
import { EmployeeContractModel } from '../../../../Models/HMS/Staff/EmployeeContractModel';
import { EmployeePenaltyModel } from '../../../../Models/HMS/Staff/EmployeePenaltyModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';
import { CustomValidators } from '../../../../Services/custom-validators';
import { StaffService } from '../../../../Services/HMS/staff.service';

@Component({
  selector: 'app-hr-penalty',
  templateUrl: './hr-penalty.component.html',
  styleUrl: './hr-penalty.component.css'
})
export class HrPenaltyComponent implements OnInit {
  TitleList = ['الموارد البشرية','الجزاءات'];
  VacationData: any[] = [];
  employeeSelectorData: FormDropdownModel[] = [];
  penaltyTypeSelectorData: FormDropdownModel[] = [];
  selectedPenaltyId: number;
  showLoader: boolean = false;
  showAddLoader: boolean = false;
  employeeContract: EmployeeContractModel;
  selectedEmployeeId: number = null;
  isUpdate: boolean = false;

  employeePenaltyModel: EmployeePenaltyModel = {} as EmployeePenaltyModel;
  employeePenaltyFilter: PagingFilterModel = {
    filterList: [],
    pageSize: 10,
    currentPage: 1,
    searchText: ''
  };
  employeePenaltyResponse: PagedResponseModel<EmployeePenaltyModel[]> = {
    results: []
  };

  public formGroup: FormGroup;
  public formErrors = {
    penaltyId: '',
    employeeId: '',
    penaltyTypeId: '',
    executionDate: '',
    deductionByDays: '',
    deductionAmount: '',
    totalDeduction: '',
    reason: ''
  };

  constructor(private modalService: NgbModal, private form: FormBuilder, private datePipe: DatePipe, private offcanvasService: NgbOffcanvas,
    private staffService: StaffService
  ) { }

  ngOnInit(): void {
    this.getActiveEmployeesSelector();
  }

  getPenaltiesByEmployeeId() {
    if (!this.checkEmployee())
      return;

    this.showLoader = true;
    this.staffService.GetPenaltiesByEmployeeId(this.selectedEmployeeId, this.employeePenaltyFilter).subscribe(data => {
      this.employeePenaltyResponse.results = data.results;
      this.employeePenaltyResponse.totalCount = data.totalCount;
      this.showLoader = false;
    }, err => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });

    this.getEmployeeContractSalary();
  }

  getEmployeeContractSalary() {
    this.staffService.GetEmployeeContractDetails(this.selectedEmployeeId).subscribe(data => {
      this.employeeContract = data;
    });
  }

  checkEmployee() {
    debugger;
    if (!this.selectedEmployeeId) {
      alert('من فضلك اختر من قائمة الموظفين');
      return false;
    }
    return true;
  }

  openNewSidePanel(content: any, penaltyModel: EmployeePenaltyModel = null) {
    if (!this.checkEmployee())
      return;
    this.isUpdate = false;
    this.buildForm();
    if (penaltyModel)
      this.fillEditForm(penaltyModel);

    this.formGroup.patchValue({ employeeId: this.selectedEmployeeId });
    this.getPenaltyTypesSelector();
    this.offcanvasService.open(content, { panelClass: 'add-new-panel', position: 'end' });
  }

  buildForm() {
    this.formGroup = this.form.group({
      penaltyId: [null],
      employeeId: [null],
      penaltyTypeId: [null, [Validators.required]],
      executionDate: [null, [Validators.required]],
      deductionByDays: [null, [Validators.required, Validators.pattern(/^[0-9]+(\.[0-9])?$/)]],
      deductionAmount: [null],
      totalDeduction: [null, [Validators.required, Validators.pattern(/^[0-9]+(\.[0-9])?$/)]],
      reason: [null, [Validators.required]],
    }, {
      validators: [CustomValidators.endDateGreaterThanStartDate('lastDayWork', 'fromDate'),
      CustomValidators.endDateGreaterThanStartDate('fromDate', 'toDate')],
    });

    this.formGroup.get('deductionByDays').valueChanges.subscribe(() => {
      this.calculateTotalDeduction();
    });

    this.formGroup.get('deductionAmount').valueChanges.subscribe(() => {
      this.calculateTotalDeduction();
    });
  }

  saveEmployeePenalty() {
    this.employeePenaltyModel = this.formGroup.value;

    if (this.employeePenaltyModel?.penaltyId)
      this.editEmployeePenalty();
    else
      this.addNewEmployeePenalty();
  }

  addNewEmployeePenalty() {
    this.showAddLoader = true;
    this.staffService.AddNewEmployeePenalty(this.selectedEmployeeId, this.employeePenaltyModel).subscribe(data => {
      if (data?.isSuccess) {
        this.formGroup?.reset();
        this.offcanvasService?.dismiss();
        this.getPenaltiesByEmployeeId();
      }
      else {
      }
      this.showAddLoader = false;
    }, err => {
      this.showAddLoader = false;
    }, () => {
      this.showAddLoader = false;
    });
  }

  editEmployeePenalty() {
    this.showAddLoader = true;
    this.staffService.EditEmployeePenalty(this.selectedEmployeeId, this.employeePenaltyModel).subscribe(data => {
      if (data?.isSuccess) {
        this.formGroup?.reset();
        this.offcanvasService?.dismiss();
        this.getPenaltiesByEmployeeId();
      }
      else {
      }
      this.showAddLoader = false;
    }, err => {
      this.showAddLoader = false;
    }, () => {
      this.showAddLoader = false;
    });
  }

  getPenaltyTypesSelector() {
    this.staffService.GetPenaltyTypesSelector().subscribe((data: FormDropdownModel[]) => {
      this.penaltyTypeSelectorData = data;
    });
  }

  fillEditForm(penaltyModel: EmployeePenaltyModel) {
    this.isUpdate = true;
    this.formGroup.patchValue({
      penaltyId: penaltyModel.penaltyId,
      employeeId: this.selectedEmployeeId,
      penaltyTypeId: penaltyModel.penaltyTypeId,
      executionDate: this.datePipe.transform(penaltyModel.executionDate, 'yyyy-MM-dd'),
      deductionByDays: penaltyModel.deductionByDays,
      moneyAmount: penaltyModel.moneyAmount,
      deductionAmount: penaltyModel.deductionAmount,
      reason: penaltyModel.reason
    });
  }

  openDeleteModal(content: any, penaltyDateId: number) {
    this.selectedPenaltyId = penaltyDateId;
    this.modalService.open(content, { centered: true, size: 'md' });
  }

  getActiveEmployeesSelector() {
    this.staffService.GetActiveEmployeesSelector().subscribe((data: FormDropdownModel[]) => {
      this.employeeSelectorData = data;
    });
  }

  filterChecked(filterItems: FilterModel[]) {
    this.employeePenaltyFilter.filterList = filterItems;
    this.getPenaltiesByEmployeeId();
  }

  pageChanged(obj: any) {
    this.employeePenaltyFilter.currentPage = obj.page;
    this.getPenaltiesByEmployeeId();
  }

  deleteEmployeePenalty() {
    this.showAddLoader = true;
    this.staffService.DeleteEmployeePenalty(this.selectedPenaltyId).subscribe(data => {
      if (data?.isSuccess) {
        this.modalService?.dismissAll();
        this.getPenaltiesByEmployeeId();
      }
      else {
      }
      this.showAddLoader = false;
    }, err => {
      this.showAddLoader = false;
    }, () => {
      this.showAddLoader = false;
    });
  }

  calculateTotalDeduction() {
    let deductionByDays = this.formGroup.get('deductionByDays').value;
    let deductionAmount = this.formGroup.get('deductionAmount').value;
    let salaryPerHour = 0;
    if (deductionByDays == null)
      deductionByDays = 0;
    if (deductionAmount == null)
      deductionAmount = 0;
    if (this.employeeContract && this.employeeContract?.basicSalary)
      salaryPerHour = this.employeeContract?.basicSalary / 30;
    this.formGroup.get('totalDeduction').setValue(Math.round(Number(deductionByDays) * Number(salaryPerHour) + Number(deductionAmount)));
  }
}
