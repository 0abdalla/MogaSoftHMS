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
      case 'MRI':
        return 'أشعة رنين';
      case 'Panorama':
        return 'أشعة بانوراما';
      case 'XRay':
        return 'أشعة عادية';
      case 'CTScan':
        return 'أشعة مقطعية';
      case 'Ultrasound':
        return 'أشعة سونار';
      case 'Echo':
        return 'أشعة إيكو';
      case 'Mammogram':
        return 'أشعة ماموجرام';
      default:
        return value;
    }
  }

}
