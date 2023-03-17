namespace BankingSystem.Features.ATM.ChangePin
{
    public class ChangeCardPINRequest
    {
        public string? CardNumber { get; set; }
        public string? PIN { get; set; }
        public string? NewPIN { get; set; }
    }
}
