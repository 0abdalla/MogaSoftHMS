using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.CostCenterTree;
using Hospital_MS.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.Finance
{
    public interface ICostCenterTreeService
    {
        Task<ErrorResponseModel<string>> CreateNewCostCenter(CostCenterTreeModel Model, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateCostCenterTree(int CostCenterId, CostCenterTreeModel Model, CancellationToken cancellationToken = default);
        string GenerateCostCenterNumber(int? ParentCostCenterId);
        Task<ErrorResponseModel<string>> DeleteCostCenterTree(int CostCenterId, CancellationToken cancellationToken = default);
        Task<List<CostCenterTree>> GetCostCenterTreeData(bool IsParent);
        List<CostCenterTreeModel> GetCostCenterTreeHierarchicalData(string SearchText);
    }
}
