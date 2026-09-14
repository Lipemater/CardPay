using Contracts.Responses;
using Domain.Entities;

namespace Repositories.interfaces;

public interface IPaymentRepository
{
    public void AddPending(Payment payment);
}