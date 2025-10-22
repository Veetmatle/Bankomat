using Bankomat.Models;

namespace Bankomat.Services.Interfaces
{
    // Parsowanie danych (xml, json)
    public interface IRateParser
    {
        ExchangeTable Parse(string rawData);
    }
}