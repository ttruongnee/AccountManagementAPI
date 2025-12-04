using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Models
{
    public class Account
    {
        public string Account_Id { get;}

        public Account() { }
        public Account(string accountId)
        {
            Account_Id = (accountId ?? string.Empty).Trim().ToUpper();
        }
    }
}
