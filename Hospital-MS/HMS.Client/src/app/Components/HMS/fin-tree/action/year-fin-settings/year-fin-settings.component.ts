import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { FinancialService } from '../../../../../Services/HMS/financial.service';

@Component({
  selector: 'app-year-fin-settings',
  templateUrl: './year-fin-settings.component.html',
  styleUrl: './year-fin-settings.component.css'
})
export class YearFinSettingsComponent {
  TitleList = ['إعدادات النظام','إعدادات الإدارة المالية' , 'إعدادات السنة المالية'];
  financialYearForm: FormGroup;

  constructor(private fb:FormBuilder , private financialService:FinancialService){}
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
    Swal.fire({
      title: 'هل تريد إنشاء السنة المالية؟',
      html: `
        برجاء مراعاة الآتي قبل بدء إنشاء السنة المالية الجديدة:<br>
        1- إقفال آخر حركة خزينة بالسنة المالية المنتهية<br>
        2- ترحيل جميع قيود اليومية لحسابات الأستاذ<br>
        3- إقفال السنة المالية المنتهية<br>
        <br>
        <strong>ملاحظة:</strong> يُرجى العلم أنه بعد الضغط على زر "إنشاء سنة مالية جديدة" لا يمكن التراجع نهائيًا.
      `,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم , إنشاء',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.financialService.postFiscalYear(this.financialYearForm.value).subscribe({
          next: (res:any) => {
            console.log(res);
            this.financialYearForm.reset();
            Swal.fire('تم إنشاء السنة المالية بنجاح', '', 'success');
          },
          error: (err) => {
            console.error('فشل الإضافة:', err);
            Swal.fire('فشل إنشاء السنة المالية', '', 'error');
          }
        });
      }
    });
  }  
}
