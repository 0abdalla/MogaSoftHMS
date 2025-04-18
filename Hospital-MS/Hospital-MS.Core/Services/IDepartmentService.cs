using Hospital_MS.Core.Abstractions;
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
        Task<Result> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
