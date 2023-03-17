using BankingSystem.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.InternetBank.Operator.AddUserDetails
{
    internal class AddCardResponse
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string CardNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? FullName { get; set; }
        public int CVV { get; set; }
        public string PIN { get; set; }
        public AccountEntity Account { get; set; }
        public string? Message { get; set; }

    }
}
