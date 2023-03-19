namespace BankingSystem.Features.ATM.ChangePin
{
    public class ChangeCardPinRequest
    {
        public string? CardNumber { get; set; }
        public string? PIN { get; set; }
        public string? NewPIN { get; set; }
    }
}
