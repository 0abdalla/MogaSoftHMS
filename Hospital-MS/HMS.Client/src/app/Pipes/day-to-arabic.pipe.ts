import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dayToArabic'
})
export class DayToArabicPipe implements PipeTransform {
  private dayMap: { [key: string]: string } = {
    'Friday': 'الجمعة',
    'Saturday': 'السبت',
    'Sunday': 'الأحد',
    'Monday': 'الإثنين',
    'Tuesday': 'الثلاثاء',
    'Wednesday': 'الأربعاء',
    'Thursday': 'الخميس'
  };

  transform(value: string): string {
    return this.dayMap[value] || value;
  }

}
