using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.ChangePin
{
    public class ChangePinResponse
    {
        public bool IsSuccessful { get; set; }
        public string? Error { get; set; } 
    }
}
