using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Beds;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.Beds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class BedService(IUnitOfWork unitOfWork) : IBedService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateBedRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Enum.TryParse<BedStatus>(request.Status, true, out var bedStatus))
                    return Result.Failure(new Error("InvalidStatus", "Invalid bed status provided.", 400));

                var bed = new Bed
                {
                    Number = request.Number,
                    RoomId = request.RoomId,
                };

                await _unitOfWork.Repository<Bed>().AddAsync(bed, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();

            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<Bed>.FailedToAdd);
            }

        }

        public async Task<Result<IReadOnlyList<BedResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var spec = new BedSpecification();

            var beds = await _unitOfWork.Repository<Bed>().GetAllWithSpecAsync(spec, cancellationToken);

            var result = beds.Select(x => new BedResponse
            {
                Id = x.Id,
                Number = x.Number,
                RoomId = x.RoomId,
                Status = x.Status.ToString(),
                RoomNumber = x.Room.Number,

            }).ToList();

            return Result.Success<IReadOnlyList<BedResponse>>(result);
        }
    }
}
