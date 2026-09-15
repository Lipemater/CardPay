namespace Contracts.Responses;

public class ConfirmPaymentResponse
{
    public required string Message { get; set; }
    public Guid Id { get; set; }
    public DateTimeOffset ConfirmedAt { get; set; }
}