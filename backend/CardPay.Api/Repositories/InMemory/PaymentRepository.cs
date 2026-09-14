using System.Collections.Concurrent;
using Domain.Entities;
using Repositories.interfaces;

namespace Repositories.InMemory;

public class PaymentRepository : IPaymentRepository
{
    private readonly ConcurrentDictionary<Guid, Payment> _pendingPayments = new ConcurrentDictionary<Guid, Payment>();


    public void AddPending(Payment payment)
    {
        _pendingPayments[payment.Id] = payment;
    }
}