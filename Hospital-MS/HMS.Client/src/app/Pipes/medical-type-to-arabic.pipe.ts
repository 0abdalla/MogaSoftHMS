import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'medicalTypeToArabic'
})
export class MedicalTypeToArabicPipe implements PipeTransform {
  transform(value: string): string {
    switch (value) {
      case 'General':
        return 'كشف';
      case 'Consultation':
        return 'استشارة';
      case 'Surgery':
        return 'عمليات';
      case 'Screening':
        return 'تحاليل';
      case 'Radiology':
        return 'أشعة';
      default:
        return value;
    }
  }

}
