using AccountManagementAPI.Models;
using AccountManagementAPI.Repositories;
using NLog;

namespace AccountManagementAPI.Ultils
{
    public class LogHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly LogEntryRepository _loggerRepo;

        public LogHelper(LogEntryRepository loggerRepo)
        {
            _loggerRepo = loggerRepo;
        }

        public void WriteLog(
            NLog.LogLevel level,
            string accountId,
            decimal? subId,
            string action,
            double? amount,
            bool success,
            string message)
        {
            //ghi log vào db
            var entry = new LogEntry(accountId, subId, action, amount, success, message);
            _loggerRepo.CreateLog(entry);

            //ghi log vào file bằng nlog
            var logEvent = new LogEventInfo(level, logger.Name, message);

            logEvent.Properties["AccountId"] = accountId ?? "";
            logEvent.Properties["SubId"] = subId?.ToString() ?? "";
            logEvent.Properties["Action"] = action ?? "";
            logEvent.Properties["Amount"] = amount?.ToString() ?? "";
            logEvent.Properties["Success"] = success;

            logger.Log(logEvent);
        }
    }
}
