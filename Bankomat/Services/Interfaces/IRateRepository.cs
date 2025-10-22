namespace Bankomat.Services.Interfaces
{
    // pobieranie danych (api/plik)
    public interface IRateRepository
    {
        Task<string> GetAsync(string source);
    }
}