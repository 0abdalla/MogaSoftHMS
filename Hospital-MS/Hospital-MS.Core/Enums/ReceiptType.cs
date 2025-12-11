using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums;

public enum ReceiptType
{
    [EnumMember(Value = "استلام من مورد")]
    Supplier,

    [EnumMember(Value = "استلام من مخزن اخر")]
    StoreTransfer,

    [EnumMember(Value = "زيادة جرد")]
    InventoryIncrease,
}
