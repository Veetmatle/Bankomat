namespace Bankomat.Models
{
    // Model pojedyńczego kursu waluty
    public class ExchangeRate
    {
        public double Rate { get; }
        public double Multiplier { get; }
        public string Code { get; }
        public string Name { get; }

        public ExchangeRate(double rate, double multiplier, string code, string name)
        {
            Rate = rate;
            Multiplier = multiplier;
            Code = code;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Code} - {Name} (1 {Code} = {Rate / Multiplier:F4} PLN)";
        }
    }
}