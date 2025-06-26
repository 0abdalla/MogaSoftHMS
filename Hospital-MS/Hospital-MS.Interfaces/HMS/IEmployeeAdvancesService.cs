using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IEmployeeAdvancesService
    {
        List<EmployeeAdvanceModel> GetAdvancesByEmployeeId(PagingFilterModel SearchModel, int EmployeeId);
        Task<ErrorResponseModel<string>> AddNewEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> EditEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model);
        Task<ErrorResponseModel<string>> ApproveEmployeeAdvance(int EmployeeAdvanceId, bool IsApproved);
        Task<ErrorResponseModel<string>> DeleteEmployeeAdvance(int EmployeeAdvanceId);
        List<SelectorDataModel> GetAdvanceTypesSelector();
    }
}
