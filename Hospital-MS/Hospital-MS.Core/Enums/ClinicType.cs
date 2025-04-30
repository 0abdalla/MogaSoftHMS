using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Hospital_MS.Core.Enums
{
    public enum ClinicType
    {
        [EnumMember(Value = "عامه")]
        General, // عام
        [EnumMember(Value = "استشاره")]
        Consultation, // استشاره
        [EnumMember(Value = "تحاليل")]
        Screening, // تحاليل
        [EnumMember(Value = "أشعه")]
        Radiology,// أشعه

    }
}
