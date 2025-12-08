using System.ComponentModel.DataAnnotations;

namespace AccountManagementAPI.DTOs
{

    public class ValidSubAccountTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string type)
            {
                return type.ToUpper() == "ĐT" || type.ToUpper() == "TK";
            }
            return false;
        }
    }

    public class SubAccountDTO
    {
        public int? Sub_Id { get; set; }
        public string Account_Id { get; set; }
        public string Name { get; set; }

        [ValidSubAccountType(ErrorMessage = "Type chỉ được phép là 'ĐT' hoặc 'TK'")]
        public string Type { get; set;}

        public double Balance { get; set; }
    }

    public class TransactionRequestDTO
    {
        [Required]
        public string Account_Id { get; set; }

        [Required]
        public int Sub_Id { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public double Amount { get; set; }
    }

    public class PayInterestDTO
    {
        [Required]
        public string Account_Id { get; set; }

        [Required]
        public int Sub_Id { get; set; }
    }

    public class AccountTotalBalanceDTO
    {
        public string AccountId { get; set; }
        public double TotalBalance { get; set; }
    }

    public class AccountWithSubAccountsDTO
    {
        public string AccountId { get; set; }
        public List<SubAccountDTO> SubAccounts { get; set; }
        public double TotalBalance { get; set; }
    }

}
