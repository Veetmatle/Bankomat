namespace Bankomat.Models
{
    // Logika biznesowa wymiany walut (przeliczanie przez PLN)
    public class Exchange
    {
        public ExchangeTable Table { get; }

        public Exchange(ExchangeTable table)
        {
            Table = table;
        }

        public double ExchangeOne(double amount, ExchangeRate fromRate, ExchangeRate toRate)
        {
            if (fromRate == null || toRate == null)
            {
                throw new ArgumentException("Invalid currency code");
            }

            double plnAmount = amount * (fromRate.Rate / fromRate.Multiplier);
            double finalAmount = plnAmount / (toRate.Rate / toRate.Multiplier);
            
            return finalAmount;
        }
    }
}