using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum StaffType
    {
        [EnumMember(Value = "طبيب")]
        Doctor,
        [EnumMember(Value = "ممرض")]
        Nurse,
        [EnumMember(Value = "عامل")]
        Worker,
        [EnumMember(Value = "اداري")]
        Administrator

        //Receptionist,
        //Pharmacist,
        //Cleaner,
        //Security
    }
}
