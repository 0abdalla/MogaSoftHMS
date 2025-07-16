using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Disbursement;
public class DisbursementReq
{
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public int? JobDepartmentId { get; set; }
    public List<DisbursementItemReq> Items { get; set; } = new List<DisbursementItemReq>();
}
