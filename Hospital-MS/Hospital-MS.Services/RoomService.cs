using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Rooms;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class RoomService(IUnitOfWork unitOfWork) : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken = default)
        {

            try
            {
                if (!Enum.TryParse<RoomType>(request.Type, true, out var roomType))
                    return Result.Failure(new Error("InvalidType", "Invalid room type provided.", 400));

                if (!Enum.TryParse<RoomType>(request.Type, true, out var roomStatus))
                    return Result.Failure(new Error("InvalidType", "Invalid room type provided.", 400));

                var room = new Room
                {
                    Number = request.Number,
                    WardId = request.WardId,
                };

                await _unitOfWork.Repository<Room>().AddAsync(room, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {

                return Result.Failure(new Error("CreateRoomError", ex.Message, 500));
            }

        }

        public async Task<Result<IReadOnlyList<RoomResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var spec = new RoomSpecification();

            var rooms = await _unitOfWork.Repository<Room>().GetAllWithSpecAsync(spec, cancellationToken);

            var response = rooms.Select(room => new RoomResponse
            {
                Id = room.Id,
                Number = room.Number,
                Type = room.Type.ToString(),
                Status = room.Status.ToString(),
                WardId = room.WardId,
                WardNumber = room.Ward.Number

            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<RoomResponse>>(response);
        }
    }
}
