using BankingSystem.DB.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.InternetBank.Operator.AddAccountForUser
{
    public class AddAccountRequest
    {
        public int UserId { get; set; }
        public string IBAN { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Amount must be a valid number")]
        public decimal Amount { get; set; }
        [Range(0, 2, ErrorMessage = "Currency must be a number between 0 and 3")]
        public Currency Currency { get; set; }    
    }
}
