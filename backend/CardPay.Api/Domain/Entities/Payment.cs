using Domain.Enums;

namespace Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public CardBrand CardBrand { get; set; }
    public int Installments { get; set; }
    public int AmountInCents { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
    public List<Installment> InstallmentsDetails { get; set; } = [];
}