using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Hospital_MS.Services.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class AccountTreeService : IAccountTreeService
    {
        private readonly ISQLHelper _sQLHelper;
        private readonly IUnitOfWork _unitOfWork;
        public AccountTreeService(ISQLHelper sQLHelper, IUnitOfWork unitOfWork)
        {
            _sQLHelper = sQLHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorResponseModel<string>> AddNewAccount(AccountTreeModel Model, CancellationToken cancellationToken = default)
        {
            try
            {
                var parentAccount = await _unitOfWork.Repository<AccountTree>().GetByIdAsync(Model.ParentAccountId.Value);
                if (parentAccount is not null && !parentAccount.IsParent.Value)
                {
                    parentAccount.IsParent = true;
                }
                AccountTree tbl = new AccountTree();

                tbl.AccountNumber = GenerateAccountNumber(Model.ParentAccountId);
                tbl.ParentAccountId = Model.ParentAccountId;
                tbl.AccountTypeId = Model.AccountTypeId;
                tbl.AccountLevel = parentAccount != null ? parentAccount.AccountLevel + 1 : 1;
                tbl.IsParent = parentAccount != null ? false : true;
                tbl.AccountNature = string.Empty;
                tbl.IsActive = Model.IsActive;
                tbl.IsGroup = Model.IsGroup;
                tbl.NameAR = Model.NameAR;
                tbl.NameEN = Model.NameEN;
                tbl.IsDisToCostCenter = Model.IsDisToCostCenter;
                tbl.CostCenterId = Model.CostCenterId;
                tbl.CreatedDate = DateTime.Now;
                tbl.CreatedBy = Model.CreatedBy;

                await _unitOfWork.Repository<AccountTree>().AddAsync(tbl, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> EditAccountTree(int AccountId, AccountTreeModel Model, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _unitOfWork.Repository<AccountTree>().GetByIdAsync(AccountId);

                if (entity != null)
                {
                    var parentAccount = _unitOfWork.Repository<AccountTree>().GetAll(x => x.AccountId == Model.ParentAccountId).FirstOrDefault();
                    if (parentAccount is not null && !parentAccount.IsParent.Value)
                        parentAccount.IsParent = true;

                    entity.ParentAccountId = Model.ParentAccountId;
                    entity.AccountTypeId = Model.AccountTypeId;
                    entity.AccountLevel = parentAccount != null ? parentAccount.AccountLevel + 1 : 1;
                    entity.IsParent = parentAccount != null ? false : true;
                    entity.AccountNature = string.Empty;
                    entity.IsActive = Model.IsActive;
                    entity.IsGroup = Model.IsGroup;
                    entity.NameAR = Model.NameAR;
                    entity.NameEN = Model.NameEN;
                    entity.IsDisToCostCenter = Model.IsDisToCostCenter;
                    entity.CostCenterId = Model.CostCenterId;
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = Model.ModifiedBy;

                    _unitOfWork.Repository<AccountTree>().Update(entity);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }

                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> DeleteAccountTree(int AccountId, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _unitOfWork.Repository<AccountTree>().GetByIdAsync(AccountId);

                if (entity != null)
                {

                    var childAccounts = _unitOfWork.Repository<AccountTree>().GetAll(x => x.ParentAccountId == AccountId);

                    if (childAccounts.Any())
                    {
                        foreach (var acc in childAccounts)
                        {
                            acc.ParentAccountId = entity.ParentAccountId;
                            acc.AccountLevel = entity.AccountLevel;
                            acc.IsParent = entity.IsParent;
                        }
                    }
                    _unitOfWork.Repository<AccountTree>().Delete(entity);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }

                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public List<AccountTreeModel> GetAccountTreeHierarchicalData(string SearchText)
        {
            var lst = GetAccountTreeData(SearchText);
            var Tree = BuildTree(lst);
            return Tree;

        }

        public string GenerateAccountNumber(int? parentAccountId)
        {
            string newAccountNumber;

            if (parentAccountId == null)
            {
                var maxParentNumber = _unitOfWork.Repository<AccountTree>().GetAll(a => a.ParentAccountId == null || a.ParentAccountId == 0).Max(a => (int?)Convert.ToInt32(a.AccountNumber)) ?? 0;
                newAccountNumber = (maxParentNumber + 1).ToString();
            }
            else
            {
                var parent = _unitOfWork.Repository<AccountTree>().GetByIdAsync(parentAccountId.Value).GetAwaiter().GetResult();
                if (parent == null) throw new Exception($"Parent account (ID: {parentAccountId}) not found");

                var maxChildNumber = _unitOfWork.Repository<AccountTree>().GetAll(a => a.ParentAccountId == parentAccountId)
                    .Max(a => (int?)Convert.ToInt32(a.AccountNumber.Substring(parent.AccountNumber.Length))) ?? 0;

                newAccountNumber = $"{parent.AccountNumber}{(maxChildNumber + 1):D2}";
            }

            return newAccountNumber;
        }

        public List<SelectorDataModel> GetAccountTypes()
        {
            var result = _sQLHelper.SQLQuery<SelectorDataModel>("[Finance].[SP_GetAccountTypes]", Array.Empty<SqlParameter>());
            return result;
        }

        public List<SelectorDataModel> GetCurrencySelector()
        {
            var result = _sQLHelper.SQLQuery<SelectorDataModel>("[Finance].[SP_GetCurrencies]", Array.Empty<SqlParameter>());
            return result;
        }

        public List<SelectorDataModel> GetAccountsSelector(bool? IsGroup)
        {
            var result = _unitOfWork.Repository<AccountTree>().GetAll(x => IsGroup == null || x.IsGroup == IsGroup).Select(a => new SelectorDataModel
            {
                Id = a.AccountId,
                Name = a.NameAR,
                Code = a.AccountNumber
            }).ToList();

            return result;
        }

        public List<SelectorDataModel> GetCostCenterSelector(bool IsParent)
        {
            var result = _unitOfWork.Repository<CostCenterTree>().GetAll(x => x.IsParent == IsParent).Select(a => new SelectorDataModel
            {
                Id = a.CostCenterId,
                Name = a.NameAR,
                Code = a.CostCenterNumber
            }).ToList();

            return result;
        }

        public List<AccountTreeModel> GetAccountTreeData(string SearchText)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@SearchText", SearchText);

            var lst = _sQLHelper.SQLQuery<AccountTreeModel>("[Finance].[SP_GetAccountTreeData]", param);
            return lst;
        }

        public List<AccountTreeModel> BuildTree(List<AccountTreeModel> accList)
        {
            var accsById = accList.ToDictionary(acc => acc.AccountId);
            var roots = new List<AccountTreeModel>();

            foreach (var acc in accList)
            {
                if (acc.ParentAccountId == 0 || acc.ParentAccountId is null)
                {
                    acc.AccountLevel = 1;
                    roots.Add(acc);
                }

                if (acc.ParentAccountId > 0 && accsById.TryGetValue(acc.ParentAccountId, out var parentAcc))
                {
                    acc.AccountLevel = parentAcc.AccountLevel + 1;
                    if (acc.IsSelected.GetValueOrDefault(false))
                    {

                        UpdateParentSelection(parentAcc, accsById);
                    }

                    parentAcc.Children.Add(acc);
                }
            }

            return roots;
        }

        public static void UpdateParentSelection(AccountTreeModel acc, Dictionary<int?, AccountTreeModel> accounts)
        {
            acc.IsSelected = true;
            if (acc.ParentAccountId >= 0 && accounts.TryGetValue(acc.ParentAccountId, out var parentAcc))
            {
                if (!parentAcc.IsSelected.GetValueOrDefault(false) && parentAcc.ParentAccountId < acc.ParentAccountId)
                    UpdateParentSelection(parentAcc, accounts);
            }
        }
    }
}
