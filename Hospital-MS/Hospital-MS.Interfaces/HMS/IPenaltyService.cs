using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using Hospital_MS.Core.Models.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IPenaltyService
    {
        List<EmployeePenaltyDto> GetAllEmployeePenaltiesData(PagingFilterModel SearchModel);
        List<EmployeePenaltyDto> GetPenaltiesByEmployeeId(int EmployeeId, PagingFilterModel SearchModel);
        Task<ErrorResponseModel<string>> AddNewEmployeePenalty(int EmployeeId, EmployeePenaltyDto model, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> EditEmployeePenalty(int EmployeeId, EmployeePenaltyDto model, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteEmployeePenalty(int PenaltyId);
        Task<ContractDetail> GetEmployeeContractDetails(int EmployeeId);
        Task<List<SelectorDataModel>> GetActiveEmployeesSelector();
        Task<List<SelectorDataModel>> GetPenaltyTypesSelector();
    }
}
