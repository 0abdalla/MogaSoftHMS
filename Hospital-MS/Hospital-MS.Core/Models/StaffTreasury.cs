using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class StaffTreasury : AuditableEntity
{
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = default!;

    public int TreasuryId { get; set; }
    public Treasury Treasury { get; set; } = default!;

    public bool IsActive { get; set; } = true;
}