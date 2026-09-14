using Microsoft.AspNetCore.Mvc;
using Domain.Enums;
using Contracts.Responses;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardBrandsController : ControllerBase
{
    [HttpGet]
    public ActionResult<CardBrandsResponse> GetCardBrands()
    {
        var cardBrands = Enum.GetValues<CardBrand>().Select(cardBrand => cardBrand.ToString()).ToList();

        var response = new CardBrandsResponse()
        {
            CardBrands = cardBrands
        };
        return Ok(response);
    }
}
