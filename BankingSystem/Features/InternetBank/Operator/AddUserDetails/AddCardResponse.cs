using BankingSystem.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.InternetBank.Operator.AddUserDetails
{
    public class AddCardResponse
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public int CardId { get; set; }

    }
}
