using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Emergency;

public class EmergencyTriageDto
{
    public int EmergencyVisitId { get; set; }
    public EmergencySeverity Severity { get; set; }
    public string? Notes { get; set; }

    public string? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public int? RespiratoryRate { get; set; }
    public float? Temperature { get; set; }
    public int? OxygenSaturation { get; set; }

    public int? PainScore { get; set; }
    public string? Allergies { get; set; }
}
// يدخلها الممرض بعد وصول المريض