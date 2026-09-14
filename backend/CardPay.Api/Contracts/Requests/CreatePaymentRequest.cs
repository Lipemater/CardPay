namespace Contracts.Request;

public class CreatePaymentRequest
{
    public required string CardNumber { get; set; }
    public int Installments { get; set; }
    public int AmountInCents { get; set; }
}