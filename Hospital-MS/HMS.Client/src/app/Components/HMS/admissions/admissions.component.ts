import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdmissionService } from '../../../Services/HMS/admission.service';
import { FilterModel, PagingFilterModel } from '../../../Models/Generics/PagingFilterModel';
import { PagedResponseModel } from '../../../Models/Generics/PagedResponseModel';
import { ReactiveFormsModule } from '@angular/forms';
import { StaffService } from '../../../Services/HMS/staff.service';
import { MessageService } from 'primeng/api';
declare var bootstrap: any;


@Component({
  selector: 'app-admissions',
  templateUrl: './admissions.component.html',
  styleUrls: ['./admissions.component.css']
})
export class AdmissionsComponent implements OnInit {

  admissions: any[] = [];

  totalRecords = 0;
  pagingFilter: PagingFilterModel = {
    searchText: '',
    currentPage: 1,
    pageSize: 10,
    filterList: []
  };

  loading = false;

  departments: any[] = [];
  wards: any[] = [];
  rooms: any[] = [];
  beds: any[] = [];
  doctors: any[] = [];
  counts: any = {};
  patientDetails: any = null;
  medicalHistory: any = null;

  admissionForm!: FormGroup;
  total: number;
  selectedAdmissionId!: number;
  admissionStatusList = [
    { value: 1, label: 'تم تنويمه' },
    { value: 2, label: 'تحت العلاج' },
    { value: 3, label: 'في العمليات' },
    { value: 4, label: 'في العناية المركزة' },
    { value: 5, label: 'تم نقله' },
    { value: 6, label: 'خرج من المستشفى' },
    { value: 7, label: 'خرج ضد التعليمات الطبية' },
    { value: 8, label: 'متوفي' }
  ];
  paymentMethods = [
    { value: 0, label: 'كاش' },
    { value: 1, label: 'تأمين' }
  ];
  admissionTypes = [
    { value: 0, label: 'طوارئ' },
    { value: 1, label: 'دخول مخطط' },
    { value: 2, label: 'جراحة' },
    { value: 3, label: 'باطنة' },
    { value: 4, label: 'أطفال' },
    { value: 5, label: 'ولادة' },
    { value: 6, label: 'تحويل من العناية المركزة' },
    { value: 7, label: 'تحويل من الطوارئ' },
    { value: 8, label: 'ملاحظة' }
  ];



