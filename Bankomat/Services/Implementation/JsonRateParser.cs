using Bankomat.Models;
using Bankomat.Services.Interfaces;

namespace Bankomat.Services.Implementation
{
    public class JsonRateParser : IRateParser
    {
        public ExchangeTable Parse(string rawData)
        {
            // Alternatywnie do implementacji jakby zmienili z xmla na json
            throw new NotImplementedException("JSON parsing is not implemented yet. Use XML parser.");
        }
    }
}