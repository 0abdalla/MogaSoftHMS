using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum RestrictionTypes
    {
        [EnumMember(Value = "قيد مركب")]
        CompoundEntry,

        [EnumMember(Value = "قيد مزدوج")]
        DoubleEntry,
    }
}
