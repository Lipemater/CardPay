using Contracts.Request;
using Contracts.Responses;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }


    [HttpGet]
    public ActionResult<ConfirmedPaymentsResponse> ConfirmedPayments()
    {
        var payments = _paymentService.GetConfirmedPayments();

        var confirmedPayment = payments.Select(payment => new ConfirmedPaymentResponse
        {
            Id = payment.Id,
            CardBrand = payment.CardBrand.ToString(),
            AmountInCents = payment.AmountInCents,
            Installments = payment.Installments,
            ConfirmedAt = payment.ConfirmedAt!.Value,
            InstallmentDetails = payment.InstallmentsDetails.Select(installment => new InstallmentDetailResponse
            {
                InstallmentNumber = installment.InstallmentNumber,
                AmountInCents = installment.AmountInCents
            }).ToList()
        }).ToList();

        var response = new ConfirmedPaymentsResponse
        {
            Payments = confirmedPayment
        };

        return Ok(response);

    }


    [HttpPost]
    public ActionResult<CreatePaymentResponse> CreatePayment(CreatePaymentRequest paymentRequest)
    {
        try
        {
            var payment = _paymentService.CreatePayment(paymentRequest.CardNumber, paymentRequest.Installments, paymentRequest.AmountInCents);

            var response = new CreatePaymentResponse
            {
                Id = payment.Id,
                CardBrand = payment.CardBrand.ToString(),
                Installments = payment.Installments,
                AmountInCents = payment.AmountInCents,
                CreatedAt = payment.CreatedAt,

                InstallmentDetails = payment.InstallmentsDetails.Select(s => new InstallmentDetailResponse
                {
                    InstallmentNumber = s.InstallmentNumber,
                    AmountInCents = s.AmountInCents
                }).ToList()
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (PaymentValidationException error)
        {
            var response = new ErrorResponse
            {
                Message = error.Message
            };
            return BadRequest(response);
        }

    }

    [HttpPost("{id}/confirm")]
    public ActionResult<ConfirmPaymentResponse> ConfirmPayment(Guid id)
    {
        try
        {
            var payment = _paymentService.ConfirmPayment(id);

            var response = new ConfirmPaymentResponse
            {
                Message = "Payment confirmed.",
                Id = payment.Id,
                ConfirmedAt = payment.ConfirmedAt!.Value
            };

            return Ok(response);
        }
        catch (PaymentNotFoundException error)
        {
            var response = new ErrorResponse
            {
                Message = error.Message
            };
            return NotFound(response);
        }
        catch (PaymentValidationException error)
        {
            var response = new ErrorResponse
            {
                Message = error.Message
            };
            return BadRequest(response);
        }
    }
}