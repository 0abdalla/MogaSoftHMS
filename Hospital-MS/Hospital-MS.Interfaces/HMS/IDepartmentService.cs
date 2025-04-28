using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IDepartmentService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
