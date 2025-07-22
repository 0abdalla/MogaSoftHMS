import { Component, OnInit } from '@angular/core';
import { GeneralSelectorModel } from '../../../../Models/Generics/GeneralSelectorModel';
import { PagedResponseModel } from '../../../../Models/Generics/PagedResponseModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';
import { FilterModel, PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { CustomValidators } from '../../../../Services/custom-validators';
import { EmployeeVacationModel } from '../../../../Models/HMS/Staff/EmployeeVacationModel';

@Component({
  selector: 'app-hr-vacation',
  templateUrl: './hr-vacation.component.html',
  styleUrl: './hr-vacation.component.css'
})
export class HrVacationComponent implements OnInit {
  VacationData: any[] = [];
  employeeVacationsData: EmployeeVacationModel[] = [];
  employeeSelectorData: GeneralSelectorModel[] = [];
  vacationTypeSelectorData: GeneralSelectorModel[] = [];
  selectedVacationId: number;
  CategorySearch: any;
  VacationDays: number;
  CategoryName = 'قائمة الموظفين';
  PagingFilterModel: PagingFilterModel = {
    currentPage: 1,
    pageSize: 16,
    filterList: [],
    searchText: ''
  };
  employeeVacationModel: EmployeeVacationModel = {} as EmployeeVacationModel;
  employeeVacationResponse: PagedResponseModel<EmployeeVacationModel[]> = {
    results: [],
  };
  showLoader: boolean = false;
  showAddLoader: boolean = false;
  public formGroup: FormGroup;
  selectedEmployeeId: number = null;
  alternativeEmployeeId: number = null;
  showAlternativeSelector = false;
  isUpdate: boolean = false;
  constructor(private modalService: NgbModal,
    private staffService: StaffService,
    private form: FormBuilder,
    private datePipe: DatePipe,
    private offcanvasService: NgbOffcanvas) { }

  ngOnInit(): void {
    this.getActiveEmployeesSelector();
  }
  getVacationsByEmployeeId() {
    if (!this.checkEmployee())
      return;

    this.showLoader = true;
    this.staffService.GetVacationsByEmployeeId(this.selectedEmployeeId, this.PagingFilterModel).subscribe(data => {
      this.employeeVacationResponse.results = data.results;
      this.employeeVacationResponse.totalCount = data.totalCount;
      let obj = this.employeeSelectorData.find(x => x.value == this.selectedEmployeeId);
      if (obj) {
        let period = 0;
        this.employeeVacationResponse.results.forEach(item => {
          period += item.period;
        });
        this.VacationDays = obj.vacationDays - period;
      }

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

  showAlternativeEmployeeSelector() {
    if (this.showAlternativeSelector == true)
      this.showAlternativeSelector = false;
    else
      this.showAlternativeSelector = true;
  }

  openNewSidePanel(content: any, vacationModel: EmployeeVacationModel = null) {
    if (!this.checkEmployee())
      return;
    this.isUpdate = false;
    this.buildForm();
    if (vacationModel)
      this.fillEditForm(vacationModel);

    this.formGroup.patchValue({ employeeId: this.selectedEmployeeId });
    this.getVacationTypesSelector();
    this.offcanvasService.open(content, { panelClass: 'add-vacation-panel', position: 'end' });
  }

  buildForm() {
    this.formGroup = this.form.group({
      vacationId: [null],
      employeeId: [null],
      isAlternativeAvailable: [false],
      alternativeEmployeeId: [null],
      vacationTypeId: [null, [Validators.required]],
      fromDate: [null, [Validators.required]],
      toDate: [null, [Validators.required]],
      lastDayWork: [null, [Validators.required]],
      notes: [null],
    }, {
      validators: [CustomValidators.endDateGreaterThanStartDate('lastDayWork', 'fromDate', 'يجب ان يكون تاريخ بدء الاجازه بعد اخر يوم عمل'),
      CustomValidators.endDateGreaterThanStartDate('fromDate', 'toDate', 'يجب ان يكون تاريخ انهاء الاجازه بعد البدء')],
    });

    this.formGroup.get('isAlternativeAvailable').valueChanges.subscribe((alternativeEmployeeId) => {
    });
  }

  saveEmployeeVacation() {
    this.employeeVacationModel = this.formGroup.value;
    if (this.employeeVacationModel?.vacationId)
      this.editEmployeeVacation();
    else
      this.addNewEmployeeVacation();
  }

  addNewEmployeeVacation() {
    this.showAddLoader = true;
    this.staffService.AddNewEmployeeVacation(this.selectedEmployeeId, this.employeeVacationModel).subscribe(data => {
      if (data?.isSuccess) {
        this.formGroup?.reset();
        this.offcanvasService?.dismiss();
        this.getVacationsByEmployeeId();
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

  editEmployeeVacation() {
    this.showAddLoader = true;
    this.staffService.EditEmployeeVacation(this.selectedEmployeeId, this.employeeVacationModel).subscribe(data => {
      if (data?.isSuccess) {
        this.formGroup?.reset();
        this.offcanvasService?.dismiss();
        this.getVacationsByEmployeeId();
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
  getVacationTypesSelector() {
    this.staffService.GetVacationTypesSelector().subscribe((data: GeneralSelectorModel[]) => {
      this.vacationTypeSelectorData = data;
    });
  }


  fillEditForm(vacationModel: EmployeeVacationModel) {
    this.isUpdate = true;
    this.formGroup.patchValue({
      vacationId: vacationModel.vacationId,
      employeeId: this.selectedEmployeeId,
      vacationTypeId: vacationModel.vacationTypeId,
      fromDate: this.datePipe.transform(vacationModel.fromDate, 'yyyy-MM-dd'),
      toDate: this.datePipe.transform(vacationModel.toDate, 'yyyy-MM-dd'),
      lastDayWork: this.datePipe.transform(vacationModel.lastDayWork, 'yyyy-MM-dd'),
      notes: vacationModel.notes,
    });
  }


  openDeleteModal(content: any, vacationId: number) {
    this.selectedVacationId = vacationId;
    this.modalService.open(content, { centered: true, size: 'md' });
  }

  getActiveEmployeesSelector() {
    this.staffService.GetActiveEmployeesSelector().subscribe((data: GeneralSelectorModel[]) => {
      this.employeeSelectorData = data;
    });
  }

  filterChecked(filterItems: FilterModel[]) {
    this.PagingFilterModel.filterList = filterItems;
    this.getVacationsByEmployeeId();
  }

  pageChanged(obj: any) {
    this.PagingFilterModel.currentPage = obj.page;
    this.getVacationsByEmployeeId();
  }

  deleteVacation() {
    this.showAddLoader = true;
    this.staffService.DeleteVacation(this.selectedVacationId).subscribe(data => {
      if (data?.isSuccess) {
        this.modalService?.dismissAll();
        this.getVacationsByEmployeeId();
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
}
