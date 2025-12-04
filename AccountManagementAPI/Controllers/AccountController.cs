using Microsoft.AspNetCore.Mvc;
using AccountManagementAPI.Services;
using AccountManagementAPI.DTOs;
using AccountManagementAPI.Utils;

namespace AccountManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: /api/accounts
        [HttpGet]
        public IActionResult GetAll()
        {
            var accounts = _accountService.GetAllAccounts();

            // map sang DTO
            var dtos = accounts.Values
                .Select(a => new AccountDto { AccountId = a.Account_Id })
                .ToList();

            return Ok(new
            {
                success = true,
                total = dtos.Count,
                data = dtos
            });
        }

        // GET: /api/accounts/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var acc = _accountService.GetAccountById(id);

            if (acc == null)
                return NotFound(new { success = false, message = "Không tìm thấy tài khoản." });

            return Ok(new AccountDto { AccountId = acc.Account_Id });
        }

        // POST: /api/accounts
        [HttpPost]
        public IActionResult Create([FromBody] AccountDto dto)
        {
            bool success = _accountService.CreateAccount(dto.AccountId, out string message);

            if (!success)
                return BadRequest(new { success = false, message });

            return Ok(new { success = true, message });
        }

        // DELETE: /api/accounts/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool success = _accountService.DeleteAccount(id, out string message);

            if (!success)
                return BadRequest(new { success = false, message });

            return Ok(new { success = true, message });
        }
    }
}
