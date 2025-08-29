using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.CostCenterTree;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.Finance;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hospital_MS.Services.Finance
{
    public class CostCenterTreeService : ICostCenterTreeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CostCenterTreeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CostCenterTree>> GetCostCenterTreeDataBySearch(string? SearchText)
        {
            if (string.IsNullOrEmpty(SearchText))
                return await _unitOfWork.Repository<CostCenterTree>().GetAll().ToListAsync();
            else
                return await _unitOfWork.Repository<CostCenterTree>().GetAll(i => i.NameAR.Contains(SearchText)).ToListAsync();
        }

        public List<CostCenterTreeModel> GetCostCenterTreeData(string SearchText)
        {

            return _unitOfWork.Repository<CostCenterTree>().GetAll().Select(x =>
                                        new CostCenterTreeModel
                                        {
                                            CostCenterId = x.CostCenterId,
                                            CostCenterNumber = x.CostCenterNumber,
                                            NameEN = x.NameEN,
                                            NameAR = x.NameAR,
                                            ParentId = x.ParentId,
                                            CostLevel = x.CostLevel,
                                            IsActive = x.IsActive,
                                            IsLocked = x.IsLocked,
                                            IsParent = x.IsParent,
                                            IsExpences = x.IsExpences,
                                            IsPost = x.IsPost,
                                            IsGroup = x.IsGroup,
                                            DisplayOrder = x.DisplayOrder,
                                            IsSelected = x.NameEN.Contains(SearchText) || x.NameEN.Contains(SearchText) || x.CostCenterNumber == SearchText

                                        }).ToList();
        }

        public List<CostCenterTreeModel> GetCostCenterTreeHierarchicalData(string SearchText)
        {
            var lst = GetCostCenterTreeData(SearchText);
            var Tree = BuildTree(lst);
            return Tree;
        }

        static List<CostCenterTreeModel> BuildTree(List<CostCenterTreeModel> costCenterList)
        {
            var costCenterById = costCenterList.ToDictionary(costCenter => costCenter.CostCenterId);

            var roots = new List<CostCenterTreeModel>();

            foreach (var costCenter in costCenterList)
            {
                if (costCenter.ParentId == 0 || costCenter.ParentId is null)
                {
                    costCenter.CostLevel = 1;
                    roots.Add(costCenter);
                }

                if (costCenter.ParentId > 0 && costCenterById.TryGetValue(costCenter.ParentId, out var parentCostCenter))
                {
                    costCenter.CostLevel = parentCostCenter.CostLevel + 1;
                    if (costCenter.IsSelected)
                    {
                        UpdateParentSelection(parentCostCenter, costCenterById);
                    }
                    parentCostCenter.Children.Add(costCenter);
                }
            }

            return roots;
        }

        public static void UpdateParentSelection(CostCenterTreeModel costCenter, Dictionary<int?, CostCenterTreeModel> costCenterList)
        {
            costCenter.IsSelected = true;
            if (costCenter.ParentId >= 0 && costCenterList.TryGetValue(costCenter.ParentId, out var parentCostCenter))
            {
                if (!parentCostCenter.IsSelected && parentCostCenter.ParentId < costCenter.ParentId)
                    UpdateParentSelection(parentCostCenter, costCenterList);
            }
        }

        public async Task<ErrorResponseModel<string>> CreateNewCostCenter(CostCenterTreeModel Model, CancellationToken cancellationToken = default)
        {
            try
            {
                CostCenterTree tbl = new CostCenterTree();
                var parent = _unitOfWork.Repository<CostCenterTree>().GetAll(x => x.CostCenterId == Model.ParentId).FirstOrDefault();

                tbl.CreatedDate = DateTime.Now;
                tbl.CreatedBy = string.Empty;
                tbl.CostCenterNumber = GenerateCostCenterNumber(Model.ParentId);
                tbl.ParentId = Model.ParentId;
                tbl.CostLevel = parent != null ? parent.CostLevel + 1 : 1;
                tbl.NameAR = Model.NameAR;
                tbl.NameEN = Model.NameAR;
                tbl.IsActive = Model.IsActive;
                tbl.IsLocked = Model.IsLocked;
                tbl.IsParent = tbl.CostLevel == 1 ? true : false;
                tbl.IsPost = Model.IsPost;
                tbl.IsGroup = Model.IsGroup;
                tbl.DisplayOrder = Model.DisplayOrder;

                await _unitOfWork.Repository<CostCenterTree>().AddAsync(tbl, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);


                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
        public string GenerateCostCenterNumber(int? parentId)
        {
            string newCostCenterNumber;
            if (parentId == null)
            {
                var maxParentNumber = _unitOfWork.Repository<CostCenterTree>().GetAll(a => a.ParentId == null || a.ParentId == 0).Max(a => (int?)Convert.ToInt32(a.CostCenterNumber)) ?? 0;
                newCostCenterNumber = (maxParentNumber + 1).ToString();
            }
            else
            {
                var parent = _unitOfWork.Repository<CostCenterTree>().GetAll(acc => acc.CostCenterId == parentId).FirstOrDefault();
                if (parent == null) throw new Exception($"Parent (ID: {parentId}) not found");

                var maxChildNumber = _unitOfWork.Repository<CostCenterTree>().GetAll(a => a.ParentId == parentId)
                    .Max(a => (int?)Convert.ToInt32(a.CostCenterNumber.Substring(parent.CostCenterNumber.Length))) ?? 0;
                newCostCenterNumber = $"{parent.CostCenterNumber}{(maxChildNumber + 1):D2}";
            }

            return newCostCenterNumber;
        }
        public async Task<ErrorResponseModel<string>> UpdateCostCenterTree(int CostCenterId, CostCenterTreeModel Model, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = _unitOfWork.Repository<CostCenterTree>().GetAll(x => x.CostCenterId == CostCenterId).FirstOrDefault();

                if (entity != null)
                {
                    var parent = _unitOfWork.Repository<CostCenterTree>().GetAll(x => x.CostCenterId == Model.ParentId).FirstOrDefault();


                    entity.ModifiedDate = DateTime.Now;
                    entity.CreatedBy = string.Empty;
                    entity.ParentId = Model.ParentId;
                    entity.CostLevel = parent != null ? parent.CostLevel + 1 : 1;
                    entity.NameAR = Model.NameAR;
                    entity.NameEN = Model.NameAR;
                    entity.IsActive = Model.IsActive;
                    entity.IsLocked = Model.IsLocked;
                    entity.IsParent = entity.CostLevel == 1 ? true : false;
                    entity.IsPost = Model.IsPost;
                    entity.IsGroup = Model.IsGroup;
                    entity.DisplayOrder = Model.DisplayOrder;

                    _unitOfWork.Repository<CostCenterTree>().Update(entity);
                    await _unitOfWork.CompleteAsync(cancellationToken);

                    return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
                }
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);

            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
        public async Task<ErrorResponseModel<string>> DeleteCostCenterTree(int CostCenterId, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = _unitOfWork.Repository<CostCenterTree>().GetAll(x => x.CostCenterId == CostCenterId).FirstOrDefault();

                if (entity != null)
                {
                    _unitOfWork.Repository<CostCenterTree>().Delete(entity);
                    await _unitOfWork.CompleteAsync(cancellationToken);

                    return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
                }
                else
                    return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);

            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
