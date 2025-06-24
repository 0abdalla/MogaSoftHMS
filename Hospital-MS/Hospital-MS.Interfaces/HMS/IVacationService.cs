using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IVacationService
    {
        List<EmployeeVacationDto> GetAllEmployeeVacationsData(PagingFilterModel SearchModel);
        List<EmployeeVacationDto> GetVacationsByEmployeeId(int employeeId, PagingFilterModel SearchModel);
        Task<ErrorResponseModel<string>> AddNewEmployeeVacation(int EmployeeId, EmployeeVacationDto model, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> EditVacation(int EmployeeId, EmployeeVacationDto model);
        Task<ErrorResponseModel<string>> DeleteVacation(int VacationId);
        Task<ErrorResponseModel<string>> ApproveEmployeeVacation(int VacationId, int EmployeeId, bool ApproveStatus);
        List<SelectorDataModel> GetVacationTypesSelector();
    }
}
