using System.Runtime.CompilerServices;

namespace Contracts.Responses;

public class CreatePaymentResponse
{
    public Guid Id { get; set; }
    public required string CardBrand { get; set; }
    public int Installments { get; set; }
    public int AmountInCents { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<InstallmentDetailResponse> InstallmentDetails { get; set; } = [];
}