using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Admissions
{
    public class UpdateAdmissionRequest
    {
        public string? HealthStatus { get; set; }
        public string? InitialDiagnosis { get; set; }
        public string? Notes { get; set; }

        public bool HasCompanion { get; set; }
        public string? CompanionName { get; set; }
        public string? CompanionPhone { get; set; }
        public string? CompanionNationalId { get; set; }

        public AdmissionStatus Status { get; set; }
        public PaymentMethodType? PaymentMethod { get; set; }
        public string? DischargeSummary { get; set; }
        public DateTime? DischargeDate { get; set; }
    }

}
