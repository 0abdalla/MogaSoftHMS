using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.AccountTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IAccountTreeService
    {
        Task<ErrorResponseModel<string>> AddNewAccount(AccountTreeModel Model, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> EditAccountTree(int AccountId, AccountTreeModel Model, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> DeleteAccountTree(int AccountId, CancellationToken cancellationToken = default);
        List<AccountTreeModel> GetAccountTreeHierarchicalData(string SearchText);
        string GenerateAccountNumber(int? parentAccountId);
        List<SelectorDataModel> GetAccountTypes();
        List<SelectorDataModel> GetCurrencySelector();
        List<SelectorDataModel> GetAccountsSelector(bool? IsGroup);
        List<SelectorDataModel> GetCostCenterSelector(bool IsParent);
        List<AccountTreeModel> GetAccountTreeData(string SearchText);
    }
}
