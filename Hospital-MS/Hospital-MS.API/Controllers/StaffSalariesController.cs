using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffSalariesController : ControllerBase
    {
        private readonly IStaffSalariesService _staffSalariesService;
        public StaffSalariesController(IStaffSalariesService staffSalariesService)
        {
            _staffSalariesService = staffSalariesService;
        }

        [HttpPost("CalculateStaffSalaries")]
        public async Task<List<StaffSalaryResponse>> CalculateStaffSalaries(List<StaffSalaryRequest> Model)
        {
            var results = await _staffSalariesService.CalculateStaffSalaries(Model);
            return results;
        }
    }
}
