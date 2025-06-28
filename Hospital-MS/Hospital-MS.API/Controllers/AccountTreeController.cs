using Hospital_MS.Core.Contracts.AccountTree;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTreeController : ControllerBase
    {
        private readonly IAccountTreeService _accountTreeService;
        public AccountTreeController(IAccountTreeService accountTreeService)
        {
            _accountTreeService = accountTreeService;
        }

        [HttpPost]
        [Route("AddNewAccount")]
        public IActionResult AddNewAccount(AccountTreeModel Model)
        {
            var results = _accountTreeService.AddNewAccount(Model);
            return Ok(results);
        }
        [HttpGet]
        [Route("GenerateAccountNumber")]

        public IActionResult GenerateAccountNumber(int? ParentAccountId)
        {
            int? id = ParentAccountId == 0 ? null : ParentAccountId;
            var results = _accountTreeService.GenerateAccountNumber(id);
            return Ok(results);
        }

        [HttpPost]
        [Route("EditAccountTree")]

        public IActionResult EditAccountTree(int AccountId, AccountTreeModel Model)
        {
            var results = _accountTreeService.EditAccountTree(AccountId, Model);
            return Ok(results);
        }
        [HttpGet]
        [Route("DeleteAccountTree")]

        public IActionResult DeleteAccountTree(int AccountId)
        {
            var results = _accountTreeService.DeleteAccountTree(AccountId);
            return Ok(results);
        }

        [HttpGet]
        [Route("GetAccountTreeData")]
        public IActionResult GetAccountTreeData(string? SearchText)
        {
            var results = _accountTreeService.GetAccountTreeData(SearchText);
            return Ok(results);
        }

        [HttpGet]
        [Route("GetAccountTreeHierarchicalData")]
        public IActionResult GetAccountTreeHierarchicalData(string? SearchText)
        {
            var results = _accountTreeService.GetAccountTreeHierarchicalData(SearchText);
            return Ok(results);
        }

        [HttpGet]
        [Route("GetAccountsSelector")]
        public List<SelectorDataModel> GetAccountsSelector(bool? IsGroup)
        {
            return _accountTreeService.GetAccountsSelector(IsGroup);
        }

        [HttpGet]
        [Route("GetAccountTypes")]
        public List<SelectorDataModel> GetAccountTypes()
        {
            return _accountTreeService.GetAccountTypes();
        }

        [HttpGet]
        [Route("GetCurrencySelector")]
        public List<SelectorDataModel> GetCurrencySelector()
        {
            return _accountTreeService.GetCurrencySelector();
        }

        [HttpGet]
        [Route("GetCostCenterSelector")]
        public List<SelectorDataModel> GetCostCenterSelector(bool IsParent)
        {
            return _accountTreeService.GetCostCenterSelector(IsParent);
        }
    }
}
