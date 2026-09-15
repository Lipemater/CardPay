using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Repositories.interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICardBrandDetector _cardBrandDetector;

    public PaymentService(IPaymentRepository paymentRepository, ICardBrandDetector cardBrandDetector)
    {
        _paymentRepository = paymentRepository;
        _cardBrandDetector = cardBrandDetector;
    }



    public Payment CreatePayment(string cardNumber, int installments, int amountInCents)
    {
        if (!(installments >= PaymentRules.MinimumInstallments && installments <= PaymentRules.MaximumInstallments))
        {
            throw new PaymentValidationException($"Invalid number of installments!");
        }

        if (amountInCents <= 0)
        {
            throw new PaymentValidationException($"Value cannot be 0 or negative!");
        }

        if (amountInCents < installments)
        {
            throw new PaymentValidationException($"Value cannot be greater then number of installments!");
        }

        CardBrand? cardBrand = _cardBrandDetector.Detect(cardNumber);
        if (cardBrand == null)
        {
            throw new PaymentValidationException($"Card number is invalid or unsupported.");
        }
        CardBrand brand = cardBrand.Value;

        Payment payment = new Payment();
        payment.Id = Guid.NewGuid();
        payment.CardBrand = brand;
        payment.Installments = installments;
        payment.AmountInCents = amountInCents;
        payment.CreatedAt = DateTimeOffset.UtcNow;
        payment.ConfirmedAt = null;
        payment.InstallmentsDetails = CalculateInstallments(amountInCents, installments);

        _paymentRepository.AddPending(payment);
        return payment;

    }

    public Payment ConfirmPayment(Guid id)
    {
        var payment = _paymentRepository.GetPaymentById(id);
        if (payment == null)
        {
            throw new PaymentNotFoundException("Payment not found or already confirmed.");
        }

        var now = DateTimeOffset.UtcNow;
        var timePassed = now - payment.CreatedAt;
        if (timePassed >= TimeSpan.FromMinutes(1))
        {
            throw new PaymentValidationException("Payment confirmation expired. The payment must be confirmed within 1 minute.");
        }


        var paymentRemoved = _paymentRepository.RemovePending(id);
        if (paymentRemoved == null)
        {
            throw new PaymentNotFoundException("Payment could not be removed from pending payments because it was not found or has already been processed.");
        }


        paymentRemoved.ConfirmedAt = now;
        _paymentRepository.AddConfirmed(payment);
        return payment;
    }

    public List<Payment> GetConfirmedPayments() => _paymentRepository.GetConfirmedPayments();










    private List<Installment> CalculateInstallments(int amountInCents, int installments)
    {
        int baseAmount = amountInCents / installments;
        int remainder = amountInCents % installments;

        List<Installment> listInstallments = [];
        for (int i = 1; i <= installments; i++)
        {
            Installment currentInstallment = new Installment();
            currentInstallment.InstallmentNumber = i;

            currentInstallment.AmountInCents = baseAmount;
            if (i == 1)
            {
                currentInstallment.AmountInCents += remainder;
            }

            listInstallments.Add(currentInstallment);
        }


        return listInstallments;
    }
}