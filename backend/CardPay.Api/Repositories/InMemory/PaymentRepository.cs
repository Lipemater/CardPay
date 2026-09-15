using System.Collections.Concurrent;
using Domain.Entities;
using Repositories.interfaces;

namespace Repositories.InMemory;

public class PaymentRepository : IPaymentRepository
{
    private readonly ConcurrentDictionary<Guid, Payment> _pendingPayments = new ConcurrentDictionary<Guid, Payment>();

    private readonly ConcurrentDictionary<Guid, Payment> _confirmedPayments = new ConcurrentDictionary<Guid, Payment>();



    public void AddPending(Payment payment) => _pendingPayments[payment.Id] = payment;

    public Payment? GetPaymentById(Guid id) => _pendingPayments.TryGetValue(id, out Payment? payment) ? payment : null;

    public Payment? RemovePending(Guid id) => _pendingPayments.TryRemove(id, out Payment? payment) ? payment : null;

    public void AddConfirmed(Payment payment) => _confirmedPayments[payment.Id] = payment;
}