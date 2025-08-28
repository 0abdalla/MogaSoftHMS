
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Rooms;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IRoomService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<RoomResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<RoomResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateAsync(int id, CreateRoomRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
