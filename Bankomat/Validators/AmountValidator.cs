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

            // Akceptujemy zarówno kropkę jak i przecinek jako separator dziesiętny
            string normalizedInput = input.Replace(',', '.');

            // Próba parsowania
            if (!double.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out amount))
            {
                return false;
            }

            // Sprawdzenie czy kwota jest dodatnia i ma sens
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