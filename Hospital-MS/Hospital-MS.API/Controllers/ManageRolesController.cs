using Hospital_MS.Core.Common;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageRolesController : ControllerBase
    {
        private readonly IManageRolesService _manageRolesService;
        public ManageRolesController(IManageRolesService manageRolesService)
        {
            _manageRolesService = manageRolesService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _manageRolesService.GetAllRoles();
            return Ok(result);
        }

        [HttpGet("GetPagesByRoleId")]
        public IActionResult GetPagesByRoleId(Guid RoleId)
        {
            var result = _manageRolesService.GetPagesByRoleId(RoleId);
            return Ok(result);
        }

        [HttpGet("GetAllPages")]
        public IActionResult GetAllPages()
        {
            var result = _manageRolesService.GetAllPages();
            return Ok(result);
        }

        [HttpPost("AssignRoleToPages")]
        public async Task<IActionResult> AssignRoleToPages(AssignRole Model, CancellationToken cancellationToken)
        {
            var result = await _manageRolesService.AssignRoleToPages(Model, cancellationToken);
            return Ok(result);
        }
    }
}
