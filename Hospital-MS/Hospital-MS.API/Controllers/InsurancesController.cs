using Hospital_MS.Core.Contracts.Insurances;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class InsurancesController(IInsuranceCompanyService insuranceService) : ApiBaseController
    {
        private readonly IInsuranceCompanyService _insuranceService = insuranceService;

        [HttpPost("company")]
        public async Task<IActionResult> CreateInsuranceCompany([FromBody] InsuranceRequest request, CancellationToken cancellationToken)
        {
            var result = await _insuranceService.CreateAsync(request, cancellationToken);

            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetInsuranceCompanies(CancellationToken cancellationToken)
        {
            var result = await _insuranceService.GetAllAsync(cancellationToken);

            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetInsuranceCompanyById(int id, CancellationToken cancellationToken)
        {
            var result = await _insuranceService.GetByIdAsync(id, cancellationToken);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        [HttpPut("company/{id}")]
        public async Task<IActionResult> UpdateInsuranceCompany(int id, [FromBody] InsuranceRequest request, CancellationToken cancellationToken)
        {
            var result = await _insuranceService.UpdateAsync(id, request, cancellationToken);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("company/{id}")]
        public async Task<IActionResult> DeleteInsuranceCompany(int id, CancellationToken cancellationToken)
        {
            var result = await _insuranceService.DeleteAsync(id, cancellationToken);
            return !result.IsSuccess ? BadRequest(result) : Ok(result);

        }
    }
}
