using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.Appointments
{
    public class AppointmentSpecification
    {
        //public AppointmentSpecification()
        //{
        //    AddIncludes();
        //}

        //public AppointmentSpecification(int id)
        //    : base(x => x.Id == id)
        //{
        //    AddIncludes();
        //}

        //public AppointmentSpecification(GetAppointmentsRequest request)
        //    : base(x => (string.IsNullOrEmpty(request.Search) ||
        //                x.Patient.FullName.ToLower().Contains(request.Search) ||
        //                x.Patient.Phone.ToLower().Contains(request.Search)) &&
        //                (string.IsNullOrEmpty(request.Type) ||
        //                x.Type == Enum.Parse<AppointmentType>(request.Type,true))
        //    )
        //{
        //    AddIncludes();
        //    ApplyOrderByDescending(x => x.Id);

        //    var pageIndexHelper = 0;

        //    if ((request.PageIndex - 1) < 0)
        //    {
        //        pageIndexHelper = 0;
        //    }
        //    else
        //    {
        //        pageIndexHelper = request.PageIndex - 1;

        //    }

        //    ApplyPagination(pageIndexHelper * request.PageSize, request.PageSize);
        //}

        //private void AddIncludes()
        //{
        //    Includes.Add(x => x.CreatedBy);
        //    Includes.Add(x => x.UpdatedBy);
        //    Includes.Add(x => x.Patient);
        //    Includes.Add(x => x.Doctor);
        //    Includes.Add(x => x.Patient.InsuranceCompany);
        //    Includes.Add(x => x.Patient.InsuranceCategory);
        //    Includes.Add(x=> x.Clinic);
        //}
    }
}
