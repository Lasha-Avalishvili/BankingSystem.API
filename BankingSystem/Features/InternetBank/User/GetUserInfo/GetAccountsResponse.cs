using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
   

    public class GetAccountsResponse 
    {
     //   public bool IsSuccessful { get; set; } if user has multiple cards he will have this fields in all of it
     //   public string? Error { get; set; }
        public int AccountId { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
    }
}
