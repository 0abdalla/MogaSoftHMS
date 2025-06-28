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

        public IActionResult CreateNewCostCenter(CostCenterTreeModel Model)
        {
            var results = _costCenterTreeService.CreateNewCostCenter(Model);
            return Ok(results);
        }

        [HttpPost]
        [Route("UpdateCostCenterTree")]

        public IActionResult UpdateCostCenterTree(int CostCenterId, CostCenterTreeModel Model)
        {
            var results = _costCenterTreeService.UpdateCostCenterTree(CostCenterId, Model);
            return Ok(results);
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

        public IActionResult DeleteCostCenterTree(int CostCenterId)
        {
            var results = _costCenterTreeService.DeleteCostCenterTree(CostCenterId);
            return Ok(results);
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
