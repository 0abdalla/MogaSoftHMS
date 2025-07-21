using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class Attendance : AuditableEntity
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public int? BranchId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? InTime { get; set; }
    public TimeOnly? OutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }

    public Staff Staff { get; set; } = default!;
}
