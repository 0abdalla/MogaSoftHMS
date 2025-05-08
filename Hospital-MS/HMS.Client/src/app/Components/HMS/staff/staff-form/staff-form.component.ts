import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { StaffService } from '../../../../Services/HMS/staff.service';
import { forkJoin } from 'rxjs';

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
  filteredJobTitles: any;
  constructor(
    private fb: FormBuilder,
    private staffService: StaffService,
    private messageService: MessageService
  ) {
    this.employeeForm = this.fb.group({
      fullName: ['', Validators.required],
      nationalId: ['', [Validators.required, Validators.pattern(/^[0-9]{14}$/)]],
      gender: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      type: [''],
      hireDate: ['', Validators.required],
      status: ['Active', Validators.required],
      maritalStatus: ['', Validators.required],
      notes: [''],
      Files: [null],
      JobDepartmentId: ['', Validators.required],
      JobLevelId: ['', Validators.required],
      JobTitleId: [{value : '' , disabled : true}, Validators.required],
      JobTypeId: ['', Validators.required],
      userName: [''],
      password: [''],
      isAuthorized: [null, Validators.required]
    });
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
        this.filteredJobTitles = this.jobTitles.filter((title : any) => title.jobDepartmentId === parseInt(value));
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
    if (this.employeeForm.invalid) {
      console.log('Form is invalid', this.employeeForm.errors);
      console.log(this.employeeForm.value);
      
      this.messageService.add({
        severity: 'error',
        summary: 'فشل',
        detail: 'يرجى ملء جميع الحقول المطلوبة بشكل صحيح',
      });
      return;
    }

    if (!this.selectedFiles || this.selectedFiles.length === 0) {
      this.messageService.add({
        severity: 'error',
        summary: 'فشل',
        detail: 'يجب اختيار ملف واحد على الأقل',
      });
      return;
    }

    const formData = new FormData();
    this.selectedFiles.forEach((file, index) => {
      formData.append(`Files[${index}]`, file, file.name);
    });
    formData.append('fullName', this.employeeForm.get('fullName')?.value || '');
    formData.append('nationalId' , this.employeeForm.get('nationalId')?.value || '')
    formData.append('gender', this.employeeForm.get('gender')?.value || '');
    formData.append('phoneNumber', this.employeeForm.get('phoneNumber')?.value || '');
    formData.append('email', this.employeeForm.get('email')?.value || '');
    formData.append('address', this.employeeForm.get('address')?.value || '');
    formData.append('type', this.employeeForm.get('type')?.value || '');
    formData.append('hireDate', this.employeeForm.get('hireDate')?.value || '');
    formData.append('status', this.employeeForm.get('status')?.value || '');
    formData.append('maritalStatus', this.employeeForm.get('maritalStatus')?.value || '');
    formData.append('notes', this.employeeForm.get('notes')?.value || '');
    formData.append('JobDepartmentId', (Number(this.employeeForm.get('JobDepartmentId')?.value) || 0).toString());
    formData.append('JobLevelId', (Number(this.employeeForm.get('JobLevelId')?.value) || 0).toString());
    formData.append('JobTitleId', (Number(this.employeeForm.get('JobTitleId')?.value) || 0).toString());
    formData.append('JobTypeId', (Number(this.employeeForm.get('JobTypeId')?.value) || 0).toString());
    formData.append('isAuthorized', this.employeeForm.get('isAuthorized')?.value?.toString() || 'false');
    formData.append('userName', this.employeeForm.get('userName')?.value || '');
    formData.append('password', this.employeeForm.get('password')?.value || '');
    this.staffService.addStaff(formData).subscribe({
      next: (data) => {
        console.log(data);
        console.log(formData);
        this.employeeForm.reset();
        this.selectedFiles = [];
        this.messageService.add({
          severity: 'success',
          summary: 'نجاح',
          detail: 'تم إضافة الموظف بنجاح',
        });
      },
      error: (error) => {
        console.error(error);
        this.messageService.add({
          severity: 'error',
          summary: 'فشل',
          detail: 'فشل في إضافة الموظف',
        });
      },
    });
  }
  loadStaffData(){
    forkJoin({
      jobDepartments: this.staffService.getJobDepartment('',1,100),
      jobTitles: this.staffService.getJobTitles('',1,100),
      jobTypes: this.staffService.getJobTypes('',1,100),
      jobLevels: this.staffService.getJobLevels('',1,100)
    }).subscribe({
      next: (data) => {
        this.jobDepartments = data.jobDepartments.results;
        this.jobTypes = data.jobTypes.results;
        this.jobTitles = data.jobTitles.results;
        this.filteredJobTitles = [];
        this.jobLevels = data.jobLevels.results;
        console.log('Departments : ',this.jobDepartments);
        console.log('Titles : ',this.jobTitles);
        console.log('Types : ',this.jobTypes);
        console.log('Levels : ',this.jobLevels);
      },
      error: (error) => {
        console.error(error);
      }
    })
  }
}
