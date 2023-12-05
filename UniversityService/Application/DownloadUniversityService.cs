using UniversityService.Application.Dto;
using UniversityService.Application.Interfaces;
using UniversityService.Domain;
using UniversityService.Infrastructure;

namespace UniversityService.Application;

internal class DownloadUniversityService : IDownloadUniversityService
{
    private static Lazy<SemaphoreSlim> _semaphoreSlim;
    
    private readonly ILogger<DownloadUniversityService>  _logger;
    private readonly IUniversityHttpClient _universityHttpClient;
    private readonly IUniversityRepository _universityRepository;
    private readonly int _concurrencyCount;
    private readonly string[] _defaultCountries;

    public DownloadUniversityService(ILogger<DownloadUniversityService> logger, IUniversityHttpClient universityHttpClient,
        IConfiguration configuration, IUniversityRepository universityRepository)
    {
        _logger = logger;
        _universityHttpClient = universityHttpClient;
        _universityRepository = universityRepository;
        
        _concurrencyCount = configuration.GetValue("ConcurrencyCount", 1);
        _semaphoreSlim = new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(_concurrencyCount), LazyThreadSafetyMode.ExecutionAndPublication);
        
        var defaultCountriesSection = configuration.GetSection("DefaultCountries");
        _defaultCountries = defaultCountriesSection.Get<string[]>();
    }

    public async Task DownloadAndSaveAsync(string[]? countryNames, CancellationToken cancellationToken)
    {
        countryNames ??= _defaultCountries;
        //параллельность для загрузки по странам
        //var universities = await WithDegreeOfParallelism(countryNames, cancellationToken);
        
        //или конкуретность для потоков?
        var universities = await WithConcurrency(countryNames, cancellationToken);

        await SaveUniversitiesToDb(universities, cancellationToken);
    }

    private async Task<IEnumerable<HipolabsResponseDto>> WithDegreeOfParallelism(string[] countryNames, CancellationToken cancellationToken)
    {
        var universityResponseDtos = countryNames
            .AsParallel()
            .WithDegreeOfParallelism(_concurrencyCount)
            .Select(async x =>  await _universityHttpClient.LoadUniversityAsync(x, cancellationToken))
            .SelectMany(x=> x.Result)
            .ToArray();
        return universityResponseDtos;
    }
    
    private async Task<IEnumerable<HipolabsResponseDto>> WithConcurrency(string[] countryNames, CancellationToken cancellationToken)
    {
        var universityResponseDtos = countryNames.AsParallel()
            .Select(async x => await LoadFromClient(x))
            .SelectMany(x=> x.Result)
            .ToArray();;
        return universityResponseDtos;

        async Task<IEnumerable<HipolabsResponseDto>?> LoadFromClient(string countryName)
        {
            await _semaphoreSlim.Value.WaitAsync(cancellationToken);
            var universities = await _universityHttpClient.LoadUniversityAsync(countryName, cancellationToken);
            _semaphoreSlim.Value.Release();
            return universities;
        }
    }

    private async Task SaveUniversitiesToDb(IEnumerable<HipolabsResponseDto> universityResponseDtos, CancellationToken cancellationToken)
    {
        var universityEntities = universityResponseDtos
            .Select(x => new University(x.Name, x.CountryName, x.Sites))
            .ToArray();

        await _universityRepository.SaveUniversitiesAsync(universityEntities, cancellationToken);
    }
}