using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryMovementResponse
{
    public int Id { get; set; }
    public int TreasuryId { get; set; }
    public string TreasuryName { get; set; }
    public bool IsClosed { get; set; }
    public DateOnly OpenedId { get; set; }
    public DateOnly? ClosedIn { get; set; }
}
