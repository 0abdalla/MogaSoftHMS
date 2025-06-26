using Hospital_MS.Core.Common;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAdvancesController : ControllerBase
    {
        private readonly IEmployeeAdvancesService _employeeAdvancesService;
        public EmployeeAdvancesController(IEmployeeAdvancesService employeeAdvancesService)
        {
            _employeeAdvancesService = employeeAdvancesService;
        }

        [HttpPost]
        [Route("GetAdvancesByEmployeeId")]
        public IActionResult GetAdvancesByEmployeeId(int EmployeeId, PagingFilterModel SearchModel)
        {
            var data = _employeeAdvancesService.GetAdvancesByEmployeeId(SearchModel, EmployeeId);
            var result = new PagedResponseModel<List<EmployeeAdvanceModel>>
            {
                Results = data,
                TotalCount = data.FirstOrDefault()?.TotalCount ?? 0,
            };
            return Ok(result);
        }

        [HttpPost]
        [Route("AddNewEmployeeAdvance")]
        public async Task<IActionResult> AddNewEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model)
        {
            var result = await _employeeAdvancesService.AddNewEmployeeAdvance(EmployeeId, model);
            return Ok(result);
        }

        [HttpPost]
        [Route("EditEmployeeAdvance")]
        public async Task<IActionResult> EditEmployeeAdvance(int EmployeeId, EmployeeAdvanceModel model)
        {
            var result = await _employeeAdvancesService.EditEmployeeAdvance(EmployeeId, model);

            return Ok(result);
        }

        [HttpGet]
        [Route("ApproveEmployeeAdvance")]
        public async Task<IActionResult> ApproveEmployeeAdvance(int EmployeeAdvanceId, bool IsApproved)
        {
            var result = await _employeeAdvancesService.ApproveEmployeeAdvance(EmployeeAdvanceId, IsApproved);
            return Ok(result);
        }

        [HttpGet]
        [Route("DeleteEmployeeAdvance")]
        public async Task<IActionResult> DeleteEmployeeAdvance(int EmployeeAdvanceId)
        {
            var result = await _employeeAdvancesService.DeleteEmployeeAdvance(EmployeeAdvanceId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAdvanceTypesSelector")]
        public IActionResult GetAdvanceTypesSelector()
        {
            var result = _employeeAdvancesService.GetAdvanceTypesSelector();
            return Ok(result);
        }
    }
}
