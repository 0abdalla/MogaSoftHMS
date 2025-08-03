using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Models.HR;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [HttpGet("CalculateStaffSalaries")]
        public async Task<ErrorResponseModel<List<EmployeeSalary>>> CalculateStaffSalaries(DateTime Date, CancellationToken cancellationToken)
        {
            var results = await _staffSalariesService.CalculateStaffSalaries(Date, cancellationToken);
            return results;
        }

        [HttpPost("AddStaffSalaries")]
        public async Task<ErrorResponseModel<bool>> AddStaffSalaries(List<EmployeeSalary> Salaries, CancellationToken cancellationToken)
        {
            var results = await _staffSalariesService.AddStaffSalaries(Salaries, cancellationToken);
            return results;
        }

        [HttpPost("GetAllStaffSalaries")]
        public async Task<PagedResponseModel<DataTable>> GetAllStaffSalariesAsync(PagingFilterModel filter)
        {
            var results = await _staffSalariesService.GetAllStaffSalariesAsync(filter);
            return results;
        }
    }
}
