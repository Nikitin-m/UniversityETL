using UniversityService.Application.Dto;

namespace UniversityService.Application.Interfaces;

public interface IUniversityHttpClient
{
    Task<IEnumerable<HipolabsResponseDto>> LoadUniversityAsync(string countryName, CancellationToken cancellationToken);
}