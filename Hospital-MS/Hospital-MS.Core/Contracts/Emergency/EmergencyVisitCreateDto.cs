using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Emergency;

public class EmergencyVisitCreateDto
{
    public string PatientName { get; set; } = default!;
    public string? Phone { get; set; }
    public Gender Gender { get; set; }
    public string ChiefComplaint { get; set; } = default!;
    public string? CompanionName { get; set; }
    public string? CompanionPhone { get; set; }
    public string? CompanionNationalId { get; set; }
}
// يدخلها موظف الاستقبال وقت دخول مريض الطوارئ