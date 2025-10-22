using Bankomat.Models;
using Bankomat.Services.Interfaces;

namespace Bankomat.Services.Implementation
{
    public class JsonRateParser : IRateParser
    {
        public ExchangeTable Parse(string rawData)
        {
            // TODO: Implementacja parsowania JSON (do przyszłego rozszerzenia)
            throw new NotImplementedException("JSON parsing is not implemented yet. Use XML parser.");
        }
    }
}