using Hospital_MS.Core.Contracts.Doctors;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.Doctors
{
    public class DoctorsCountSpecification : BaseSpecification<Doctor>
    {

        public DoctorsCountSpecification(GetDoctorsRequest request)
            : base(x =>
                    (string.IsNullOrEmpty(request.Search) ||
                    x.FullName.ToLower().Contains(request.Search) ||
                    x.Phone.ToLower().Contains(request.Search)) &&
                    (string.IsNullOrEmpty(request.Status) ||
                    x.Status == Enum.Parse<StaffStatus>(request.Status)
            ))
        { }

    }
}
