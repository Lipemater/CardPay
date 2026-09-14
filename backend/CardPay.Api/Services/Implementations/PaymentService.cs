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