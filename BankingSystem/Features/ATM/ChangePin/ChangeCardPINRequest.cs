using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Features.ATM.ChangePin
{
    public class ChangeCardPinRequest
    {
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits")]
        public string? CardNumber { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be 4 digits")]
        public string? PIN { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "New PIN must be 4 digits")]
        public string? NewPIN { get; set; }
    }
}
