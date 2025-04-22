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
    public class StaffSpecification : BaseSpecification<Staff>
    {
        public StaffSpecification(int id)
            : base(x => x.Id == id)
        {
            AddIncludes();
        }

        public StaffSpecification(GetStaffRequest request)
            : base(x =>
                    (string.IsNullOrEmpty(request.Search) ||
                    x.FullName.ToLower().Contains(request.Search) ||
                    x.PhoneNumber.ToLower().Contains(request.Search)) &&
                    (string.IsNullOrEmpty(request.Type) ||
                    x.Type == Enum.Parse<StaffType>(request.Type))
            )
        {
            AddIncludes();

            ApplyOrderByDescending(x => x.Id);

            var pageIndexHelper = 0;

            if ((request.PageIndex - 1) < 0)
            {
                pageIndexHelper = 0;
            }
            else
            {
                pageIndexHelper = request.PageIndex - 1;

            }

            ApplyPagination(pageIndexHelper * request.PageSize, request.PageSize);
        }

        private void AddIncludes()
        {
            Includes.Add(x => x.Clinic);
            Includes.Add(x => x.Department);
            Includes.Add(x => x.StaffAttachments);
        }
    }
}
