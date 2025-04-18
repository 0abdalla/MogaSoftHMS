using Hospital_MS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class InsurancesController(IInsuranceService insuranceService) : ApiBaseController
    {
        private readonly IInsuranceService _insuranceService = insuranceService;
    }
}
