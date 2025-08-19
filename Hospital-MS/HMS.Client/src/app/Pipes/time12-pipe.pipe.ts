import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'time12Pipe'
})
export class Time12PipePipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return '';
    let date: Date;
    if (value.includes('T')) {
      date = new Date(value);
    } else {
      const [hours, minutes, seconds] = value.split(':').map(Number);
      date = new Date();
      date.setHours(hours, minutes || 0, seconds || 0);
    }
    let h = date.getHours();
    const m = date.getMinutes().toString().padStart(2, '0');
    const s = date.getSeconds().toString().padStart(2, '0');
    const period = h >= 12 ? 'مساءً' : 'صباحًا';
    h = h % 12 || 12;

    return `${h}:${m}:${s} ${period}`;
  }
}
