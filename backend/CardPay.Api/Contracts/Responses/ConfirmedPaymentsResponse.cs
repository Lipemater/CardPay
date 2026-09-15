namespace Contracts.Responses;

public class ConfirmedPaymentsResponse
{
    public required List<ConfirmedPaymentResponse> Payments { get; set; }
}