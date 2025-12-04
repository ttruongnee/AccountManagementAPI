using AccountManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Services
{
    public interface ILoggerService
    {
        void CreateLog(LogEntry logEntry); 
        IReadOnlyList<LogEntry> GetAll(); 
        void ShowLogs();  
    }
}
