using UniversityService.Application.Interfaces;
using UniversityService.Domain;
using UniversityService.Infrastructure.DAL;

namespace UniversityService.Infrastructure;

internal class UniversityRepository : IUniversityRepository
{
    private readonly UniversityDbContext _universityDbContext;

    public UniversityRepository(UniversityDbContext universityDbContext)
    {
        _universityDbContext = universityDbContext;
    }

    public async Task SaveUniversitiesAsync(ICollection<University> universities, CancellationToken cancellationToken)
    {
        await _universityDbContext.AddRangeAsync(universities, cancellationToken);
        await _universityDbContext.AddRangeAsync(universities.Where(x => x.Sites is not null).SelectMany(x => x.Sites),
            cancellationToken);
        await _universityDbContext.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<University> GetUniversityQuery()
    {
        return _universityDbContext.Universities;
    }
}