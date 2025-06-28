using Hospital_MS.Core.Common;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ControllerBase
    {
        private readonly IVacationService _vacationService;
        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [HttpPost]
        [Route("GetAllEmployeeVacationsData")]
        public IActionResult GetAllEmployeeVacationsData(PagingFilterModel Model)
        {
            var data = _vacationService.GetAllEmployeeVacationsData(Model);
            var result = new PagedResponseModel<List<EmployeeVacationDto>>
            {
                Results = data,
                TotalCount = data.FirstOrDefault()?.TotalCount ?? 0,
            };
            return Ok(result);
        }

        [HttpPost]
        [Route("GetVacationsByEmployeeId")]
        public IActionResult GetVacationsByEmployeeId(int EmployeeId, PagingFilterModel Model)
        {
            var data = _vacationService.GetVacationsByEmployeeId(EmployeeId, Model);
            var result = new PagedResponseModel<List<EmployeeVacationDto>>
            {
                Results = data,
                TotalCount = data.FirstOrDefault()?.TotalCount ?? 0,
            };
            return Ok(result);
        }
        [HttpPost]
        [Route("AddNewEmployeeVacation")]
        public async Task<IActionResult> AddNewEmployeeVacation(int EmployeeId, EmployeeVacationDto model)
        {
            var result = await _vacationService.AddNewEmployeeVacation(EmployeeId, model);
            return Ok(result);
        }

        [HttpPost]
        [Route("EditEmployeeVacation")]
        public async Task<IActionResult> EditVacation(int EmployeeId, EmployeeVacationDto model)
        {
            var result = await _vacationService.EditVacation(EmployeeId, model);

            return Ok(result);
        }

        [HttpGet]
        [Route("DeleteVacation")]
        public async Task<IActionResult> DeleteVacation(int VacationId)
        {
            var result = await _vacationService.DeleteVacation(VacationId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetVacationTypesSelector")]
        public IActionResult GetVacationTypesSelector()
        {
            var result = _vacationService.GetVacationTypesSelector();
            return Ok(result);
        }
    }
}
