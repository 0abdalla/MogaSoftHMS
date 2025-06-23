using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltyController : ControllerBase
    {
        private readonly IPenaltyService _penaltyService;

        public PenaltyController(IPenaltyService penaltyService)
        {
            _penaltyService = penaltyService;
        }


        [HttpGet]
        [Route("GetAllEmployeePenaltiesData")]
        public List<EmployeePenaltyDto> GetAllEmployeePenaltiesData(PagingFilterModel model)
        {
            return _penaltyService.GetAllEmployeePenaltiesData(model);
        }

        [HttpPost]
        [Route("GetPenaltiesByEmployeeId")]
        public IActionResult GetPenaltiesByEmployeeId(int EmployeeId, PagingFilterModel Model)
        {
            var data = _penaltyService.GetPenaltiesByEmployeeId(EmployeeId, Model);
            var result = new PagedResponseModel<List<EmployeePenaltyDto>>
            {
                Results = data,
                TotalCount = data.FirstOrDefault()?.TotalCount ?? 0,
            };
            return Ok(result);
        }
        [HttpPost]
        [Route("AddNewEmployeePenalty")]
        public async Task<IActionResult> AddNewEmployeePenalty(int EmployeeId, EmployeePenaltyDto model)
        {
            var result = await _penaltyService.AddNewEmployeePenalty(EmployeeId, model);
            return Ok(result);
        }

        [HttpPost]
        [Route("EditEmployeePenalty")]
        public async Task<IActionResult> EditEmployeePenalty(int EmployeeId, EmployeePenaltyDto model)
        {
            var result = await _penaltyService.EditEmployeePenalty(EmployeeId, model);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetPenaltyTypesSelector")]
        public async Task<IActionResult> GetPenaltyTypesSelector()
        {
            var result = await _penaltyService.GetPenaltyTypesSelector();
            return Ok(result);
        }

        [HttpGet]
        [Route("DeleteEmployeePenalty")]
        public async Task<IActionResult> DeleteEmployeePenalty(int PenaltyId)
        {
            var result = await _penaltyService.DeleteEmployeePenalty(PenaltyId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetActiveEmployeesSelector")]
        public async Task<IActionResult> GetActiveEmployeesSelector()
        {
            var result = await _penaltyService.GetActiveEmployeesSelector();
            return Ok(result);
        }
    }
}
