using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IRoomService
    {
        Task<Result> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<RoomResponse>>> GetAllAsync( CancellationToken cancellationToken = default);
    }
}
