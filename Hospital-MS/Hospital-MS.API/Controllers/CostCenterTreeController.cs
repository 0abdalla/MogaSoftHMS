using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.CostCenterTree;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Finance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostCenterTreeController : ControllerBase
    {
        private readonly ICostCenterTreeService _costCenterTreeService;

        public CostCenterTreeController(ICostCenterTreeService costCenterTreeService)
        {
            _costCenterTreeService = costCenterTreeService;
        }

        [HttpPost]
        [Route("CreateNewCostCenter")]

        public async Task<ErrorResponseModel<string>> CreateNewCostCenter(CostCenterTreeModel Model)
        {
            var results = await _costCenterTreeService.CreateNewCostCenter(Model);
            return results;
        }

        [HttpPost]
        [Route("UpdateCostCenterTree")]

        public async Task<ErrorResponseModel<string>> UpdateCostCenterTree(int CostCenterId, CostCenterTreeModel Model)
        {
            var results = await _costCenterTreeService.UpdateCostCenterTree(CostCenterId, Model);
            return results;
        }
        [HttpGet]
        [Route("GenerateCostCenterNumber")]
        public IActionResult GenerateCostCenterNumber(int? ParentCostCenterId)
        {
            int? id = ParentCostCenterId == 0 ? null : ParentCostCenterId;
            var results = _costCenterTreeService.GenerateCostCenterNumber(id);
            return Ok(results);
        }
        [HttpGet]
        [Route("DeleteCostCenterTree")]

        public async Task<ErrorResponseModel<string>> DeleteCostCenterTree(int CostCenterId)
        {
            var results = await _costCenterTreeService.DeleteCostCenterTree(CostCenterId);
            return results;
        }


        [HttpGet]
        [Route("GetCostCenterTreeHierarchicalData")]
        public IActionResult GetCostCenterTreeHierarchicalData(string? SearchText)
        {
            var results = _costCenterTreeService.GetCostCenterTreeHierarchicalData(SearchText);
            return Ok(results);
        }



        [HttpGet]
        [Route("GetCostCenterTreeData")]
        public async Task<List<CostCenterTree>> GetCostCenterTreeData(bool IsParent)
        {
            return await _costCenterTreeService.GetCostCenterTreeData(IsParent);
        }
    }
}
