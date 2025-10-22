using Bankomat.Models;

namespace Bankomat.Validators
{
    public static class CurrencyValidator
    {
        public static bool IsValidCurrencyCode(string code, List<ExchangeRate> availableRates)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            return availableRates.Any(r => r.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }

        public static ExchangeRate? GetRateByCode(string code, List<ExchangeRate> availableRates)
        {
            return availableRates.FirstOrDefault(r => 
                r.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }

        public static bool TryGetRateByCode(string code, List<ExchangeRate> availableRates, out ExchangeRate? rate)
        {
            rate = GetRateByCode(code, availableRates);
            return rate != null;
        }
    }
}