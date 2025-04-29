using Hospital_MS.Core.Contracts.Departments;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class DepartmentsController(IDepartmentService departmentService) : ApiBaseController
    {
        private readonly IDepartmentService _departmentService = departmentService;

        [HttpPost("")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var result = await _departmentService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
        {
            var result = await _departmentService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
