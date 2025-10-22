using Bankomat.Services.Implementation;
using Bankomat.UI;

namespace Bankomat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Bankomat - Kantor Walutowy";

            try
            {
                var exchangeService = ExchangeService.Instance;
                
                Console.WriteLine("=== BANKOMAT - KANTOR WALUTOWY ===");
                Console.WriteLine("Inicjalizacja systemu...\n");
                
                await exchangeService.UpdateAsync();
                
                Console.WriteLine($"Kursy aktualne na dzień: {exchangeService.GetTableDate():yyyy-MM-dd}");
                Console.WriteLine($"Numer tabeli NBP: {exchangeService.GetTableNo()}\n");
                
                var menuManager = new MenuManager(exchangeService);
                await menuManager.RunAsync();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Błąd krytyczny aplikacji: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}