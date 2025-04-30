using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum BedStatus
    {
        [EnumMember(Value = "متاح")]
        Available,
        [EnumMember(Value = "محجوز")]
        NotAvailable,
        [EnumMember(Value = "صيانه")]
        UnderMaintenance,
    }
}
