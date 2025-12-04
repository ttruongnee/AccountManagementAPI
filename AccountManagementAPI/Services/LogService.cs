using System;
using System.Collections.Generic;
using System.Globalization;
using AccountManagementAPI.Models;
using AccountManagementAPI.Repositories;

namespace AccountManagementAPI.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly LogEntryRepository _loggerRepo;

        public LoggerService(LogEntryRepository logEntryRepo)
        {
            _loggerRepo = logEntryRepo;
        }

        public IReadOnlyList<LogEntry> GetAll() => _loggerRepo.GetAllLogs();  


        public void CreateLog(LogEntry logEntry)
        {
            //create
            throw new NotImplementedException();
        }


        public void ShowLogs()
        {
            var logs = _loggerRepo.GetAllLogs();

            if (logs.Count == 0)
            {
                Console.WriteLine("Chưa có hành động nào.");
                return;
            }

            Console.WriteLine("Thời gian\t\tTài khoản\tTài khoản con\t\tHành động\t\tSố tiền\t\tTrạng thái\tGhi chú");
            foreach (var log in logs)
            {
                string subName = log.Sub_Name != null ? log.Sub_Name.ToUpper() : "_\t";
                string amount = log.Amount.HasValue ? log.Amount.Value.ToString("N0", new CultureInfo("vi-VN")) + "đ" : "_";
                string status = log.Success ? "Thành công" : "Thất bại";
                Console.WriteLine($"{log.Time}\t{log.Account_Id.ToUpper()}\t\t{subName}\t\t{log.Action}\t\t{amount}\t{status}\t{log.Note}");
            }
        }
    }
}
