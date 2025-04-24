using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.Patients
{
    public class PatientCountsSpecification
    {
        //public PatientCountsSpecification(GetPatientsRequest request)
        //    : base(x =>
        //            (string.IsNullOrEmpty(request.Search) ||
        //            x.FullName.ToLower().Contains(request.Search) ||
        //            x.Phone.ToLower().Contains(request.Search)) &&
        //            (string.IsNullOrEmpty(request.Status) ||
        //            x.Status == Enum.Parse<PatientStatus>(request.Status)) &&
        //            (!request.FromDate.HasValue || x.CreatedOn.Date >= request.FromDate.Value.Date) &&
        //            (!request.ToDate.HasValue || x.CreatedOn.Date <= request.ToDate.Value.Date)
        //    )

        //{
        //}
    }
}
