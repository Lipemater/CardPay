using Contracts.Responses;
using Domain.Entities;

namespace Repositories.interfaces;

public interface IPaymentRepository
{
    public void AddPending(Payment payment);
    public Payment? GetPaymentById(Guid id);
    public Payment? RemovePending(Guid id);
    public void AddConfirmed(Payment payment);

}