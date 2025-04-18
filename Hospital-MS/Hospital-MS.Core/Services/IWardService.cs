using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Wards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IWardService
    {
        Task<Result> CreateAsync(CreateWardRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<WardResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    }
}
