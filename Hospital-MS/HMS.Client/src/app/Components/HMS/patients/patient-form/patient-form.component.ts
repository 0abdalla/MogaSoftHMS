import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { MessageService } from 'primeng/api';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';

@Component({
  selector: 'app-patient-form',
  templateUrl: './patient-form.component.html',
  styleUrls: ['./patient-form.component.css']
})
export class PatientFormComponent implements OnInit {
  pagingFilterModel: PagingFilterModel = {
      searchText: '',
      currentPage: 1,
      pageSize: 100,
      filterList: []
    };
  patientForm: FormGroup;
  // 
  doctors!:any[];
  filteredDoctors!: any[];
  departments!:any;
  rooms!:any;
  filteredRooms!:any[];
  beds!:any;
  filteratedBeds!:any[];
  insuranceCompanies!:any;
  insuranceCategories!:any;
  // 
  showSecondContact = false;
  // 
  currentDate!:any
  // 
  selectedDailyPrice: number | null = null;
  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
    private addmisionService: AdmissionService,
    private messageService : MessageService,
    private insuranceService: InsuranceService
  ) {
    const minBirthDate = new Date(1920, 0, 1);
    this.patientForm = this.fb.group({
      patientName: ['', Validators.required],
      patientPhone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      patientBirthDate: ['', [Validators.required, this.minDateValidator(minBirthDate)]],
      patientNationalId: ['', [Validators.required, Validators.pattern(/^[0-9]{14}$/)]],
      patientAddress: ['', Validators.required],
      patientStatus: ['', Validators.required],
      patientGender: ['', Validators.required],
      emergencyPhone01: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      emergencyContact01: ['', Validators.required],
      emergencyPhone02: [''], 
      emergencyContact02: [''], 
      departmentId: ['', Validators.required],
      doctorId: [{ value: '', disabled: true }, Validators.required],
      roomType: ['', Validators.required],
      roomId: [{ value: '', disabled: true }, Validators.required],
      bedId: [{ value: '', disabled: true }, Validators.required],
      insuranceCompanyId: [''],
      insuranceCategoryId: [null],
      insuranceNumber: [null],
      healthStatus: ['', Validators.required],
      initialDiagnosis: ['', Validators.required],
      hasCompanion: [false, Validators.required], 
      companionName: ['', []], 
      companionNationalId: ['', [Validators.pattern(/^[0-9]{14}$/)]], 
      companionPhone: ['', [Validators.pattern(/^01[0125][0-9]{8}$/)]], 
      notes: [''],
    });

    this.patientForm.get('hasCompanion')?.valueChanges.subscribe((value) => {
      this.updateCompanionValidators(value);
    });
    this.patientForm.get('emergencyPhone02')?.valueChanges.subscribe(() => {
      this.updateSecondContactValidators();
    });
    this.patientForm.get('emergencyContact02')?.valueChanges.subscribe(() => {
      this.updateSecondContactValidators();
    });
    this.patientForm.get('insuranceCompanyId')?.valueChanges.subscribe(companyId => {
      const selectedCompany = this.insuranceCompanies.find(company => company.id === +companyId);
      this.insuranceCategories = selectedCompany?.insuranceCategories || [];
    });
  }
  addSecondContact() {
    this.showSecondContact = true;
  }
  ngOnInit(): void {
    this.loadAdmissionData();
    this.currentDate = new Date().toISOString().split('T')[0];
  }
  loadAdmissionData() {
    forkJoin({
      doctors: this.staffService.getDoctors(this.pagingFilterModel),
      departments: this.addmisionService.getDepartments(),
      rooms: this.addmisionService.getRooms(),
      beds: this.addmisionService.getBeds(),
      insuranceCompanies: this.insuranceService.getAllInsurances()
    }).subscribe({
      next: (res: any) => {
        this.doctors = res.doctors.results;
        this.departments = res.departments.results;
        this.rooms = res.rooms.results;
        this.beds = res.beds.results;
        this.insuranceCompanies = res.insuranceCompanies.results;
        this.insuranceCategories = this.insuranceCompanies
          .filter(company => company.insuranceCategories && Array.isArray(company.insuranceCategories))
          .flatMap(company => company.insuranceCategories);
        console.log('Doctors:', this.doctors);
        console.log('Departments:', this.departments);
        console.log('Rooms:', this.rooms);
        console.log('Beds:', this.beds);
        console.log('Insurance Companies:', this.insuranceCompanies);
        console.log('Insurance Categories:', this.insuranceCategories);
      },
      error: (err) => {
        console.error('Error loading admission data', err);
      }
    });
  }
  onSubmit() {
    if (this.patientForm.valid) {
      this.addmisionService.addAdmision(this.patientForm.value).subscribe({
        next: () => {
          console.log("Done");
          this.patientForm.reset();
          this.showSecondContact = false;
          this.messageService.add({ severity: 'success', summary: 'تم الحجز', detail: 'تم إنشاء الحجز بنجاح' });
        },
        error: (error) => {
          console.error(error);
          this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: 'حدث خطأ أثناء إنشاء الحجز' });

        }
      });
    }else{
      console.log("error");
    }
  }
  onDepartmentChange() {
    const selectedDeptId = this.patientForm.get('departmentId')?.value;
    if (selectedDeptId) {
      this.filteredDoctors = this.doctors.filter(doctor => doctor.departmentId === +selectedDeptId);
      this.patientForm.get('doctorId')?.enable();
    } else {
      console.log("error");
      this.filteredDoctors = [];
      this.patientForm.get('doctorId')?.disable();
    }
    this.patientForm.get('doctorId')?.setValue('');
  }
  onRoomTypeChange(){
    const selectedRoomType = this.patientForm.get('roomType')?.value;
    if (selectedRoomType) {
      this.filteredRooms = this.rooms.filter((room:any) => room.type === selectedRoomType);
      this.patientForm.get('roomId')?.enable();
    } else {
      console.log("error");
      this.filteredRooms = [];
      this.patientForm.get('roomId')?.disable();
    }
    this.patientForm.get('roomId')?.setValue('');
  }
  onRoomChange(){
    const selectedRoomId = this.patientForm.get('roomId')?.value;
    const selectedRoom = this.rooms.find(room => room.id === +selectedRoomId);
    this.selectedDailyPrice = selectedRoom ? selectedRoom.dailyPrice : null;
    if (selectedRoomId) {
      this.filteratedBeds = this.beds.filter((bed:any) => bed.roomId === +selectedRoomId);
      this.patientForm.get('bedId')?.enable();
    } else {
      console.log("error");
      this.filteratedBeds = [];
      this.patientForm.get('bedId')?.disable();
    }
    this.patientForm.get('bedId')?.setValue('');
  }

  // 
  updateCompanionValidators(hasCompanion: boolean): void {
    const companionName = this.patientForm.get('companionName');
    const companionNationalId = this.patientForm.get('companionNationalId');
    const companionPhone = this.patientForm.get('companionPhone');

    if (hasCompanion) {
      companionName?.setValidators([Validators.required]);
      companionNationalId?.setValidators([Validators.required, Validators.pattern(/^[0-9]{14}$/)]);
      companionPhone?.setValidators([Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]);
    } else {
      companionName?.setValidators([]);
      companionNationalId?.setValidators([Validators.pattern(/^[0-9]{14}$/)]);
      companionPhone?.setValidators([Validators.pattern(/^01[0125][0-9]{8}$/)]);
    }

    companionName?.updateValueAndValidity();
    companionNationalId?.updateValueAndValidity();
    companionPhone?.updateValueAndValidity();
  }
  updateSecondContactValidators(): void {
    const emergencyPhone02 = this.patientForm.get('emergencyPhone02');
    const emergencyContact02 = this.patientForm.get('emergencyContact02');

    if (this.showSecondContact) {
      emergencyPhone02?.setValidators([Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]);
      emergencyContact02?.setValidators([Validators.required]);
    } else {
      emergencyPhone02?.setValidators([Validators.pattern(/^01[0125][0-9]{8}$/)]);
      emergencyContact02?.setValidators([]);
    }

    emergencyPhone02?.updateValueAndValidity();
    emergencyContact02?.updateValueAndValidity();
  }
  // 
  minDateValidator(minDate: Date): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (!control.value) {
        return null;
      }
      const inputDate = new Date(control.value);
      if (inputDate < minDate) {
        return { minDate: { value: control.value } };
      }
      return null;
    };
  }
}
