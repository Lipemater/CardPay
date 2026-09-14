namespace Domain.Exceptions;

public sealed class PaymentValidationException : Exception
{
    public PaymentValidationException(string message) : base(message)
    {
    }
}