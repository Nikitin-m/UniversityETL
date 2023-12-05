using UniversityService.Domain;

namespace UniversityService.Application.Interfaces;

public interface IUniversityRepository
{
    Task SaveUniversitiesAsync(ICollection<University> universities, CancellationToken cancellationToken);

    IQueryable<University> GetUniversityQuery();
}