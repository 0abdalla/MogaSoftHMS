import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { InsuranceCompany } from '../../../../Models/HMS/insurance';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
@Component({
  selector: 'app-insurance-form',
  templateUrl: './insurance-form.component.html',
  styleUrl: './insurance-form.component.css'
})
export class InsuranceFormComponent {
  insuranceForm: FormGroup;
  companies: InsuranceCompany[] = [];

  constructor(
    private fb: FormBuilder,
    private insuranceService: InsuranceService,
    private messageService : MessageService
  ) {
    this.insuranceForm = this.fb.group({
      name: ['', Validators.required],
      code: [''],
      contactNumber: ['', [Validators.required , Validators.pattern(/^01[0125][0-9]{8}$/)]],
      email: ['', [Validators.required, Validators.email]],
      contractStartDate: ['', Validators.required],
      contractEndDate: ['', Validators.required],
      insuranceCategories: this.fb.array([])
    }, { validators: this.dateRangeValidator });    
  }

  ngOnInit() : void {
    this.addCategory();
  }
  get insuranceCategories() {
    return this.insuranceForm.get('insuranceCategories') as FormArray;
  }

  addCategory() {
    this.insuranceCategories.push(this.fb.group({
      name: ['', Validators.required],
      rate: [null, [Validators.required, Validators.min(0), Validators.max(100)]]
    }));
  }

  removeCategory(index: number) {
    if (this.insuranceCategories.length <= 1) {
      this.messageService.add({
        severity: 'warn',
        summary: 'تحذير',
        detail: 'يجب أن يكون هناك على الأقل فئة واحدة'
      });
      return;
    }
    this.insuranceCategories.removeAt(index);
  }

  onSubmit() {
    this.insuranceForm.markAllAsTouched();
    if (this.insuranceForm.invalid) {
      this.messageService.add({
      severity: 'error',
      summary: 'خطأ',
      detail: 'يرجى ملء جميع الحقول المطلوبة بشكل صحيح'
    });
    return;
  }else{
    this.insuranceService.addInsurance(this.insuranceForm.value).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'تم الإضافة', detail: 'تم إضافة الشركة بنجاح' });
        this.resetForm();
      },
      error: (error) => {
        console.error('Error adding insurance', error);
        console.error('details:', this.insuranceForm.value);
        this.messageService.add({ severity: 'error', summary: 'فشل الإضافة', detail: 'حدث خطأ أثناء إضافة الشركة' });
      }
    });
  }
}

resetForm() {
  this.insuranceForm.reset({
    name: '',
    code: '',
    contactNumber: '',
    email: '',
    contractStartDate: '',
    contractEndDate: '',
    insuranceCategories: []
  });
  this.insuranceCategories.clear();
  this.addCategory();
  }
  // 
  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const startDate = group.get('contractStartDate')?.value;
    const endDate = group.get('contractEndDate')?.value;
    if (startDate && endDate && new Date(startDate) > new Date(endDate)) {
      return { invalidDateRange: true };
    }
    return null;
  }  
}
