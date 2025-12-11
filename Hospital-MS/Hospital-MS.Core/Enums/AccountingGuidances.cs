using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum AccountingGuidances
    {
        [EnumMember(Value = "الخزينة")]
        Treasury,

        [EnumMember(Value = "التصنيع")]
        Manufacturing,

        [EnumMember(Value = "البنوك")]
        Banks,

        [EnumMember(Value = "المخازن")]
        Stores,

        [EnumMember(Value = "الإيرادات")]
        Revenues,

        [EnumMember(Value = "الاستحقاق")]
        Accrual,

        [EnumMember(Value = "قيد افتتاحي")]
        OpeningEntry,

        [EnumMember(Value = "قيد إقفال")]
        ClosingEntry
    }
}
