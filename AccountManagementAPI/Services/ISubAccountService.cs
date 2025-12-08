using AccountManagementAPI.DTOs;
using AccountManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Services
{
    public interface ISubAccountService
    {
        Dictionary<decimal, SubAccount> GetAllSubAccounts();

        //lấy ra dict subaccounts theo account_id
        Dictionary<decimal, SubAccount> GetByAccountId(string accountId);

        //lấy ra subaccount theo sub_id
        SubAccount GetBySubAccountId(string account_id, decimal sub_id);

        //lấy ra string kiểu tài khoản để hiển thị
        string GetSubAccountType(string type);

        //thêm
        bool CreateSubAccount(SubAccount subAccount, out string message);
        
        //xoá
        bool DeleteSubAccount(string account_id, decimal subId, out string message);


        //gửi tiền
        bool Deposit(string account_id, decimal subId, double amount, out string message);
        
        //rút tiền
        bool Withdraw(string account_id, decimal subId, double amount, out string message);

        //trả lãi
        bool PayInterest(string account_id, decimal subId, out string message);

        //lấy danh sách tài khoản kèm tổng số dư
        List<AccountTotalBalanceDTO> GetAccountsWithTotalBalance();

        //lấy danh sách tài khoản kèm subaccounts
        List<AccountWithSubAccountsDTO> GetAccountWithSubAccounts();

    }
}
