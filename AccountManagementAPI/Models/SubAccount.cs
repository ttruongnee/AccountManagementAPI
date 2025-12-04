using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Models
{
    public class SubAccount
    {
        public int? Sub_Id { get; set; }
        public string Account_Id { get; set; }
        public string Name { get; }
        public string Type { get; set; }
        //public double InterestRate { get; }
        public double InterestRate => Type == "TK" ? 4.7 : 5.1;


        private double _balance;
        public double Balance
        {
            get => _balance;
            protected set => _balance = double.IsNaN(value) || value < 0 ? 0 : value;
        }

        public SubAccount() { }

        //khởi tạo 
        public SubAccount(int? sub_Id, string account_Id, string name, string type, double initialBalance = 0)
        {
            Sub_Id = sub_Id;
            Account_Id = account_Id;            
            Name = name.ToUpper();
            Type = type;
            Balance = initialBalance;
        }

        public double GetInterest() => Balance * InterestRate / 100;

        public void Deposit(double amount)
        {
            if (amount <= 0) throw new ArgumentException("Số tiền nạp phải lớn hơn 0");
            Balance += amount;
        }

        public void Withdraw(double amount)
        {
            if (amount <= 0) throw new ArgumentException("Số tiền muốn rút phải lớn hơn 0");
            if (Balance < amount) throw new InvalidOperationException("Số tiền muốn rút lớn hơn số tiền đang có");
            Balance -= amount;
        }
    }
}
