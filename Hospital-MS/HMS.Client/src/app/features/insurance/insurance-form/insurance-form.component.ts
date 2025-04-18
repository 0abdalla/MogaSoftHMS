import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
export interface InsuranceCompany {
  id: number;
  name: string;
  code: string;
  contactNumber: string;
  email: string;
  address: string;
  status: 'Active' | 'Inactive';
  registrationDate?: Date;
  processedClaimsCount?: number;
  contractDetails?: {
    description: string;
    startDate: Date;
    endDate: Date;
    categories: { name: string; coveragePercentage: number }[];
  };
}
@Component({
  selector: 'app-insurance-form',
  templateUrl: './insurance-form.component.html',
  styleUrl: './insurance-form.component.css'
})
export class InsuranceFormComponent {
  insuranceForm: FormGroup;
  isEditMode = false;
  companies: InsuranceCompany[] = [];
  editCompanyId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.insuranceForm = this.fb.group({
      name: ['', Validators.required],
      code: [''],
      contactNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      status: ['Active', Validators.required],
      contractDetails: this.fb.group({
        description: [''],
        startDate: [new Date().toISOString().split('T')[0], Validators.required],
        endDate: [new Date().toISOString().split('T')[0], Validators.required],
        categories: this.fb.array([])
      }),
      assignedAgents: this.fb.array([])
    });
  }

  ngOnInit() : void {
    
  }
  get categories() {
    return this.insuranceForm.get('contractDetails.categories') as FormArray;
  }

  get assignedAgents() {
    return this.insuranceForm.get('assignedAgents') as FormArray;
  }

  addCategory() {
    const categoryName = this.insuranceForm.get('contractDetails')?.get('categories')?.value.length + 1;
    const coverage = this.insuranceForm.get('contractDetails')?.get('categories')?.value.length * 10;
    this.categories.push(this.fb.group({
      name: [`Category ${categoryName}`, Validators.required],
      coveragePercentage: [coverage <= 100 ? coverage : 100, [Validators.required, Validators.min(0), Validators.max(100)]]
    }));
  }

  removeCategory(index: number) {
    this.categories.removeAt(index);
  }

  onSubmit() {
    if (this.insuranceForm.invalid) {
      alert('يرجى ملء جميع الحقول المطلوبة بشكل صحيح');
      return;
    }

  }

  resetForm() {
    this.insuranceForm.reset({
      name: '',
      code: '',
      contactNumber: '',
      email: '',
      address: '',
      status: 'Active',
      contractDetails: {
        description: '',
        startDate: new Date().toISOString().split('T')[0],
        endDate: new Date().toISOString().split('T')[0],
        categories: []
      },
    });
    this.categories.clear();
    this.assignedAgents.clear();
  }
}
