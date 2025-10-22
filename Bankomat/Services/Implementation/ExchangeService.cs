using Bankomat.Models;
using Bankomat.Services.Interfaces;

namespace Bankomat.Services.Implementation
{
    public sealed class ExchangeService : IExchangeService
    {
        private static readonly Lazy<ExchangeService> _instance = 
            new Lazy<ExchangeService>(() => new ExchangeService());

        private readonly IRateRepository _repository;
        private readonly IRateParser _parser;
        private readonly string _sourceUrl;
        private Exchange? _exchange;
        private ExchangeTable? _currentTable;

        public static ExchangeService Instance => _instance.Value;

        private ExchangeService()
        {
            _sourceUrl = "https://static.nbp.pl/dane/kursy/xml/LastA.xml";
            _repository = new RestRateRepository();
            _parser = new XmlRateParser();
        }

        public async Task UpdateAsync()
        {
            string rawData = await _repository.GetAsync(_sourceUrl);
            _currentTable = _parser.Parse(rawData);
            _exchange = new Exchange(_currentTable);
        }

        public double Convert(double amount, ExchangeRate fromRate, ExchangeRate toRate)
        {
            if (_exchange == null)
            {
                throw new InvalidOperationException("Service has not been initialized. Call UpdateAsync first.");
            }
            return _exchange.ExchangeOne(amount, fromRate, toRate);
        }

        public List<ExchangeRate> GetAvailableRates()
        {
            if (_currentTable == null)
            {
                throw new InvalidOperationException("Service has not been initialized.");
            }
            return _currentTable.Rates;
        }

        public DateTime GetTableDate()
        {
            if (_currentTable == null)
            {
                throw new InvalidOperationException("Service has not been initialized.");
            }
            return _currentTable.Date;
        }

        public string GetTableNo()
        {
            if (_currentTable == null)
            {
                throw new InvalidOperationException("Service has not been initialized.");
            }
            return _currentTable.No;
        }
    }
}