using Hospital_MS.Core.Contracts.Doctors;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Services;
using Hospital_MS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class DoctorsController(IDoctorService doctorService) : ApiBaseController
    {
        private readonly IDoctorService _doctorService = doctorService;

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] DoctorRequest request, CancellationToken cancellationToken)
        {
            var result = await _doctorService.CreateAsync(request, cancellationToken);

            return result.IsSuccess ? Created() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("all")]
        public async Task<ActionResult<Pagination<IReadOnlyList<AllDoctorsResponse>>>> GetAllDoctors([FromQuery] GetDoctorsRequest request, CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetAllAsync(request, cancellationToken);

            int count = await _doctorService.GetAllCountAsync(request, cancellationToken);

            return result.IsSuccess
                ? Ok(new Pagination<AllDoctorsResponse>(request.PageIndex, request.PageSize, result.Value, count))
                : NotFound(result.Error);
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetAllCount(CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetCountsAsync(cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] DoctorRequest request, CancellationToken cancellationToken)
        {
            var result = await _doctorService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        //{
        //    var result = await _doctorService.DeleteAsync(id, cancellationToken);
        //    return result.IsSuccess ? NoContent() : NotFound(result.Error);

        //}
    }
}
