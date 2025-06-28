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

   static regexPattern(type: RegexType, message: string =null): ValidatorFn {
    const regex = regexList.find(x => x.type === type);
    if (!regex) {
      return (control: AbstractControl) => null;
    }
  
    return (control: AbstractControl) => {
      if (control.value && !regex.pattern.test(control.value)) {
        return { regexPattern: message ? message : regex.message };
      }
      return null;
    };
  }

  static dateGreaterThan(specificDate: Date, message: string): ValidatorFn {
    return (control: AbstractControl) => {
      if (control.value) {
        const inputDate = new Date(control.value);
        specificDate.setHours(0, 0, 0, 0);
        inputDate.setHours(0, 0, 0, 0);
        if (inputDate <= specificDate) {
          return { dateGreaterThan: message };
        }
      }
      return null;
    };
  }
}

export enum RegexType {
  text=1,
  email,
  url,
  number,
  date,
  alpha,
  alphaAllowSpaces,
  alphaAllowSpacesAndSplash,
  alphaNumeric,
  alphaNumericAllowSpaces,
  alphaNumericAllowDash,
  numericAllowDash,
  numeric,
  currency,
  addressLine
}

export interface RegexModel{
  pattern: RegExp;
  message: string;
  type: RegexType;
}

export const regexList: RegexModel[] = [
  {
    pattern: /^[0-9]+(\.[0-9]+)?$/,
    message: "ادخل ارقام فقط",
    type: RegexType.number
  },
  {
    pattern: /^[a-zA-Z]+$/,
    message: "يُسمح باستخدام الأحرف الأبجدية فقط.",
    type: RegexType.alpha
  },
  {
    pattern: /^[a-zA-Z\s]+$/,
    message: "يُسمح باستخدام الأحرف الأبجدية والمسافات فقط.",
    type: RegexType.alphaAllowSpaces
  },
  {
    pattern: /^[a-zA-Z\s/]+$/,
    message: "يُسمح باستخدام الأحرف الأبجدية، والمسافات، والشرط المائلة ( / ) فقط..",
    type: RegexType.alphaAllowSpacesAndSplash
  },
  {
    pattern: /^[a-zA-Z0-9]+$/,
    message: "يُسمح باستخدام الأحرف والأرقام فقط.",
    type: RegexType.alphaNumeric
  },
  {
    pattern: /^[a-zA-Z0-9\s]+$/,
    message: "يُسمح باستخدام الأحرف والأرقام والمسافات فقط.",
    type: RegexType.alphaNumericAllowSpaces
  },
  {
    pattern: /^[a-zA-Z0-9-]+$/,
    message: "يُسمح باستخدام الأحرف والأرقام والشرط ( - ) فقط.",
    type: RegexType.alphaNumericAllowDash
  },
  {
    pattern: /^\d+$/,
    message: "يُسمح باستخدام الأرقام فقط.",
    type: RegexType.numeric
  },
  {
    pattern: /^[0-9-]+$/,
    message: "يُسمح باستخدام الأرقام والشرط ( - ) فقط.",
    type: RegexType.numericAllowDash
  },
  {
    pattern: /^\d+(\.\d{1,2})?$/,
    message: "يُسمح باستخدام القيم الرقمية التي تحتوي على منزلتين عشريتين كحد أقصى فقط.",
    type: RegexType.currency
  },
  {
    pattern: /^[\w\s,-]+$/,
    message: "يُسمح باستخدام الأحرف، والأرقام، والمسافات، والفواصل (،)، والشرط ( - ) فقط.",
    type: RegexType.addressLine
  },
  {
    pattern: /^\d{4}-\d{2}-\d{2}$/,
    message: "يجب أن يكون التاريخ بصيغة YYYY-MM-DD (سنة-شهر-يوم).",
    type: RegexType.date
  },
  {
    pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
    message: "يرجى إدخال عنوان بريد إلكتروني صالح.",
    type: RegexType.email
  },
  {
    pattern: /^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$/,
    message: "يرجى إدخال رابط URL صالح.",
    type: RegexType.url
  },
  {
    pattern: /^[a-zA-Z\s]+$/,
    message: "يُسمح باستخدام أحرف النص فقط.",
    type: RegexType.text
  }
];