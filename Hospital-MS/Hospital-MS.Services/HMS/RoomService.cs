using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Rooms;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                if (!Enum.TryParse<RoomType>(request.Type, true, out var roomStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                var room = new Room
                {
                    Number = request.Number,
                    WardId = request.WardId,
                    DailyPrice = request.DailyPrice
                    
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

        public async Task<ErrorResponseModel<List<RoomResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var rooms = await _unitOfWork.Repository<Room>().GetAll().Include(x => x.Ward).ToListAsync(cancellationToken);

            var response = rooms.Select(room => new RoomResponse
            {
                Id = room.Id,
                Number = room.Number,
                Type = room.Type.ToString(),
                Status = room.Status.ToString(),
                WardId = room.WardId,
                WardNumber = room.Ward.Number,
                DailyPrice = room.DailyPrice,
                

            }).ToList();

            return ErrorResponseModel<List<RoomResponse>>.Success(GenericErrors.GetSuccess, response);
        }
    }
}
