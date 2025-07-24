import { AbstractControl, ValidationErrors } from '@angular/forms';

export function todayDateValidator(control: AbstractControl): ValidationErrors | null {
  const controlValue = control.value;

  if (!controlValue) return null;

  const selectedDate = new Date(controlValue).toISOString().substring(0, 10);
  const today = new Date().toISOString().substring(0, 10);

  return selectedDate === today ? null : { notToday: true };
}
