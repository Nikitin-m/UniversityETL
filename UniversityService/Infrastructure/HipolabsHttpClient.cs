using UniversityService.Application.Dto;
using UniversityService.Application.Interfaces;

namespace UniversityService.Infrastructure;

internal class HipolabsHttpClient : IUniversityHttpClient
{
    private const string SearchPostfix = "/search?country=";
    
    private readonly ILogger<HipolabsHttpClient> _logger;
    private readonly HttpClient _httpClient;

    public HipolabsHttpClient(ILogger<HipolabsHttpClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<HipolabsResponseDto>> LoadUniversityAsync(string countryName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start loading universities for country with name '{countryName}'", countryName);

        var universities =
            await _httpClient.GetFromJsonAsync<HipolabsResponseDto[]>(SearchPostfix + countryName,
                cancellationToken) ?? Array.Empty<HipolabsResponseDto>();
        
        _logger.LogInformation("Found {count} universities with country name '{countryName}'", universities.Count(), countryName);
        return universities;
    }
}