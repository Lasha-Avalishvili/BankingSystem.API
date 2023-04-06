using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.User.Transactions;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public class GetTransactionsResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public List<TransactionObject> TransactionReponse {get; set;}

    }


}
