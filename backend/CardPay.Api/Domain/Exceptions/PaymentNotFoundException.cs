using Microsoft.AspNetCore.Mvc;

namespace Domain.Exceptions;

public sealed class PaymentNotFoundException : Exception
{
    public PaymentNotFoundException(string message) : base(message)
    {
    }
}