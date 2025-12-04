using AccountManagementAPI.Models;
using AccountManagementAPI.Repositories;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Services
{
    public class AccountService : IAccountService
    {
        //dùng Logger của nlog
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();  //lấy luôn tên logger là tên class
        private void Log(NLog.LogLevel logLevel, string accountId, decimal? subId, string action, double? amount, bool success, string note)
        {
            var logEvent = new LogEventInfo(logLevel, logger.Name, note);
            

            logEvent.Properties["AccountId"] = accountId ?? "";
            logEvent.Properties["SubId"] = subId?.ToString() ?? "";
            logEvent.Properties["Action"] = action ?? "";
            logEvent.Properties["Amount"] = amount?.ToString() ?? "";
            logEvent.Properties["Success"] = success;

            logger.Log(logEvent);
        }

        private readonly AccountRepository _accountRepo;
        private readonly SubAccountRepository _subAccountRepo;
        private readonly LogEntryRepository _loggerRepo;
        public AccountService(AccountRepository accountRepo, SubAccountRepository subAccountRepository, LogEntryRepository logEntryRepo)
        {
            _accountRepo = accountRepo;
            _subAccountRepo = subAccountRepository;
            _loggerRepo = logEntryRepo;
        }

        //trả về 1 dictionary chỉ đọc chứa các tài khoản (chính)
        public IReadOnlyDictionary<string, Account> GetAllAccounts() => _accountRepo.GetAllAccounts();


        public Account GetAccountById(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId.ToUpper())) return null;
            return _accountRepo.GetAccountById(accountId.ToUpper());
        }


        //tạo tài khoản chính, trả về kiểu bool và message thông báo
        public bool CreateAccount(string accountId, out string message)
        {
            if (string.IsNullOrWhiteSpace(accountId))
            {
                message = "Mã tài khoản chính không hợp lệ.";
                return false;
            }
            accountId = accountId.Trim().ToUpper();

            try
            {
                var result = _accountRepo.CreateAccount(new Account(accountId));
                if (!result)
                {
                    message = "Không thể tạo tài khoản chính.";

                    _loggerRepo.CreateLog(new LogEntry(accountId, null, "Tạo tài khoản chính", null, false, message));
                    Log(NLog.LogLevel.Info, accountId, null, "Tạo tài khoản chính", null, false, message);
                    return false;
                }

                message = "Tạo tài khoản chính thành công.";
                //log cho db
                _loggerRepo.CreateLog(new LogEntry(accountId, null, "Tạo tài khoản chính", null, true, message));
                //log cho text file bằng nlog
                Log(NLog.LogLevel.Info, accountId, null, "Tạo tài khoản chính", null, true, message);

                return true;
            }
            catch (OracleException ex)
            {
                switch (ex.Number)
                {
                    case 1:
                        message = "Tài khoản đã tồn tại.";
                        break;

                    case 1400:
                        message = "Thiếu dữ liệu yêu cầu (NOT NULL).";
                        break;

                    case 2291:
                        message = "Tài khoản cha không tồn tại.";
                        break;

                    case 904:
                        message = "Tên cột không hợp lệ.";
                        break;

                    default:
                        message = $"Lỗi CSDL (Oracle {ex.Number}): {ex.Message}";
                        break;
                }
                //log cho db
                _loggerRepo.CreateLog(new LogEntry(accountId, null, "Tạo tài khoản chính", null, true, message));
                //log cho text file bằng nlog
                Log(NLog.LogLevel.Error, accountId, null, "Tạo tài khoản chính", null, false, message);
                return false;
            }
            catch (Exception ex)
            {
                message = $"Lỗi hệ thống: {ex.Message}";
                //log cho db
                _loggerRepo.CreateLog(new LogEntry(accountId, null, "Tạo tài khoản chính", null, true, message));
                //log cho text file bằng nlog
                Log(NLog.LogLevel.Error, accountId, null, "Tạo tài khoản chính", null, false, message);
                return false;
            }

        }

        //xoá tài khoản chính khi truyền vào id tài khoản, trả về kiểu bool và message thông báo
        public bool DeleteAccount(string accountId, out string message)
        {
            accountId = accountId.Trim().ToUpper();

            var acc = _accountRepo.GetAccountById(accountId);
            if (acc == null)
            {
                message = "Không tồn tại tài khoản.";
                return false;
            }

            var subAccounts = _subAccountRepo.GetByAccountId(accountId);
            if (subAccounts != null)
            {
                //kiểm tra số dư của tất cả tài khoản con của tài khoản muốn xoá, nếu có tài khoản con > 0 thì không xoá được
                var blockedSub = subAccounts.Values.FirstOrDefault(s => s.Balance > 0);
                if (blockedSub != null)
                {
                    message = $"Không thể xóa: tài khoản con {blockedSub.Name} còn tiền.";
                    _loggerRepo.CreateLog(new LogEntry(blockedSub.Account_Id, blockedSub.Sub_Id, "Xoá tài khoản chính", null, false, message));
                    Log(NLog.LogLevel.Info, blockedSub.Account_Id, blockedSub.Sub_Id, "Xoá tài khoản chính", null, false, message);
                    return false;
                }
            }
            try
            {
                var result = _accountRepo.DeleteAccount(accountId);
                if (!result)
                {
                    message = $"Xoá tài khoản {accountId} thất bại.";
                    _loggerRepo.CreateLog(new LogEntry(acc.Account_Id, null, "Xoá tài khoản chính", null, false, message));
                    Log(NLog.LogLevel.Info, acc.Account_Id, null, "Xoá tài khoản chính", null, false, message);
                    return false;
                }
                
                message = $"Xoá tài khoản {accountId} thành công.";
                _loggerRepo.CreateLog(new LogEntry(acc.Account_Id, null, "Xoá tài khoản chính", null, true, message));
                Log(NLog.LogLevel.Info, acc.Account_Id, null, "Xoá tài khoản chính", null, true, message);

                return true;
            }
            catch (OracleException ex)
            {
                switch (ex.Number)
                { 
                    case 2292:
                        message = "Không thể xóa tài khoản: còn tài khoản con liên quan (FK constraint).";
                        break;

                    default:
                        message = $"Lỗi CSDL (Oracle {ex.Number}): {ex.Message}";
                        break;
                }
                _loggerRepo.CreateLog(new LogEntry(acc.Account_Id, null, "Xoá tài khoản chính", null, false, message));
                Log(NLog.LogLevel.Error, acc.Account_Id, null, "Xoá tài khoản chính", null, false, message);
                return false;
            }
            catch (Exception ex)
            {
                message = $"Lỗi hệ thống: {ex.Message}";
                _loggerRepo.CreateLog(new LogEntry(acc.Account_Id, null, "Xoá tài khoản chính", null, false, message));
                Log(NLog.LogLevel.Error, acc.Account_Id, null, "Xoá tài khoản chính", null, false, message);
                return false;
            }           
        }
    }
}
