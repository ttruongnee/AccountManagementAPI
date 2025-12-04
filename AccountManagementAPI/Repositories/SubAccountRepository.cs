using System.Collections.Generic;
using Dapper;
using AccountManagementAPI.Database;
using AccountManagementAPI.Models;
using System.Linq;
using System.Data;
using AccountManagementAPI.Database;

namespace AccountManagementAPI.Repositories
{
    public class SubAccountRepository
    {
        private readonly IOracleDb OracleDb;
        public SubAccountRepository(IOracleDb db)
        {
            OracleDb = db;
        }
        public Dictionary<decimal, SubAccount> GetAllSubAccounts()
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "select * from sub_accounts";
                var allsubs = conn.Query(sql);

                var dict = new Dictionary<decimal, SubAccount>();
                foreach (var sub in allsubs)
                {
                    dict.Add(sub.SUB_ID, new SubAccount(sub.ACCOUNT_ID, sub.NAME, sub.TYPE, sub.BALANCE));
                }
                return dict;
            }
        }
        public Dictionary<decimal, SubAccount> GetByAccountId(string accountId)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "select * from sub_accounts where account_id = :account_id";
                var subs = conn.Query(sql, new { account_id = accountId.ToUpper() });

                var dict = new Dictionary<decimal, SubAccount>();
                foreach (var sub in subs)
                {
                    dict.Add(sub.SUB_ID, new SubAccount((int)sub.SUB_ID, sub.ACCOUNT_ID, sub.NAME, sub.TYPE, sub.BALANCE));
                }
                return dict;
            }
        }

        public SubAccount GetBySubAccountId(string account_id, decimal sub_id)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "select * from sub_accounts where sub_id = :sub_id and account_id = :account_id";
                var sub = conn.Query<SubAccount>(sql, new { sub_id, account_id});
                
                return sub.FirstOrDefault();
            }
        }

        public bool CreateSubAccount(SubAccount subAccount, out decimal newSubId)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "insert into sub_accounts(name, account_id, balance, type) VALUES (:Name, :Account_Id, :Balance, :Type) returning sub_id into :NewSub_Id";
                var parameters = new DynamicParameters();
                parameters.Add("Name", subAccount.Name);
                parameters.Add("Account_Id", subAccount.Account_Id);
                parameters.Add("Balance", subAccount.Balance);
                parameters.Add("Type", subAccount.Type);

                // Tham số output để nhận sub_id
                parameters.Add("NewSub_Id", dbType: DbType.Decimal, direction: ParameterDirection.Output);

                var result = conn.Execute(sql, parameters);
                newSubId = parameters.Get<decimal>("NewSub_Id"); // Lấy sub_id trả về
                return result > 0;
            }
        }

        public bool UpdateSubAccount(SubAccount subAccount)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "update sub_accounts set name = :Name, balance = :Balance, type = :Type where sub_id = :Sub_Id";
                var result = conn.Execute(sql, subAccount);

                return result > 0;
            }
        }

        
        public bool DeleteSubAccount(string account_id, decimal subId)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = "delete from sub_accounts where sub_id = :subId and account_id = :account_id";
                var result = conn.Execute(sql, new { account_id, subId });

                return result > 0;  
            }
        }
    }
}
