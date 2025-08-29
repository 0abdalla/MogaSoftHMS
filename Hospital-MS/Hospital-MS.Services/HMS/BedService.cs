using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Beds;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS
{
    public class BedService(IUnitOfWork unitOfWork) : IBedService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateBedRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Enum.TryParse<BedStatus>(request.Status, true, out var bedStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

                var bed = new Bed
                {
                    Number = request.Number,
                    RoomId = request.RoomId,
                    DailyPrice = request.DailyPrice,


                };

                await _unitOfWork.Repository<Bed>().AddAsync(bed, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);

            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var bed = await _unitOfWork.Repository<Bed>()
                    .GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (bed == null)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                _unitOfWork.Repository<Bed>().Delete(bed);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<List<BedResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var beds = await _unitOfWork.Repository<Bed>()
                .GetAll()
                .Include(i => i.Room)
                .OrderByDescending(i => i.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            var result = beds.Select(x => new BedResponse
            {
                Id = x.Id,
                Number = x.Number,
                RoomId = x.RoomId,
                Status = x.Status.ToString(),
                RoomNumber = x.Room.Number,

            }).ToList();

            return ErrorResponseModel<List<BedResponse>>.Success(GenericErrors.GetSuccess, result);
        }

        public async Task<ErrorResponseModel<BedResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var bed = await _unitOfWork.Repository<Bed>()
                .GetAll()
                .Include(i => i.Room)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            if (bed == null)
                return ErrorResponseModel<BedResponse>.Failure(GenericErrors.NotFound);
            var result = new BedResponse
            {
                Id = bed.Id,
                Number = bed.Number,
                RoomId = bed.RoomId,
                Status = bed.Status.ToString(),
                RoomNumber = bed.Room.Number,
            };
            return ErrorResponseModel<BedResponse>.Success(GenericErrors.GetSuccess, result);
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateBedRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Enum.TryParse<BedStatus>(request.Status, true, out var bedStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);
                var bed = await _unitOfWork.Repository<Bed>()
                    .GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (bed == null)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                bed.Number = request.Number;
                bed.RoomId = request.RoomId;
                bed.Status = bedStatus;
                _unitOfWork.Repository<Bed>().Update(bed);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
