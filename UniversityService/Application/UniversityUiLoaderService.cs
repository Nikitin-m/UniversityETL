using Microsoft.EntityFrameworkCore;
using UniversityService.Application.Dto;
using UniversityService.Application.Interfaces;
using UniversityService.Domain;
using UniversityService.Infrastructure;

namespace UniversityService.Application;

internal class UniversityUiLoaderService : IUniversityUiLoaderService
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityUiLoaderService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }
    
    public async Task<IReadOnlyCollection<UniversityResponseDto>> GetUniversitiesByCountryNameOrNameAsync(string? countryName, string? name, CancellationToken cancellationToken)
    {
        IQueryable<University> universityQuery = _universityRepository.GetUniversityQuery()
            .AsNoTracking()
            .Include(x => x.Sites);

        if (countryName is not null)
            universityQuery = universityQuery.Where(x => x.CountryName == countryName);
        
        if (name is not null)
            universityQuery = universityQuery.Where(x => x.Name == name);

        var universities = await universityQuery.ToListAsync(cancellationToken);
        return ConvertToResponseDto(universities);
    }

    private static List<UniversityResponseDto> ConvertToResponseDto(IEnumerable<University> universities)
    {
        return universities.Select(x => new UniversityResponseDto()
        {
            Name = x.Name,
            CountryName = x.CountryName,
            Sites = x.Sites is null? string.Empty : string.Join(";", x.Sites.Select(x=>x.StringUri))
        }).ToList();
    }
}
