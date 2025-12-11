using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums;

public enum AdmissionStatus
{
    Admitted = 1,          // المريض تم تنويمه وهو داخل المستشفى الآن
    UnderTreatment = 2,    // المريض يتلقى علاج أدوية / تمريض / متابعة
    InSurgery = 3,         // المريض داخل غرفة العمليات الحالية
    InICU = 4,             // المريض في العناية المركزة
    Transferred = 5,       // تم نقل المريض إلى قسم / غرفة / سرير آخر
    Discharged = 6,        // المريض خرج من المستشفى
    LAMA = 7,              // Left Against Medical Advice = خرج رغمًا عن قرار الطبيب
    Deceased = 8           // المريض توفّى أثناء التنويم
}

