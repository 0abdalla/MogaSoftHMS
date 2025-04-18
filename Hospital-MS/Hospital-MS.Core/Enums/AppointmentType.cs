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
        [EnumMember(Value = "استشاره")]
        Consultation, 
        [EnumMember(Value = "عمليه")]
        Surgery, 
        [EnumMember(Value = "تحاليل")]
        Screening, 
        [EnumMember(Value = "اشعه")]
        Radiology ,
        [EnumMember(Value = "طوارئ")]
        Emergency
    }
}
