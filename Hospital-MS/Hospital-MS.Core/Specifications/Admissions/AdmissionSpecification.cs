using Hospital_MS.Core.Contracts.Admissions;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.Admissions
{
    public class AdmissionSpecification : BaseSpecification<Admission>
    {
        public AdmissionSpecification()
        {
            AddIncludes();
        }
        public AdmissionSpecification(int patientId)
            : base(x => x.PatientId == patientId)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Includes.Add(x => x.CreatedBy);
            Includes.Add(x => x.UpdatedBy);
            Includes.Add(x => x.Patient);
            Includes.Add(x => x.Bed);
            Includes.Add(x => x.Room);
            Includes.Add(x => x.Doctor);
            Includes.Add(x => x.Department);
        }
    }
}
