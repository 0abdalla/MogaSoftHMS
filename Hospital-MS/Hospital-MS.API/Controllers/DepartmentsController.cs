using Hospital_MS.Core.Contracts.Departments;
using Hospital_MS.Core.Services;
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
            return result.IsSuccess
                ? Created()
                : BadRequest(result.Error);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
        {
            var result = await _departmentService.GetAllAsync(cancellationToken);
            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }
    }
}
