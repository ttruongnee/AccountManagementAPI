using AccountManagementAPI.Database;
using AccountManagementAPI.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Repositories
{
    public class LogEntryRepository
    {
        private readonly IOracleDb OracleDb;
        public LogEntryRepository(IOracleDb db)
        {
            OracleDb = db;
        }
        public List<LogEntry> GetAllLogs()
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = @"SELECT 
                                l.*,
                                s.name AS sub_name
                            FROM logs l
                            LEFT JOIN sub_accounts s 
                                ON l.sub_id = s.sub_id";

                var logs = conn.Query<LogEntry>(sql);
                return logs.ToList();
            }
        }

        public bool CreateLog(LogEntry log)
        {
            using (var conn = OracleDb.GetConnection())
            {
                string sql = @"INSERT INTO logs
                            (account_id, sub_id, action, amount, success, note) 
                            VALUES 
                            (:account_id, :sub_id, :action, :amount, :success, :note)";
                var affectedRows = conn.Execute(sql, new
                {
                    account_id = log.Account_Id,
                    sub_id = log.Sub_Id,
                    action = log.Action,
                    amount = log.Amount,
                    success = log.Success ? 1 : 0,  // map bool -> 1/0
                    note = log.Note
                });
                return affectedRows > 0;
            }
        }
    }
}
