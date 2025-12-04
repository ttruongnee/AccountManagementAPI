using AccountManagementAPI.DTOs;
using AccountManagementAPI.Models;
using AccountManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AccountManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubAccountController : ControllerBase
    {
        private readonly ISubAccountService _subAccountService;

        public SubAccountController(ISubAccountService subAccountService)
        {
            _subAccountService = subAccountService;
        }

        [HttpGet]
        public IActionResult GetAllSubAccounts()
        {
            var subAccounts = _subAccountService.GetAllSubAccounts();

            if (subAccounts == null || subAccounts.Count == 0)
                return Ok(new { success = true, total = 0, data = new List<SubAccountDTO>() });


            var dtos = subAccounts.Values.Select(subAccounts => new SubAccountDTO
            {
                Sub_Id = subAccounts.Sub_Id,
                Account_Id = subAccounts.Account_Id,
                Name = subAccounts.Name,
                Type = subAccounts.Type,
                Balance = subAccounts.Balance
            })
                .ToList();

            return Ok(new {success = true, total = dtos.Count, data = dtos });
        }

        [HttpGet("ByAccountId/{accountId}")]
        public IActionResult GetByAccountId(string accountId)
        {
            accountId = accountId.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(accountId))
                return BadRequest(new { success = false, message = "accountId không được để trống" });

            var subAccounts = _subAccountService.GetByAccountId(accountId);

            if (subAccounts == null || subAccounts.Count == 0)
                return NotFound(new { success = false, message = $"Tài khoản {accountId} không có tài khoản con" });

            var dtos = subAccounts.Values.Select(subAccounts => new SubAccountDTO
            {
                Sub_Id = subAccounts.Sub_Id,
                Account_Id = subAccounts.Account_Id,
                Name = subAccounts.Name,
                Type = subAccounts.Type,
                Balance = subAccounts.Balance
            })
                .ToList();
            return Ok(new { success = true, total = dtos.Count, data = dtos });
        }

        [HttpGet("BySubId/{subId}")]  //có thể viết route bằng cách viết ở dòng này hoặc thêm [FromRoute] vào int subId ở dòng dưới 
        public IActionResult GetBySubAccountId([FromQuery][Required] string accountId, int subId)
        {
            accountId = accountId.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(accountId))
                return BadRequest(new { success = false, message = "accountId không được để trống" });

            var subAccount = _subAccountService.GetBySubAccountId(accountId, subId);

            if (subAccount == null)
                return NotFound(new { success = false, message = $"Không tìm thấy tài khoản con với Sub_Id {subId} và Account_Id {accountId}" });

            var dto = new SubAccountDTO()
            {
                Sub_Id = subAccount.Sub_Id,
                Account_Id = subAccount.Account_Id,
                Name = subAccount.Name,
                Type = subAccount.Type,
                Balance = subAccount.Balance
            };
            return Ok(new { success = true, data = dto });
        }

        [HttpPost]
        public IActionResult CreateSubAccount([FromBody] SubAccountDTO dto)
        {
            SubAccount sub = new SubAccount(dto.Sub_Id, dto.Account_Id, dto.Name, dto.Type, dto.Balance);
            bool success = _subAccountService.CreateSubAccount(sub, out string message);

            if (!success)
            {
                return BadRequest(new {success = false, message});
            }
            return Ok(new {success = true, message});
        }

        [HttpDelete("{subId}")]
        public IActionResult DeleteSubAccount(int subId, [FromQuery][Required] string accountId)
        {
            bool success = _subAccountService.DeleteSubAccount(accountId, subId, out string message);

            if (!success) return BadRequest(new {success = false, message});

            return Ok(new {success = true, message});
        }

        [HttpPut("Deposit")]
        public IActionResult Deposit([FromBody] TransactionRequestDTO dto)
        {
            bool success = _subAccountService.Deposit(dto.Account_Id, dto.Sub_Id, dto.Amount, out string message);

            if (!success) return BadRequest(new {success=false, message});

            return Ok(new {success = true,  message});
        }

        [HttpPut("Withdraw")]
        public IActionResult Withdraw([FromBody] TransactionRequestDTO dto)
        {
            bool success = _subAccountService.Withdraw(dto.Account_Id, dto.Sub_Id, dto.Amount, out string message);

            if (!success) return BadRequest(new { success = false, message });

            return Ok(new { success = true, message });
        }

        [HttpPut("PayInterest")]
        public IActionResult PayInterest([FromBody] PayInterestDTO dto)
        {
            bool success = _subAccountService.PayInterest(dto.Account_Id, dto.Sub_Id, out string message);

            if (!success) return BadRequest(new { success = false, message });

            return Ok(new { success = true, message });
        }
    }
}
