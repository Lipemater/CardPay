using Domain.Enums;

namespace Services.Interfaces;

public interface ICardBrandDetector
{
    public CardBrand? Detect(string cardNumber);
}