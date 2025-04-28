using Hospital_MS.Core.Contracts.Rooms;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class RoomsController(IRoomService roomService) : ApiBaseController
    {
        private readonly IRoomService _roomService = roomService;

        [HttpPost("")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetRooms(CancellationToken cancellationToken)
        {
            var result = await _roomService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

    }
}
