using UniversityService.Application.Dto;

namespace UniversityService.Application.Interfaces;

public interface IUniversityUiLoaderService
{
    Task<IReadOnlyCollection<UniversityResponseDto>> GetUniversitiesByCountryNameOrNameAsync(string? countryName, string? name,
        CancellationToken cancellationToken);
}