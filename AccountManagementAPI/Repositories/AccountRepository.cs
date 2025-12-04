using System.Collections.Generic;
using Dapper;
using AccountManagementAPI.Database;
using AccountManagementAPI.Models;
using System.Linq;
using System.CodeDom;
using AccountManagementAPI.Database;

namespace AccountManagementAPI.Repositories
{
    public class AccountRepository
    {
        private readonly IOracleDb OracleDb;
        public AccountRepository(IOracleDb db)
        {
            OracleDb = db;
        }
        public Dictionary<string,Account> GetAllAccounts()
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "select account_id from accounts";

                var result = conn.Query<Account>(sql);
                return result.ToDictionary(r => r.Account_Id, r => r);
            }
        }

        public Account GetAccountById(string accountId)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "select account_id from accounts where account_id = :account_id";
                var account = conn.Query<Account>(sql, new {account_id = accountId.ToUpper()});

                return account.FirstOrDefault();
            }
        }

        public bool CreateAccount(Account account)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = @"insert into accounts(account_id) values (:Account_Id)";
                var affectedRows = conn.Execute(sql, account);  // Trả về số lượng bản ghi bị ảnh hưởng
                return affectedRows > 0;  // Trả về true nếu ít nhất 1 dòng bị ảnh hưởng
            }
        }


        public bool DeleteAccount(string account_id)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "delete from accounts where account_id = :account_id";
                var affectedRows = conn.Execute(sql, new {account_id});
                return affectedRows > 0;
            }
        }
    }
}
