using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Patients
{
    public class PatientResponse
    {
        // Patient Information
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string PatientStatus { get; set; }

        // Audit Information
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
