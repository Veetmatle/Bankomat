namespace Bankomat.Models
{
    // Tabela kursów z datą i numerem tabeli NBP
    public class ExchangeTable
    {
        public DateTime Date { get; }
        public string No { get; }
        public List<ExchangeRate> Rates { get; }

        public ExchangeTable(DateTime date, string no, List<ExchangeRate> rates)
        {
            Date = date;
            No = no;
            Rates = rates;
        }
    }
}