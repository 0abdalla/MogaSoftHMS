using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum BillingStatus
    {
        Unpaid,            // لم يتم الدفع  
        PartiallyPaid,     // دفع جزء  
        Paid,              // دفع كامل  
        Insurance,         // على التأمين  
        Corporate,         // شركة متكفلة  
        Refunded           // تم الترجيع  
    }
}
