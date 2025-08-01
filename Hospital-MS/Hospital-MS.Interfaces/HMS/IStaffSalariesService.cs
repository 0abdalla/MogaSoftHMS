using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Models.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IStaffSalariesService
    {
        Task<ErrorResponseModel<List<EmployeeSalary>>> CalculateStaffSalaries(DateTime Date, CancellationToken cancellationToken);
        Task<ErrorResponseModel<bool>> AddStaffSalaries(List<EmployeeSalary> Salaries, CancellationToken cancellationToken);
        Task<PagedResponseModel<DataTable>> GetAllStaffSalariesAsync(PagingFilterModel filter);
    }
}
