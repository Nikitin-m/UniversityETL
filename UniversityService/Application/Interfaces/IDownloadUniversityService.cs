namespace UniversityService.Application.Interfaces;

public interface IDownloadUniversityService
{
    Task DownloadAndSaveAsync(string[]? countryNames, CancellationToken cancellationToken);
}