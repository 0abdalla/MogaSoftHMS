using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums;

public enum EmergencyStatus
{
    Waiting = 1,          // لسه داخل ومستنّي التقييم (Triage)
    UnderTriage = 2,      // الممرض بيعمل تقييم Vital Signs / Severity
    UnderAssessment = 3,  // الطبيب بيكشف وبيشخّص
    UnderTreatment = 4,   // علاج / ملاحظة / أدوية / تدخلات
    ReadyForDischarge = 5,// الحالة خلصت وبتتحضر للخروج
    Discharged = 6,       // خرج من الطوارئ بالكامل
    Admitted = 7,         // تم تحويله للتنويم (Admission)
    Transferred = 8       // اتنقل لقسم تاني (مثلاً ICU)
}

