using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Errors
{
    public class GenericErrors
    {
        public static Error GetSuccess = new("تمت العملية بنجاح", Status.Success);

        public static Error AddSuccess = new("تمت الاضافة بنجاح", Status.Success);

        public static Error UpdateSuccess = new("تم التعديل بنجاح", Status.Success);

        public static Error DeleteSuccess = new("تم الحذف بنجاح", Status.Success);

        public static Error TransFailed = new("لقد حدث خطأ", Status.Failed);

        public static Error NotFound = new("هذا العنصر غير موجود", Status.NotFound);

        public static Error InvalidStatus = new("هذه الحالة غير صالحة", Status.Failed);

        public static Error InvalidType = new("هذا النوع غير صالح", Status.Failed);

        public static Error InvalidGender = new("هذا الجنس غير صالح", Status.Failed);

        public static Error InvalidMaritalStatus = new("هذه الحالة الاجتماعية غير صالحة", Status.Failed);

        public static Error NotEmergency = new("هذا الإجراء مسموح به فقط لنوع الطوارئ", Status.Failed);

        public static Error InvalidCredentials = new("اسم المستخدم او كلمة المرور غير صالح", Status.Unauthorized);

        public static Error DuplicateEmail = new("البريد الإلكتروني مسجل مسبقاً", Status.Conflict);

        public static Error SuccessLogin = new("تم تسجيل الدخول بنجاح", Status.Success);

        public static Error SuccessRegister = new("تم تسجيل مستخدم جديد بنجاح", Status.Success);

        
    }
}
