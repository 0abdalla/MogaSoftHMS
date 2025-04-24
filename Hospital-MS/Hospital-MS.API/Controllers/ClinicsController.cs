using Hospital_MS.Core.Contracts.Clinics;
using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class ClinicsController(IClinicService clinicService) : ApiBaseController
    {
        private readonly IClinicService _clinicService = clinicService;

        [HttpPost("")]
        public async Task<IActionResult> CreateClinic([FromBody] CreateClinicRequest request, CancellationToken cancellationToken)
        {
            var result = await _clinicService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllClinics(CancellationToken cancellationToken)
        {
            var result = await _clinicService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
