using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Patients
{
    public class PatientMedicalHistoryResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
