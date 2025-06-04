using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class StoreCount : AuditableEntity // الجرد
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public bool IsActive { get; set; } = true;
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}
