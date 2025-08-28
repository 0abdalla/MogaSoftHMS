using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Rooms;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS
{
    public class RoomService(IUnitOfWork unitOfWork) : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken = default)
        {

            try
            {
                if (!Enum.TryParse<RoomType>(request.Type, true, out var roomType))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                if (!Enum.TryParse<RoomStatus>(request.Type, true, out var roomStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                var room = new Room
                {
                    Number = request.Number,
                    WardId = request.WardId,
                    DailyPrice = request.DailyPrice,
                    Type = roomType,
                    Status = roomStatus,
                };

                await _unitOfWork.Repository<Room>().AddAsync(room, cancellationToken);

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
                var room = await _unitOfWork.Repository<Room>()
                    .GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (room == null)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                _unitOfWork.Repository<Room>().Delete(room);
                await _unitOfWork.CompleteAsync(cancellationToken);
                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<List<RoomResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var rooms = await _unitOfWork.Repository<Room>()
                .GetAll().Include(x => x.Ward).ToListAsync(cancellationToken);

            var response = rooms.Select(room => new RoomResponse
            {
                Id = room.Id,
                Number = room.Number,
                Type = room.Type.ToString(),
                Status = room.Status.ToString(),
                WardId = room.WardId,
                WardName = room.Ward.Name,
                DailyPrice = room.DailyPrice,

            }).ToList();

            return ErrorResponseModel<List<RoomResponse>>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<RoomResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var room = await _unitOfWork.Repository<Room>()
                .GetAll().Include(x => x.Ward)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (room == null)
                return ErrorResponseModel<RoomResponse>.Failure(GenericErrors.NotFound);
            var response = new RoomResponse
            {
                Id = room.Id,
                Number = room.Number,
                Type = room.Type.ToString(),
                Status = room.Status.ToString(),
                WardId = room.WardId,
                WardName = room.Ward.Name,
                DailyPrice = room.DailyPrice,
            };
            return ErrorResponseModel<RoomResponse>.Success(GenericErrors.GetSuccess, response);

        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateRoomRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var room = await _unitOfWork.Repository<Room>()
                    .GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (room == null)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
                if (!Enum.TryParse<RoomType>(request.Type, true, out var roomType))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);
                if (!Enum.TryParse<RoomType>(request.Type, true, out var roomStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);
                room.Number = request.Number;
                room.WardId = request.WardId;
                room.DailyPrice = request.DailyPrice;
                _unitOfWork.Repository<Room>().Update(room);
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
