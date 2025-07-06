import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';

@Component({
  selector: 'app-year-fin-settings',
  templateUrl: './year-fin-settings.component.html',
  styleUrl: './year-fin-settings.component.css'
})
export class YearFinSettingsComponent {
  TitleList = ['إعدادات النظام','إعدادات الإدارة المالية' , 'إعدادات السنة المالية'];
  financialYearForm: FormGroup;

  constructor(private fb:FormBuilder){}
  ngOnInit() {
    this.financialYearForm = this.fb.group({
      startDate: [null, Validators.required],
      endDate: [null, Validators.required]
    }, { validators: this.validateDateRange });
  }
  
  validateDateRange(group: AbstractControl): ValidationErrors | null {
    const start = group.get('startDate')?.value;
    const end = group.get('endDate')?.value;
  
    if (start && end && new Date(end) < new Date(start)) {
      return { invalidRange: true };
    }
    return null;
  }
  
  submitFinancialYear() {
    if (this.financialYearForm.valid) {
      const data = this.financialYearForm.value;
      console.log('تم إرسال السنة المالية:', data);
    }
  }
}
