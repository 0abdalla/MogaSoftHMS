using Hospital_MS.Core.Contracts.Rooms;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id, CancellationToken cancellationToken)
        {
            var result = await _roomService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] CreateRoomRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id, CancellationToken cancellationToken)
        {
            var result = await _roomService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
