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
    public class DoctorSpecification : BaseSpecification<Doctor>
    {
        public DoctorSpecification(int id)
            : base(x => x.Id == id)
        {
            AddIncludes();
        }

        public DoctorSpecification(GetDoctorsRequest request)
            : base(x =>
                    (string.IsNullOrEmpty(request.Search) ||
                    x.FullName.ToLower().Contains(request.Search) ||
                    x.Phone.ToLower().Contains(request.Search)) &&
                    (string.IsNullOrEmpty(request.Status) ||
                    x.Status == Enum.Parse<StaffStatus>(request.Status)
            ))
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

        public DoctorSpecification()
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(d => d.Department);
            Includes.Add(d => d.Specialty);
            Includes.Add(d => d.CreatedBy);
            Includes.Add(d => d.UpdatedBy);
            Includes.Add(d => d.Schedules);
            Includes.Add(d => d.Ratings);
        }
    }
}
