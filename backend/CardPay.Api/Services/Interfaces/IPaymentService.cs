using Domain.Entities;

namespace Services.Interfaces;

public interface IPaymentService
{
    public Payment CreatePayment(string cardNumber, int installments, int amountInCents);
}