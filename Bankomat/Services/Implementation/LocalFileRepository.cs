using Bankomat.Services.Interfaces;

namespace Bankomat.Services.Implementation
{
    public class LocalFileRepository : IRateRepository
    {
        public async Task<string> GetAsync(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source), "File path is empty");
            }

            if (!File.Exists(source))
            {
                throw new FileNotFoundException($"File not found: {source}");
            }

            try
            {
                return await File.ReadAllTextAsync(source);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read file from {source}", ex);
            }
        }
    }
}