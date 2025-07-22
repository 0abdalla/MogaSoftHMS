using Hospital_MS.Core.Contracts.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IStaffSalariesService
    {
        Task<List<StaffSalaryResponse>> CalculateStaffSalaries(List<StaffSalaryRequest> Model);
    }
}
