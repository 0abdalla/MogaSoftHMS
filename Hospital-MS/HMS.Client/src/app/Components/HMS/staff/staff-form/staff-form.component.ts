import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FinancialService } from '../../../../Services/HMS/financial.service';
import { PagingFilterModel } from '../../../../Models/Generics/PagingFilterModel';

@Component({
  selector: 'app-staff-form',
  templateUrl: './staff-form.component.html',
  styleUrl: './staff-form.component.css',
  animations: [
    trigger('fadeIn', [
      transition('void => *', [
        style({ opacity: 0 }),
        animate('300ms ease-in', style({ opacity: 1 }))
      ])
    ])
  ]
})
export class StaffFormComponent implements OnInit {
  employeeForm: FormGroup;
  selectedFiles: File[] = [];
  // 
  jobDepartments: any;
  jobTitles: any;
  jobTypes: any;
  jobLevels: any;
  branches: any;
  filteredJobTitles: any;
  pagingFilterModel: PagingFilterModel = {
    pageSize: 100,
    currentPage: 1,
    filterList: []
  }
  netSalary: number;
  // 
  employeeId: number;
  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
    private financialService: FinancialService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.employeeId = +id;
        this.loadEmployeeData(this.employeeId);
      }
    });
    this.employeeForm = this.fb.group({
      fullName: ['', Validators.required],
      nationalId: ['', [Validators.required, Validators.pattern(/^[0-9]{14}$/)]],
      gender: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      type: [''],
      hireDate: ['', Validators.required],
      // 
      branchId: ['', Validators.required],
      basicSalary: [null, Validators.required],
      allowances: [null , [Validators.required , Validators.min(0)]],
      tax: [null, [Validators.required, Validators.min(0), Validators.max(100)]],
      insurance: [null, [Validators.required, Validators.min(0), Validators.max(100)]],
      vacationDays: [null, [Validators.required, Validators.min(0), Validators.max(30)]],
      // 
      status: ['Active', Validators.required],
      maritalStatus: ['', Validators.required],
      notes: [''],
      Files: [null],
      JobDepartmentId: ['', Validators.required],
      JobLevelId: ['', Validators.required],
      JobTitleId: [{ value: '', disabled: true }, Validators.required],
      JobTypeId: ['', Validators.required],
      userName: [''],
      password: ['' , Validators.minLength(8)],
      isAuthorized: [null, Validators.required]
    });
    this.employeeForm.get('basicSalary')!.valueChanges.subscribe(() => this.calculateNetSalary());
    this.employeeForm.get('tax')!.valueChanges.subscribe(() => this.calculateNetSalary());
    this.employeeForm.get('insurance')!.valueChanges.subscribe(() => this.calculateNetSalary());
    this.employeeForm.get('allowances')!.valueChanges.subscribe(() => this.calculateNetSalary());
  }
  calculateNetSalary() {
    const salary = Number(this.employeeForm.get('basicSalary')?.value);
    const taxPercent = Number(this.employeeForm.get('tax')?.value);
    const insurancePercent = Number(this.employeeForm.get('insurance')?.value);
    const allowances = Number(this.employeeForm.get('allowances')?.value);
    if (!salary || isNaN(taxPercent) || isNaN(insurancePercent)) {
      this.netSalary = null;
      return;
    }
    const taxAmount = salary * (taxPercent / 100);
    const insuranceAmount = salary * (insurancePercent / 100);
    this.netSalary = salary - taxAmount - insuranceAmount + allowances;
    const shouldDisable = this.netSalary <= 0;
    const isDisabled = this.employeeForm.disabled;
    if (shouldDisable) {
      this.employeeForm.get('basicSalary')?.setErrors({ 'invalid': true });
    } else {
      this.employeeForm.get('basicSalary')?.setErrors(null);
    }
  }

  ngOnInit() {
    this.employeeForm.get('isAuthorized')?.valueChanges.subscribe(value => {
      const userNameControl = this.employeeForm.get('userName');
      const passwordControl = this.employeeForm.get('password');
      if (value === true) {
        userNameControl?.setValidators([Validators.required]);
        passwordControl?.setValidators([Validators.required]);
      } else {
        userNameControl?.clearValidators();
        passwordControl?.clearValidators();
        userNameControl?.reset();
        passwordControl?.reset();
      }
      userNameControl?.updateValueAndValidity();
      passwordControl?.updateValueAndValidity();
    });
    this.employeeForm.get('JobDepartmentId')?.valueChanges.subscribe(value => {
      const jobTitleControl = this.employeeForm.get('JobTitleId');
      jobTitleControl?.setValue('');
      if (value) {
        this.filteredJobTitles = this.jobTitles.filter((title: any) => title.jobDepartmentId === parseInt(value));
        jobTitleControl?.enable();
      } else {
        this.filteredJobTitles = [];
        jobTitleControl?.disable();
      }
    });
    this.loadStaffData();
  }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const files: FileList = target.files as FileList;
    this.selectedFiles = [];

    if (files && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const validTypes = ['image/png', 'image/jpeg', 'application/pdf'];
        if (validTypes.includes(file.type)) {
          this.selectedFiles.push(file);
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'خطأ',
            detail: `الملف ${file.name} غير مدعوم. يُسمح فقط بـ PNG, JPG, JPEG, PDF.`,
          });
        }
      }
    }
  }
  onSubmit() {
    console.log(this.employeeForm.value);
    console.log(this.employeeId);
    
    if (this.employeeForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'فشل',
        detail: 'يرجى ملء جميع الحقول المطلوبة بشكل صحيح',
      });
      return;
    }
    const formData = new FormData();
    this.selectedFiles?.forEach((file, index) => {
      formData.append(`Files[${index}]`, file, file.name);
    });
    Object.keys(this.employeeForm.controls).forEach(key => {
      formData.append(key, this.employeeForm.get(key)?.value ?? '');
    });
    if (this.employeeId) {
      this.staffService.updateStaff(this.employeeId, formData).subscribe({
        next: (res: any) => {
          if (res.isSuccess === true) {
            this.messageService.add({
              severity: 'success',
              summary: 'نجاح',
              detail: 'تم تعديل بيانات الموظف بنجاح',
            });
            setTimeout(() => {
              this.router.navigate(['/hms/staff/list']);
            }, 1000);
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'فشل',
              detail: 'فشل في تعديل بيانات الموظف',
            });
            console.log('update', res);
          }
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'فشل',
            detail: 'فشل في تعديل بيانات الموظف',
          });
        },
      });
    }
    
    else {
      this.staffService.addStaff(formData).subscribe({
        next: (res: any) => {
          if (res.isSuccess === true) {
            this.messageService.add({
              severity: 'success',
              summary: 'نجاح',
              detail: 'تم إضافة الموظف بنجاح',
            });
            setTimeout(() => {
              this.router.navigate(['/hms/staff/list']);
            }, 1000);
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل',
              detail: 'فشل في إضافة الموظف',
            });
            console.log('add',res);
            console.log(res.message);
            console.log(res.errors);
            
          }
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'فشل',
            detail: 'فشل في إضافة الموظف',
          });
        }
      });
    }
  }
  
  loadStaffData() { 
    forkJoin({
      jobDepartments: this.staffService.getJobDepartment('', 1, 100),
      jobTitles: this.staffService.getJobTitles('', 1, 100),
      jobTypes: this.staffService.getJobTypes('', 1, 100),
      jobLevels: this.staffService.getJobLevels('', 1, 100),
      branches: this.staffService.GetBranches(this.pagingFilterModel)
    }).subscribe({
      next: (data) => {
        this.jobDepartments = data.jobDepartments.results;
        this.jobTypes = data.jobTypes.results;
        this.jobTitles = data.jobTitles.results;
        this.jobLevels = data.jobLevels.results;
        this.branches = data.branches.results;
        if (this.employeeData?.jobDepartmentId) {
          this.onDepartmentChange(this.employeeData.jobDepartmentId);
          this.employeeForm.patchValue({
            JobTitleId: this.employeeData.jobTitleId
          });
        }
      },
      error: (error) => {
        console.error(error);
      }
    })
  }
  
  
  // 
  employeeData = null;
  loadEmployeeData(id: number) {
    this.staffService.getStaffById(id).subscribe({
      next: (res: any) => {
        this.employeeData = res.results;
        this.employeeId = this.employeeData.id;
        console.log(this.employeeData);
        this.employeeForm.patchValue({
          fullName: this.employeeData.fullName,
          nationalId: this.employeeData.nationalId,
          gender: this.employeeData.gender,
          phoneNumber: this.employeeData.phoneNumber,
          email: this.employeeData.email,
          address: this.employeeData.address,
          type: this.employeeData.type ?? '', 
          hireDate: this.employeeData.hireDate,
          branchId: this.employeeData.branchId,
          basicSalary: this.employeeData.basicSalary ?? 0,
          tax: this.employeeData.tax ?? 0,
          insurance: this.employeeData.insurance ?? 0,
          allowances: this.employeeData.allowances ?? 0,
          vacationDays: this.employeeData.vacationDays ?? 0,
          status: this.employeeData.status,
          maritalStatus: this.employeeData.maritalStatus,
          notes: this.employeeData.notes,
          JobDepartmentId: this.employeeData.jobDepartmentId,
          JobLevelId: this.employeeData.jobLevelId,
          JobTypeId: this.employeeData.jobTypeId,
          isAuthorized: this.employeeData.isAuthorized ?? null,
          userName: this.employeeData.userName ?? '',
          password: '',
        });
      },
      error: (err) => {
        console.error(err);
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل بيانات الموظف' });
      }
    });
  }
  onDepartmentChange(deptId: number) {
    if (!this.jobTitles) {
      this.filteredJobTitles = [];
      this.employeeForm.get('JobTitleId')?.disable();
      return;
    }
    this.filteredJobTitles = this.jobTitles.filter((x: any) => x.jobDepartmentId === deptId);
  
    if (this.filteredJobTitles.length > 0) {
      this.employeeForm.get('JobTitleId')?.enable();
    } else {
      this.employeeForm.get('JobTitleId')?.disable();
    }
  }   
}
