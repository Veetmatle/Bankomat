using Bankomat.Models;

namespace Bankomat.Services.Interfaces
{
    // Kontakt z serwisem do wymiany walut
    public interface IExchangeService
    {
        Task UpdateAsync();
        double Convert(double amount, ExchangeRate fromRate, ExchangeRate toRate);
        List<ExchangeRate> GetAvailableRates();
        DateTime GetTableDate();
        string GetTableNo();
    }
}