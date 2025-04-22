using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.Staffs
{
    public class StaffCountSpecification : BaseSpecification<Staff>
    {
        public StaffCountSpecification(GetStaffRequest request)
            : base(x =>
                    (string.IsNullOrEmpty(request.Search) ||
                    x.FullName.ToLower().Contains(request.Search) ||
                    x.PhoneNumber.ToLower().Contains(request.Search)) &&
                    (string.IsNullOrEmpty(request.Type) ||
                    x.Type == Enum.Parse<StaffType>(request.Type))
            )
        { }
    }
}
