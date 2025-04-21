using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.MedicalHistories
{
    public class AllPatientHistoriesSpecification : BaseSpecification<PatientMedicalHistory>
    {
        public AllPatientHistoriesSpecification(int patientId)
            : base(x => x.PatientId == patientId)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(x => x.Patient);
            Includes.Add(x => x.CreatedBy);
            Includes.Add(x => x.UpdatedBy);
        }
    }
}
