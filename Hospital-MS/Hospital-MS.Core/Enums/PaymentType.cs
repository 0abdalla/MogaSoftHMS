using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums;

public enum PaymentType
{
    [EnumMember(Value = "Cash")]
    Cash,
    [EnumMember(Value = "Credit")]
    Credit
}
