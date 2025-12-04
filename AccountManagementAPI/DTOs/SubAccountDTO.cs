using System.ComponentModel.DataAnnotations;

namespace AccountManagementAPI.DTOs
{
    public class SubAccountDTO
    {
        public int? Sub_Id { get; set; }
        public string Account_Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set;}
        //public double InterestRate => Type == "TK" ? 4.7 : 5.1;
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

}
