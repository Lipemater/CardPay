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
}