import { Component, OnInit } from '@angular/core';
import { FormDropdownModel } from '../../../../Models/Generics/FormDropdownModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { EmployeeAdvanceModel, HRWorkflowStatus } from '../../../../Models/HMS/Staff/EmployeeAdvanceModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';
import { CustomValidators, RegexType } from '../../../../Services/custom-validators';

@Component({
  selector: 'app-hr-advances',
  templateUrl: './hr-advances.component.html',
  styleUrl: './hr-advances.component.css'
})
export class HrAdvancesComponent implements OnInit {
  VacationData: any[] = [];
  employeeSelectorData: FormDropdownModel[] = [];
  penaltyTypeSelectorData: FormDropdownModel[] = [];
  advanceTypesSelectorData: FormDropdownModel[] = [];
  selectedAdvanceId: number;
  employeeAdvanceModel: EmployeeAdvanceModel = {} as EmployeeAdvanceModel;
  showLoader: boolean = false;
  showAddLoader: boolean = false;
  formGroup: FormGroup;
  selectedEmployeeId: number = null;
  isUpdate: boolean = false;
  workflowStatus = HRWorkflowStatus;
  employeeAdvanceResponse: PagedResponseModel<EmployeeAdvanceModel[]> = { results: [] };
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentPage: 1,
    pageSize: 16,
    searchText: ''
  };

  constructor(private modalService: NgbModal, private staffService: StaffService, private form: FormBuilder,
    private datePipe: DatePipe, private offcanvasService: NgbOffcanvas) { }

  ngOnInit(): void {
    this.getActiveEmployeesSelector();
  }
  getAdvanceByEmployeeId() {
    if (!this.checkEmployee())
      return;

    this.showLoader = true;
    this.staffService.GetAdvancesByEmployeeId(this.selectedEmployeeId, this.PagingFilter).subscribe(data => {
      this.employeeAdvanceResponse.results = data.results;
      this.employeeAdvanceResponse.totalCount = data.totalCount;

      this.showLoader = false;
    }, err => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });
  }

  checkEmployee() {
    if (!this.selectedEmployeeId) {
      alert('من فضلك اختر من قائمة الموظفين');
      return false;
    }
    return true;
  }

  openNewSidePanel(content: any, advanceModel: EmployeeAdvanceModel = null) {
    if (!this.checkEmployee())
      return;

    this.getAdvanceTypesSelector();

    this.isUpdate = false;
    this.buildForm();
    if (advanceModel)
      this.fillEditForm(advanceModel);

    this.formGroup.patchValue({ employeeId: this.selectedEmployeeId });

    this.offcanvasService.open(content, { panelClass: 'add-new-panel', position: 'end' });
  }
  buildForm() {

    this.formGroup = this.form.group({
      staffAdvanceId: [null],
      employeeId: [null],
      advanceTypeId: [null, [Validators.required]],
      advanceAmount: [null, [Validators.required, CustomValidators.regexPattern(RegexType.number)]],
      paymentAmount: [null, [Validators.required, CustomValidators.regexPattern(RegexType.number)]],
      isApproved: [null],
      paymentFromDate: [null, [Validators.required, CustomValidators.dateGreaterThan(new Date(), 'ادخل تاربخ اكبر')]],
      notes: [null],

    });
  }

  saveEmployeeAdvance() {
    debugger;
    this.employeeAdvanceModel = this.formGroup.value;
    if (this.employeeAdvanceModel?.staffAdvanceId)
      this.editEmployeeAdvance();
    else
      this.addNewEmployeeAdvance();
  }

  addNewEmployeeAdvance() {
    this.showAddLoader = true;
    this.staffService.AddNewEmployeeAdvance(this.selectedEmployeeId, this.employeeAdvanceModel).subscribe(data => {
      if (data?.isSuccess) {
        this.formGroup?.reset();
        this.offcanvasService?.dismiss();
        this.getAdvanceByEmployeeId();
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

  editEmployeeAdvance() {
    this.showAddLoader = true;
    this.staffService.EditEmployeeAdvance(this.selectedEmployeeId, this.employeeAdvanceModel).subscribe(data => {
      if (data?.isSuccess) {
        this.formGroup?.reset();
        this.offcanvasService?.dismiss();
        this.getAdvanceByEmployeeId();
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


  fillEditForm(advanceModel: EmployeeAdvanceModel) {
    debugger;
    this.isUpdate = true;
    this.formGroup.patchValue({
      staffAdvanceId: advanceModel.staffAdvanceId,
      advanceTypeId: advanceModel.advanceTypeId,
      advanceAmount: advanceModel.advanceAmount,
      paymentAmount: advanceModel.paymentAmount,
      isApproved: advanceModel.isApproved,
      employeeId: this.selectedEmployeeId,
      paymentFromDate: this.datePipe.transform(advanceModel.paymentFromDate, 'yyyy-MM-dd'),
      notes: advanceModel.notes
    });
  }


  openDeleteModal(content: any, staffAdvanceId: number) {
    this.selectedAdvanceId = staffAdvanceId;
    this.modalService.open(content, { centered: true, size: 'md' });
  }

  getActiveEmployeesSelector() {
    this.staffService.GetActiveEmployeesSelector().subscribe((data: FormDropdownModel[]) => {
      this.employeeSelectorData = data;
    });
  }

  filterChecked(filterItems: FilterModel[]) {
    this.PagingFilter.filterList = filterItems;
    this.getAdvanceByEmployeeId();
  }

  pageChanged(obj: any) {
    this.PagingFilter.currentPage = obj.page;
    this.getAdvanceByEmployeeId();
  }


  deleteEmployeeAdvance() {
    this.showAddLoader = true;
    this.staffService.DeleteEmployeeAdvance(this.selectedAdvanceId).subscribe(data => {

      if (data?.isSuccess) {
        this.modalService?.dismissAll();
        this.getAdvanceByEmployeeId();
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

  openApproveModal(content: any, staffAdvanceId: number) {
    this.selectedAdvanceId = staffAdvanceId;
    this.modalService.open(content, { centered: true, size: 'md' });
  }
  approveEmployeeAdvance() {
    this.showAddLoader = true;
    this.staffService.ApproveEmployeeAdvance(this.selectedAdvanceId).subscribe(data => {
      if (data?.isSuccess) {
        this.modalService?.dismissAll();
        this.getAdvanceByEmployeeId();
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
  
  getAdvanceTypesSelector() {
    this.staffService.GetAdvanceTypesSelector().subscribe((data: FormDropdownModel[]) => {
      this.advanceTypesSelectorData = data;
    });
  }
}
