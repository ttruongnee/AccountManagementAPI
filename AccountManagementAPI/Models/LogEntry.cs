using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Models
{
    public class LogEntry
    {
        public decimal Log_Id { get; }
        public DateTime Time { get; } = DateTime.Now;
        public string Account_Id { get; }
        //string thì ngầm hiểu là có thể null nên không cần ? ở đầu
        public decimal? Sub_Id { get; }
        public string Sub_Name { get; }
        public string Action { get; }
        public double? Amount { get; }
        public bool Success { get; }
        public string Note { get; }
        public LogEntry() { }

        public LogEntry(string accountId, decimal? subId, string action, double? amount, bool success, string note)
        {
            Account_Id = accountId;
            Sub_Id = subId;
            Action = action;
            Amount = amount;
            Success = success;
            Note = note;
        }
    }
}
