using Azure.Core;
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
    public class AdmissionCountSpecification
    {
        //public AdmissionCountSpecification(GetAdmissionsRequest request)
        //: (x =>
        //            (string.IsNullOrEmpty(request.Search) ||
        //            x.Patient.FullName.ToLower().Contains(request.Search) ||
        //            x.Patient.Phone.ToLower().Contains(request.Search)) &&
        //            (string.IsNullOrEmpty(request.Status) ||
        //            x.Patient.Status == Enum.Parse<PatientStatus>(request.Status)) &&
        //            (!request.FromDate.HasValue || x.AdmissionDate.Date >= request.FromDate.Value.Date) &&
        //            (!request.ToDate.HasValue || x.AdmissionDate.Date <= request.ToDate.Value.Date)
        //    )
        //{

        //}
    }
}
