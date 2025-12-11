using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums;

public enum AccountTypes
{
    [EnumMember(Value = "قائمة مركز مالي")]
    ListFinancialCenters,

    [EnumMember(Value = "حساب أرباح وخسائر")]
    ProfitLossAccount,
}
