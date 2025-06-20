import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { MessageService } from 'primeng/api';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { AdmissionService } from '../../../../Services/HMS/admission.service';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';
import { GeneralSelectorModel } from '../../../../Models/Generics/GeneralSelectorModel';

@Component({
  selector: 'app-patient-form',
  templateUrl: './patient-form.component.html',
  styleUrls: ['./patient-form.component.css']
})
export class PatientFormComponent implements OnInit {
  TitleList = ['إدارة المستشفى', 'المرضى', 'إضافة مريض'];
  Genders = [{ name: 'ذكر', value: 'Male' }, { name: 'أنثى', value: 'Female' }];
  PatientStatus = [{ name: 'إقامة', value: 'Staying' }, { name: 'عناية مركزة', value: 'IntensiveCare' }, { name: 'طوارئ', value: 'Emergency' },
  { name: 'حضانات الأطفال', value: 'NeonatalCare' }, { name: 'عمليات', value: 'Surgery' }];
  PatientHealthStatus = [{ name: 'مستقرة', value: 'مستقرة' }, { name: 'غير مستقرة', value: 'غير مستقرة' }, { name: 'حرجة', value: 'حرجة' }];
  pagingFilterModel: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 100,
    filterList: []
  };
  patientForm: FormGroup;
  // 
  doctors!: any[];
  filteredDoctors!: any[];
  departments!: any;
  rooms!: any;
  filteredRooms!: any[];
  beds!: any;
  filteratedBeds!: any[];
  insuranceCompaniesData!: any[];
  insuranceCompanies!: any;
  insuranceCategories!: any;
  patients!: any;
  // 
  showSecondContact = false;
  // 
  currentDate!: any
  // 
  selectedDailyPrice: number | null = null;
  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
    private addmisionService: AdmissionService,
    private messageService: MessageService,
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
      const selectedCompany = this.insuranceCompaniesData.find(company => company.id === +companyId);
      this.insuranceCategories = selectedCompany?.insuranceCategories.map(i => { return { value: i.id, name: i.name } }) as GeneralSelectorModel[] || [];
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
      insuranceCompanies: this.insuranceService.getAllInsurances(),
      patients: this.addmisionService.getAddmision(this.pagingFilterModel)
    }).subscribe({
      next: (res: any) => {
        this.doctors = res.doctors.results;
        this.departments = res.departments.results;
        this.rooms = res.rooms.results;
        this.beds = res.beds.results;
        this.insuranceCompaniesData = res.insuranceCompanies.results;
        this.insuranceCompanies = res.insuranceCompanies.results.map(i => { return { value: i.id, name: i.name } }) as GeneralSelectorModel[];
        this.insuranceCategories = res.insuranceCompanies.results
          .filter(company => company.insuranceCategories && Array.isArray(company.insuranceCategories))
          .flatMap(company => company.insuranceCategories).map(i => { return { value: i.id, name: i.name } }) as GeneralSelectorModel[];
        this.patients = res.patients.results;
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
          this.patientForm.reset();
          this.showSecondContact = false;
          this.messageService.add({ severity: 'success', summary: 'تم الحجز', detail: 'تم إنشاء الحجز بنجاح' });
        },
        error: (error) => {
          console.error(error);
          this.messageService.add({ severity: 'error', summary: 'فشل الحجز', detail: 'حدث خطأ أثناء إنشاء الحجز' });

        }
      });
    } else {
    }
  }
  onDepartmentChange() {
    const selectedDeptId = this.patientForm.get('departmentId')?.value;
    if (selectedDeptId) {
      this.filteredDoctors = this.doctors.filter(doctor => doctor.departmentId === +selectedDeptId);
      this.patientForm.get('doctorId')?.enable();
    } else {
      this.filteredDoctors = [];
      this.patientForm.get('doctorId')?.disable();
    }
    this.patientForm.get('doctorId')?.setValue('');
  }
  onRoomTypeChange() {
    const selectedRoomType = this.patientForm.get('roomType')?.value;
    if (selectedRoomType) {
      this.filteredRooms = this.rooms.filter((room: any) => room.type === selectedRoomType);
      this.patientForm.get('roomId')?.enable();
    } else {
      this.filteredRooms = [];
      this.patientForm.get('roomId')?.disable();
    }
    this.patientForm.get('roomId')?.setValue('');
  }
  onRoomChange() {
    const selectedRoomId = this.patientForm.get('roomId')?.value;
    const selectedRoom = this.rooms.find(room => room.id === +selectedRoomId);
    this.selectedDailyPrice = selectedRoom ? selectedRoom.dailyPrice : null;
    if (selectedRoomId) {
      this.filteratedBeds = this.beds.filter((bed: any) => bed.roomId === +selectedRoomId);
      this.patientForm.get('bedId')?.enable();
    } else {
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
  // 
  searchPatientByPhone(event: Event) {
    const input = event.target as HTMLInputElement;
    const phoneNumber = input.value.trim();

    if (phoneNumber.length === 11 && /^01[0125][0-9]{8}$/.test(phoneNumber)) {
      this.pagingFilterModel.searchText = phoneNumber;
      this.addmisionService.getAddmision(this.pagingFilterModel).subscribe({
        next: (data) => {
          const patientId = data.results[0].patientId;
          if (data.results && data.results.length > 0) {
            const patient = data.results[0];
            let genderValue = '';
            if (patient.patientGender === 'ذكر') {
              genderValue = 'Male';
            } else if (patient.patientGender === 'أنثى') {
              genderValue = 'Female';
            }
            const birthDate = patient.dateOfBirth ? patient.dateOfBirth.split('T')[0] : '';
            this.patientForm.patchValue({
              patientName: patient.patientName,
              patientPhone: patient.phone,
              patientNationalId: patient.patientNationalId,
              patientBirthDate: birthDate,
              patientGender: genderValue,
              patientAddress: patient.address
            });
            this.patientForm.get('patientName')?.disable();
            this.patientForm.get('patientPhone')?.disable();
            this.patientForm.get('patientNationalId')?.disable();
            this.patientForm.get('patientBirthDate')?.disable();
            this.patientForm.get('patientGender')?.disable();

            this.messageService.add({
              severity: 'success',
              summary: 'تم العثور على المريض',
              detail: 'تم تسجيل بيانات المريض تلقائياً',
            });
          } else {
            this.messageService.add({
              severity: 'info',
              summary: 'لا يوجد مريض',
              detail: 'لم يتم العثور على مريض بهذا الرقم',
            });
          }
        },
        error: (err) => {
          console.error('Search error:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'خطأ في البحث',
            detail: 'حدث خطأ أثناء البحث عن المريض',
          });
        }
      });
    }
  }
}
