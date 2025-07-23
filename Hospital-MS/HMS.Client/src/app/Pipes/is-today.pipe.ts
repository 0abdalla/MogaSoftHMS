import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'isToday'
})
export class IsTodayPipe implements PipeTransform {
  transform(value: string | Date): boolean {
    const today = new Date().toISOString().substring(0, 10);
    const inputDate = new Date(value).toISOString().substring(0, 10);
    return inputDate === today;
  }
}
