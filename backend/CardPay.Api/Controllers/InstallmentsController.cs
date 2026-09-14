using Microsoft.AspNetCore.Mvc;
using Contracts.Responses;
using Domain.Constants;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstallmentsController : ControllerBase
{
    [HttpGet]
    public ActionResult<AvaliableInstallmentsResponse> GetInstallments()
    {
        var installments = Enumerable.Range(PaymentRules.MinimumInstallments, ((PaymentRules.MaximumInstallments - PaymentRules.MinimumInstallments) + 1)).ToList();


        var response = new AvaliableInstallmentsResponse()
        {
            Installments = installments
        };
        return Ok(response);
    }
}
