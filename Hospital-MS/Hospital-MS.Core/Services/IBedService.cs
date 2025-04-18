using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Beds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IBedService
    {
        Task<Result> CreateAsync(CreateBedRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<BedResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
