namespace Contracts.Responses;

public class ConfirmedPaymentResponse
{
    public Guid Id { get; set; }
    public DateTimeOffset ConfirmedAt { get; set; }
    public required string CardBrand { get; set; }
    public int AmountInCents { get; set; }
    public int Installments { get; set; }
    public List<InstallmentDetailResponse> InstallmentDetails { get; set; } = [];
}