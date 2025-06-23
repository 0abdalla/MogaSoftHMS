import { AbstractControl, FormControl, ValidatorFn, Validators } from "@angular/forms";

export class CustomValidators extends Validators{
  
  static endDateGreaterThanStartDate(startDateCName: string, endDateCName: string,message=null): ValidatorFn {
    return (formGroup: AbstractControl) => {
      const startDate_C = formGroup.get(startDateCName);
      const endDate_C = formGroup.get(endDateCName);
      if (startDate_C?.value && endDate_C?.value) {
        const startDate = new Date(startDate_C?.value);
        const endDate = new Date(endDate_C?.value);
        if (startDate >= endDate) {
          endDate_C.setErrors({ endDateLessThanStartDate: message });
        } else {
          endDate_C.setErrors(null);
          return null;
        }
      }
      return null;
    };
  }
}