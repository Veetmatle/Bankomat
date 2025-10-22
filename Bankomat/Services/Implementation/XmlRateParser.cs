using System.Globalization;
using System.Xml.Linq;
using Bankomat.Models;
using Bankomat.Services.Interfaces;

namespace Bankomat.Services.Implementation
{
    public class XmlRateParser : IRateParser
    {
        public ExchangeTable Parse(string rawData)
        {
            if (string.IsNullOrWhiteSpace(rawData))
            {
                throw new ArgumentException("Failed to pass XML data");
            }

            try
            {
                XDocument xmlDoc = XDocument.Parse(rawData);

                XElement root = xmlDoc.Element("tabela_kursow");

                string no = root.Element("numer_tabeli").Value;

                DateTime date = DateTime.Parse(root.Element("data_publikacji").Value);

                List<ExchangeRate> rates = root.Elements("pozycja").Select(x => new ExchangeRate(
                    rate: double.Parse(x.Element("kurs_sredni").Value, new CultureInfo("pl-PL")),
                    multiplier: double.Parse(x.Element("przelicznik").Value, CultureInfo.InvariantCulture),
                    code: x.Element("kod_waluty").Value,
                    name: x.Element("nazwa_waluty").Value 
                )).ToList();

                rates.Add(new ExchangeRate(1.0, 1.0, "PLN", "polski zloty"));

                return new ExchangeTable(date, no, rates);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to parse XML exchange rate data.", ex);
            }
        }
    }
}