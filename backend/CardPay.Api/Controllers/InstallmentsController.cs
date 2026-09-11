using Microsoft.AspNetCore.Mvc;
using Contracts.Responses.InstallmentsResponse;
using Domain.Constants.PaymentRules;
using System.Linq;

namespace Controllers.InstallmentControllers;

[ApiController]
[Route("api/[controller]")]
public class InstallmentsController : ControllerBase
{
    [HttpGet]
    public ActionResult<InstallmentsResponse> GetInstallments()
    {
        var installments = Enumerable.Range(PaymentRules.MinimumInstallments, ((PaymentRules.MaximumInstallments - PaymentRules.MinimumInstallments) + 1)).ToList();


        var response = new InstallmentsResponse()
        {
            Installments = installments
        };
        return Ok(response);
    }
}