using Bankomat.Models;
using Bankomat.Services.Interfaces;
using Bankomat.Validators;

namespace Bankomat.UI
{
    public class MenuManager
    {
        private readonly IExchangeService _exchangeService;
        private bool _isRunning;

        public MenuManager(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
            _isRunning = true;
        }

        public async Task RunAsync()
        {
            while (_isRunning)
            {
                ShowMainMenu();
                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        await PerformExchangeAsync();
                        break;
                    case "2":
                        ShowAvailableCurrencies();
                        break;
                    case "3":
                        await RefreshRatesAsync();
                        break;
                    case "4":
                        ShowAbout();
                        break;
                    case "0":
                        Exit();
                        break;
                    default:
                        ConsoleHelper.PrintError("Nieprawidłowy wybór. Spróbuj ponownie.");
                        ConsoleHelper.WaitForKey();
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            ConsoleHelper.PrintHeader("BANKOMAT - KANTOR WALUTOWY");
            
            Console.WriteLine("1. Wymień walutę");
            Console.WriteLine("2. Pokaż dostępne waluty");
            Console.WriteLine("3. Odśwież kursy walut");
            Console.WriteLine("4. Informacje");
            Console.WriteLine("0. Wyjście");
            
            ConsoleHelper.PrintSeparator();
            Console.Write("Wybierz opcję: ");
        }

        private async Task PerformExchangeAsync()
        {
            ConsoleHelper.PrintHeader("WYMIANA WALUTY");

            var availableRates = _exchangeService.GetAvailableRates();

            // Wybór waluty źródłowej
            ConsoleHelper.PrintInfo("Wybór waluty źródłowej:");
            string fromCurrencyCode = ConsoleHelper.ReadLine("Podaj kod waluty (np. USD, EUR, PLN): ").ToUpper();

            if (!CurrencyValidator.TryGetRateByCode(fromCurrencyCode, availableRates, out ExchangeRate? fromRate))
            {
                ConsoleHelper.PrintError($"Nieprawidłowy kod waluty: {fromCurrencyCode}");
                ConsoleHelper.WaitForKey();
                return;
            }

            // Wybór waluty docelowej
            ConsoleHelper.PrintInfo("Wybór waluty docelowej:");
            string toCurrencyCode = ConsoleHelper.ReadLine("Podaj kod waluty (np. USD, EUR, PLN): ").ToUpper();

            if (!CurrencyValidator.TryGetRateByCode(toCurrencyCode, availableRates, out ExchangeRate? toRate))
            {
                ConsoleHelper.PrintError($"Nieprawidłowy kod waluty: {toCurrencyCode}");
                ConsoleHelper.WaitForKey();
                return;
            }

            // Sprawdzenie czy waluty nie są takie same
            if (fromCurrencyCode == toCurrencyCode)
            {
                ConsoleHelper.PrintWarning("Waluta źródłowa i docelowa są takie same. Brak potrzeby wymiany.");
                ConsoleHelper.WaitForKey();
                return;
            }

            // Wprowadzenie kwoty
            ConsoleHelper.PrintInfo("Podaj kwotę do wymiany:");
            string amountInput = ConsoleHelper.ReadLine($"Kwota w {fromCurrencyCode}: ");

            if (!AmountValidator.IsValidAmount(amountInput, out double amount))
            {
                ConsoleHelper.PrintError(AmountValidator.GetValidationErrorMessage(amountInput));
                ConsoleHelper.WaitForKey();
                return;
            }

            // Wykonanie wymiany
            try
            {
                double result = _exchangeService.Convert(amount, fromRate!, toRate!);

                ConsoleHelper.PrintSeparator();
                ConsoleHelper.PrintSuccess("Wymiana zakończona pomyślnie!");
                Console.WriteLine();
                Console.WriteLine($"Kwota źródłowa:  {amount:F2} {fromCurrencyCode}");
                Console.WriteLine($"Kwota wynikowa:  {result:F2} {toCurrencyCode}");
                Console.WriteLine();
                Console.WriteLine($"Kurs {fromCurrencyCode}/PLN: {fromRate!.Rate / fromRate.Multiplier:F4}");
                Console.WriteLine($"Kurs {toCurrencyCode}/PLN: {toRate!.Rate / toRate.Multiplier:F4}");
                Console.WriteLine($"Kurs {fromCurrencyCode}/{toCurrencyCode}: {result / amount:F4}");
                ConsoleHelper.PrintSeparator();
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Błąd podczas wymiany: {ex.Message}");
            }

            ConsoleHelper.WaitForKey();
        }

        private void ShowAvailableCurrencies()
        {
            ConsoleHelper.PrintHeader("DOSTĘPNE WALUTY");

            try
            {
                var rates = _exchangeService.GetAvailableRates();
                var tableDate = _exchangeService.GetTableDate();
                var tableNo = _exchangeService.GetTableNo();

                Console.WriteLine($"Data tabeli: {tableDate:yyyy-MM-dd}");
                Console.WriteLine($"Numer tabeli: {tableNo}");
                ConsoleHelper.PrintSeparator();
                Console.WriteLine();

                Console.WriteLine($"{"Kod",-8} {"Nazwa waluty",-30} {"Kurs średni",-15} {"Przelicznik",-12}");
                ConsoleHelper.PrintSeparator();

                foreach (var rate in rates.OrderBy(r => r.Code))
                {
                    Console.WriteLine($"{rate.Code,-8} {rate.Name,-30} {rate.Rate / rate.Multiplier,15:F4} {rate.Multiplier,12:F0}");
                }

                Console.WriteLine();
                ConsoleHelper.PrintInfo($"Łącznie dostępnych walut: {rates.Count}");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Błąd podczas pobierania listy walut: {ex.Message}");
            }

            ConsoleHelper.WaitForKey();
        }

        private async Task RefreshRatesAsync()
        {
            ConsoleHelper.PrintHeader("ODŚWIEŻANIE KURSÓW WALUT");

            try
            {
                ConsoleHelper.PrintInfo("Pobieranie aktualnych kursów z NBP...");
                await _exchangeService.UpdateAsync();
                
                var tableDate = _exchangeService.GetTableDate();
                var tableNo = _exchangeService.GetTableNo();

                ConsoleHelper.PrintSuccess("Kursy zostały zaktualizowane!");
                Console.WriteLine($"Data tabeli: {tableDate:yyyy-MM-dd}");
                Console.WriteLine($"Numer tabeli: {tableNo}");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Błąd podczas odświeżania kursów: {ex.Message}");
            }

            ConsoleHelper.WaitForKey();
        }

        private void ShowAbout()
        {
            ConsoleHelper.PrintHeader("INFORMACJE O APLIKACJI");

            Console.WriteLine("Aplikacja: Bankomat - Kantor Walutowy");
            Console.WriteLine("Wersja: 1.0.0");
            Console.WriteLine();
            Console.WriteLine("Opis:");
            Console.WriteLine("  Aplikacja konsolowa do wymiany walut na podstawie");
            Console.WriteLine("  aktualnych kursów średnich NBP (Narodowy Bank Polski).");
            Console.WriteLine();
            Console.WriteLine("Źródło danych:");
            Console.WriteLine("  https://www.nbp.pl/kursy/xml/lasta.xml");
            Console.WriteLine();
            Console.WriteLine("Technologie:");
            Console.WriteLine("  - .NET 8.0");
            Console.WriteLine("  - C# 12");
            Console.WriteLine("  - Wzorzec projektowy: Singleton");
            Console.WriteLine("  - Architektura: Warstwowa (OOP)");

            ConsoleHelper.WaitForKey();
        }

        private void Exit()
        {
            ConsoleHelper.PrintHeader("WYJŚCIE");
            ConsoleHelper.PrintInfo("Dziękujemy za skorzystanie z aplikacji Bankomat!");
            Console.WriteLine("Do widzenia! 👋");
            _isRunning = false;
        }
    }
}