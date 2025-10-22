using System.Globalization;

namespace Bankomat.Validators
{
    public static class AmountValidator
    {
        public static bool IsValidAmount(string input, out double amount)
        {
            amount = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string normalizedInput = input.Replace(',', '.');

            if (!double.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out amount))
            {
                return false;
            }
            return amount > 0 && amount <= double.MaxValue && !double.IsInfinity(amount) && !double.IsNaN(amount);
        }

        public static bool IsValidAmount(double amount)
        {
            return amount > 0 && amount <= double.MaxValue && !double.IsInfinity(amount) && !double.IsNaN(amount);
        }

        public static string GetValidationErrorMessage(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Kwota nie może być pusta.";
            }

            if (!IsValidAmount(input, out double amount))
            {
                return "Nieprawidłowy format kwoty. Użyj liczby dodatniej.";
            }

            if (amount <= 0)
            {
                return "Kwota musi być większa od zera.";
            }

            return "Kwota jest prawidłowa.";
        }
    }
}