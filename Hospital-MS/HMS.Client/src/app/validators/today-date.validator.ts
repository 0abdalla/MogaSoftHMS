import { AbstractControl, ValidationErrors } from '@angular/forms';

export function todayDateValidator(control: AbstractControl): ValidationErrors | null {
  const controlValue = control.value;

  if (!controlValue) return null;

  const selectedDate = new Date(controlValue).toISOString().substring(0, 10);
  const today = new Date().toISOString().substring(0, 10);

  return selectedDate === today ? null : { notToday: true };
}

export function notOldDayValidator(control: AbstractControl): ValidationErrors | null {
  const selectedDate = new Date(control.value);
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  if (selectedDate < today) {
    return { notToday: true };
  }
  return null;
}
export function exactTodayValidator(control: AbstractControl): ValidationErrors | null {
  const controlValue = control.value;
  if (!controlValue) return null;

  const selectedDate = new Date(controlValue);
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  selectedDate.setHours(0, 0, 0, 0);

  if (selectedDate < today) {
    return { pastDate: true };
  } else if (selectedDate > today) {
    return { futureDate: true };
  }

  return null;
}


