using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum AppointmentType
    {
        [EnumMember(Value = "كشف")]
        General, 
        [EnumMember(Value = "استشارة")]
        Consultation, 
        [EnumMember(Value = "عمليات")]
        Surgery, 
        [EnumMember(Value = "تحاليل")]
        Screening, 
        [EnumMember(Value = "أشعة")]
        Radiology ,
        [EnumMember(Value = "طوارئ")]
        Emergency
    }
}
