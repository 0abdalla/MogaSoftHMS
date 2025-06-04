using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.StoreCount;
public class StoreCountResponse
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string StoreName { get; set; } 
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public AuditResponse Audit { get; set; }
}
