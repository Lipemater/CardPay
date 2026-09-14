using Services.Interfaces;
using Domain.Enums;
using System.Text.RegularExpressions;

namespace Services.Implementations;

public class CardBrandDetector : ICardBrandDetector
{

    private static readonly string[] visaElectronPrefixes =
    {
        "4026",
        "417500",
        "4508",
        "4844",
        "4913",
        "4917"
    };

    private static readonly string[] amexPrefixes =
    {
        "34",
        "37"
    };

    private static readonly int[] lengthValid =
    {
        13,
        16,
        19
    };

    private static readonly string[] eloCreditPrefixes =
    {
        "401178",
        "401179",
        "431274",
        "438935",
        "457631",
        "457632",
        "506700",
        "506701",
        "506702",
        "506704",
        "506705",
        "506706",
        "506710",
        "506711",
        "506712",
        "506714",
        "506716",
        "506717",
        "506718",
        "506733",
        "506737",
        "506739",
        "506741",
        "506742"
    };

    private static readonly string[] eloDebitPrefixes =
    {
        "451416",
        "506699",
        "506703",
        "506707",
        "506708",
        "506709",
        "506713",
        "506715",
        "506719",
        "506722",
        "506723",
        "506734",
        "506735",
        "506736",
        "506738",
        "506750",
        "506751",
        "506752",
        "506754",
        "506769",
        "509003",
        "509010",
        "509011",
        "509012"
    };


    public CardBrand? Detect(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber))
        {
            return null;
        }

        string numberClean = Normalize(cardNumber);


        if (!NumberIsValid(numberClean))
        {
            return null;
        }


        if (!IsValidLuhn(numberClean))
        {
            return null;
        }



        if (IsVisaElectron(numberClean))
        {
            return CardBrand.VisaElectron;
        }
        if (IsEloCredit(numberClean))
        {
            return CardBrand.EloCredito;
        }
        if (IsEloDebit(numberClean))
        {
            return CardBrand.EloDebito;
        }
        if (IsMaestro(numberClean))
        {
            return CardBrand.Maestro;
        }
        if (IsAmex(numberClean))
        {
            return CardBrand.Amex;
        }
        if (IsMastercard(numberClean))
        {
            return CardBrand.Mastercard;
        }
        if (IsVisa(numberClean))
        {
            return CardBrand.Visa;
        }

        return null;

    }





    private string Normalize(string cardNum)
    {
        return Regex.Replace(cardNum, @"[\s-]", "");
    }

    private bool NumberIsValid(string cardNum)
    {
        if (cardNum.Length < 12 || cardNum.Length > 19)
        {
            return false;
        }
        if (!cardNum.All(char.IsDigit) || string.IsNullOrEmpty(cardNum))
        {
            return false;
        }

        return true;
    }



    private bool IsValidLuhn(string cardNum)
    {
        int sum = 0;
        bool shouldDouble = false;

        for (int i = cardNum.Length - 1; i >= 0; i--)
        {
            int digit = cardNum[i] - '0';

            if (shouldDouble)
            {
                digit *= 2;

                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            shouldDouble = !shouldDouble;
        }

        return sum % 10 == 0;
    }



    private bool IsVisaElectron(string cardNum)
    {

        return visaElectronPrefixes.Any(prefix => cardNum.StartsWith(prefix));
    }

    private bool IsAmex(string cardNum)
    {

        return (amexPrefixes.Any(prefix => cardNum.StartsWith(prefix)) && cardNum.Length == 15);
    }

    private bool IsMastercard(string cardNum)
    {
        int firstTwoDigits = int.Parse(cardNum.Substring(0, 2));
        int firstSixDigits = int.Parse(cardNum.Substring(0, 6));

        bool isOldMastercardRange = firstTwoDigits >= 51 && firstTwoDigits <= 55;
        bool isNewMastercardRange = firstSixDigits >= 222100 && firstSixDigits <= 272099;
        bool hasValidLength = cardNum.Length == 16;

        return ((isOldMastercardRange || isNewMastercardRange) && hasValidLength);

    }

    private bool IsMaestro(string cardNum)
    {
        int firstEightDigits = int.Parse(cardNum.Substring(0, 8));

        bool isMaestro = (firstEightDigits >= 63900000 && firstEightDigits <= 63909999) || (firstEightDigits >= 67000000 && firstEightDigits <= 67999999);
        bool hasValidLength = cardNum.Length >= 12 && cardNum.Length <= 19;

        return (isMaestro && hasValidLength);
    }

    private bool IsVisa(string cardNum)
    {

        return (lengthValid.Any(n => n == cardNum.Length) && cardNum.StartsWith('4'));

    }

    private bool IsEloCredit(string cardNum)
    {

        return eloCreditPrefixes.Any(prefix => cardNum.StartsWith(prefix));
    }

    private bool IsEloDebit(string cardNum)
    {

        return eloDebitPrefixes.Any(prefix => cardNum.StartsWith(prefix));
    }

}