import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { InsuranceService } from '../../../../Services/HMS/insurance.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-insurance-edit',
  templateUrl: './insurance-edit.component.html',
  styleUrl: './insurance-edit.component.css'
})
export class InsuranceEditComponent implements OnInit {
  insuranceForm!:FormGroup;
  // 
  companyId: number = 0;
  constructor(
    private fb: FormBuilder,
    private insuranceService: InsuranceService,
    private messageService : MessageService,
    private activ : ActivatedRoute
    ) {
      this.insuranceForm = this.fb.group({
        name: ['', Validators.required],
        code: [''],
        phone: ['', [Validators.required , Validators.pattern(/^01[0125][0-9]{8}$/)]],
        email: ['', [Validators.required, Validators.email]],
        contractStartDate: ['', Validators.required],
        contractEndDate: ['', Validators.required],
        insuranceCategories: this.fb.array([])
      }, { validators: this.dateRangeValidator });    
      this.activ.params.subscribe((params) => {
        this.companyId = params['id'];
      });    
    }
  
  ngOnInit() : void {
    this.addCategory();
    this.getCompanyId(this.companyId);
  }
  getCompanyId(id: number) {
    this.insuranceService.getInsuranceById(id).subscribe({
      next: (response: any) => {
        const company = response.results;
        this.insuranceForm.patchValue({
          name: company.name,
          code: company.code,
          phone: company.phone,
          email: company.email,
          contractStartDate: company.contractStartDate,
          contractEndDate: company.contractEndDate
        });
        const categoriesArray = this.insuranceForm.get('insuranceCategories') as FormArray;
        categoriesArray.clear();
        company.insuranceCategories.forEach((cat: any) => {
          categoriesArray.push(this.fb.group({
            name: [cat.name, Validators.required],
            rate: [cat.rate, [Validators.required, Validators.min(0), Validators.max(100)]]
          }));
        });
      },
      error: (error) => {
        console.error('Error retrieving insurance', error);
        this.messageService.add({ severity: 'error', summary: 'فشل', detail: 'حدث خطأ أثناء جلب الشركة' });
      }
    });
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
  onSubmit(){
    if (this.insuranceForm.invalid) {
      this.insuranceForm.markAllAsTouched();
      return;
    }else{
      this.insuranceService.updateInsurance(this.companyId , this.insuranceForm.value).subscribe({
        next: (response) => {
          this.messageService.add({
            severity: 'success',
            summary: 'نجاح',
            detail: 'تم تحديث الشركة بنجاح'
          });
        },
        error: (error) => {
          console.error('Error updating insurance', error);
          this.messageService.add({
            severity: 'error',
            summary: 'فشل',
            detail: 'حدث خطأ أثناء تحديث الشركة'
          });
        }
      });
    }
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
