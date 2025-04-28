using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Patients
{
    public class PatientMedicalHistoryRequest
    {
        public int PatientId { get; set; }
        public string Description { get; set; }
    }
}
