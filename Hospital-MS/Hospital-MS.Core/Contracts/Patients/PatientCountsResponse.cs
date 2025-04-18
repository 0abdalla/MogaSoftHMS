using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Patients
{
    public class PatientCountsResponse
    {
        public int StayingCount { get; set; }
        public int SurgeryCount { get; set; }
        public int CriticalConditionCount { get; set; }
        public int TreatedCount { get; set; }
        public int ArchivedCount { get; set; }
        public int OutpatientCount { get; set; }
    }
}
