using AccountManagementAPI.DTOs;
using AccountManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AccountManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalController : ControllerBase
    {
        private readonly ISubAccountService _subAccountService;

        public StatisticalController(ISubAccountService subAccountService)
        {
            _subAccountService = subAccountService;
        }

        [HttpGet("GetAccountsWithTotalBalance")]
        public IActionResult GetAccountsWithTotalBalance()
        {
            var result = _subAccountService.GetAccountsWithTotalBalance();
            if(result == null || result.Count == 0)
            {
                //return NotFound("Hệ thống chưa có tài khoản nào!");
                return Ok(new { success = true, total = 0, data = new List<object>() });
            }   

            return Ok(new  { success = true, total = result.Count, data = result }
            );
        }

        [HttpGet("GetAccountWithSubAccounts")]
        public IActionResult GetAccountWithSubAccounts()
        {
            var result = _subAccountService.GetAccountWithSubAccounts();
            if (result == null || result.Count == 0)
            {
                //return NotFound("Hệ thống chưa có tài khoản nào!");
                return Ok(new { success = true, total = 0, data = new List<object>() });
            }

            return Ok(new { success = true, total = result.Count, data = result }
            );
        }
    }
}
