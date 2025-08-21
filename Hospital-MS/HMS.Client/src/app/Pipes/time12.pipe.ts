import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'time12'
})
export class Time12Pipe implements PipeTransform {

  transform(value: string): string {
    if (!value) return '';
    const [hoursStr, minutesStr] = value.split(':');
    let hours = parseInt(hoursStr, 10);
    const minutes = parseInt(minutesStr, 10);
    const suffix = hours >= 12 ? 'مساءً' : 'صباحًا';
    hours = hours % 12;
    hours = hours ? hours : 12;
    const formattedMinutes = minutes < 10 ? '0' + minutes : minutes;
    return `${hours}:${formattedMinutes} ${suffix}`;
  }
}
