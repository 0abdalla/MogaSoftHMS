using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MainGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS;
public interface IMainGroupService
{
    Task<ErrorResponseModel<string>> CreateAsync(MainGroupRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> UpdateAsync(int id, MainGroupRequest request, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorResponseModel<MainGroupResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponseModel<List<MainGroupResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default);
}
