using Bankomat.Services.Interfaces;

namespace Bankomat.Services.Implementation
{
    public class RestRateRepository : IRateRepository
    {
        private static readonly HttpClient _httpClient;

        static RestRateRepository()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BankomatApp/1.0");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/xml, text/xml, */*");
        }

        public async Task<string> GetAsync(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source), "Source URL is empty");
            }

            try
            {
                var response = await _httpClient.GetAsync(source);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to download data from {source}. Error: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Request timeout - check your internet connection", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while downloading from {source}: {ex.Message}", ex);
            }
        }
    }
}