  constructor(private admissionService: AdmissionService, private fb: FormBuilder, private doctorService: StaffService, private messageService: MessageService,) {
    this.admissionForm = this.fb.group({
      patientName: ['', [Validators.required, Validators.minLength(3)]],
      patientPhone: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{11}$/)
        ]
      ],
      patientBirthDate: ['', Validators.required],
      patientNationalId: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{14}$/)
        ]
      ],
      patientAddress: ['', Validators.required],
      patientStatus: [1, Validators.required],
      patientGender: [0, Validators.required],

      emergencyContact01: [
        '',
        Validators.pattern(/^\d{11}$/)
      ],
      emergencyPhone01: [
        '',
        Validators.pattern(/^\d{11}$/)
      ],
      emergencyContact02: [
        '',
        Validators.pattern(/^\d{11}$/)
      ],
      emergencyPhone02: [
        '',
        Validators.pattern(/^\d{11}$/)
      ],

      admissionType: [0, Validators.required],
      paymentMethod: [1, Validators.required],

      departmentId: [null, Validators.required],
      doctorId: [null],
      roomId: [null],
      bedId: [null],

      insuranceCompanyId: [null],
      insuranceCategoryId: [null],
      insuranceNumber: [''],

      healthStatus: [''],
      initialDiagnosis: [''],

      hasCompanion: [false],
      companionName: [''],
      companionNationalId: ['', Validators.pattern(/^\d{14}$/)],
      companionPhone: ['', Validators.pattern(/^\d{11}$/)],

      notes: ['']
    });



  }


  ngOnInit(): void {
    this.loadAdmissions();
    this.loadDepartments();
    this.loadRoomsBeds();
    this.loadCounts();
    this.getDoctors();
  }

  loadAdmissions(): void {
    this.loading = true;
    this.admissionService.getAdmissions(this.pagingFilter).subscribe({
      next: res => {
        this.admissions = Array.isArray(res.results) ? res.results : [res.results];
        this.totalRecords = res.totalCount;
        this.loading = false;
      },
      error: () => this.loading = false
    });

  }

  onSearch(searchText: string): void {
    this.pagingFilter.searchText = searchText;
    this.pagingFilter.currentPage = 1;
    this.loadAdmissions();
  }

  changePage(page: number): void {
    this.pagingFilter.currentPage = page;
    this.loadAdmissions();
  }


  loadDepartments(): void {

    this.admissionService.getDepartments().subscribe(res => {
      console.log("res", res)
      this.departments = res.results
    });
  }

  loadRoomsBeds(): void {
    this.admissionService.getRooms().subscribe(res => this.rooms = res.results);
    this.admissionService.getBeds().subscribe(res => this.beds = res.results);
  }

  loadCounts(): void {
    this.admissionService.getCounts().subscribe(res => this.counts = res.results);
  }

  getAdmissionById(id: number): void {
    this.admissionService.getAdmissionById(id).subscribe(res => {
      this.admissionForm.patchValue(res.results);
    });
  }

  addAdmission(): void {
    if (this.admissionForm.invalid) {
      this.admissionForm.markAllAsTouched();
      return;
    }

    this.admissionService.addAdmission(this.admissionForm.value).subscribe({
      next: (res) => {
        this.messageService.add({
          severity: 'success',
          summary: 'تمت الإضافة',
          detail: 'تم إضافة الدخول بنجاح'
        });
        this.loadAdmissions();
        this.admissionForm.reset();
      },
      error: (err) => {
        console.error(" API ERROR:", err);
        this.messageService.add({
          severity: 'error',
          summary: 'فشل الإضافة',
          detail: 'حدث خطأ أثناء إضافة البيانات'
        });

      }
    });
  }




  updateAdmission(): void {
    const formData = new FormData();
    Object.keys(this.admissionForm.value).forEach(key =>
      formData.append(key, this.admissionForm.value[key])
    );

    this.admissionService.updateAdmission(this.selectedAdmissionId, formData)
      .subscribe(() => {
        this.loadAdmissions();
        this.admissionForm.reset();
      });
  }


  deleteAdmission(id: number): void {
    this.admissionService.deleteAdmission(id).subscribe(() => this.loadAdmissions());
  }

  getMedicalHistory(patientId: number): void {
    this.admissionService.getMedicalHistory(patientId).subscribe(res => console.log(res));
  }
  getDoctors() {
    this.doctorService.getDoctors(this.pagingFilter).subscribe({
      next: (res) => {
        console.log("doc", res.results)
        this.doctors = res.results.map((doctor: any) => {
          if (doctor.status === 'Active') {
            doctor.status = 'متاح';
          } else {
            doctor.status = 'غير متاح';
          }
          return doctor;
        });

        this.total = res.totalCount;
      },
      error: () => { },
    });
  }
  openPatientDetails(admission: any) {
    this.admissionService.getAdmissionById(admission.id).subscribe(res => {
      this.patientDetails = res.results;
      this.admissionService.getMedicalHistory(admission.patientId).subscribe(hist => {
        this.medicalHistory = hist;
        const modal = new bootstrap.Modal(
          document.getElementById('inpatientDetailsModal') as HTMLElement
        );
        modal.show();
      });
    });
  }

  // edit
  editAdmission(data: any) {

    this.selectedAdmissionId = data.id;
    this.admissionForm.patchValue({
      patientName: data.patientName,
      patientPhone: data.patientPhone,
      patientGender: data.patientGender,
      patientBirthDate: data.patientBirthDate,
      patientNationalId: data.patientNationalId,
      patientAddress: data.patientAddress,
      departmentId: data.departmentId,
      doctorId: data.doctorId,
      roomId: data.roomId,
      bedId: data.bedId,
      admissionType: data.admissionType,
      paymentMethod: data.paymentMethod,
      companionName: data.companionName,
      companionNationalId: data.companionNationalId,
      companionPhone: data.companionPhone,
      notes: data.notes
    });
    const modalElement = document.getElementById('editAdmissionModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
  }
  mapStatusToArabic(status: string): string {
    switch (status) {
      case 'Admitted': return 'تم تنويمه';
      case 'UnderTreatment': return 'تحت العلاج';
      case 'InSurgery': return 'في غرفة العمليات';
      case 'InICU': return 'في العناية المركزة';
      case 'Transferred': return 'تم نقله';
      case 'Discharged': return 'خرج من المستشفى';
      case 'LAMA': return 'خرج ضد النصيحة الطبية';
      case 'Deceased': return 'متوفى';
      default: return status;
    }
  }
  getStatusColor(status: string): string {
    switch (status) {
      case 'Admitted': return '#3A86FF';
      case 'UnderTreatment': return '#FFA500';
      case 'InSurgery': return '#FF006E';
      case 'InICU': return '#6A00F4';
      case 'Transferred': return '#8338EC';
      case 'Discharged': return '#06D6A0';
      case 'LAMA': return '#FFBE0B';
      case 'Deceased': return '#000000';
      default: return '#999999';
    }
  }


}

