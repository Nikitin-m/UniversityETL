using Microsoft.AspNetCore.Mvc;
using UniversityService.Infrastructure.DAL;

namespace UniversityService.Controllers;

[ApiController]
[Route("[controller]")]
public class SystemController : ControllerBase
{
    private readonly UniversityDbContext _universityDbContext;

    public SystemController(UniversityDbContext universityDbContext)
    {
        _universityDbContext = universityDbContext;
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> ClearAllTables(CancellationToken cancellationToken)
    {
        foreach (var entity in _universityDbContext.Universities)
        {
            _universityDbContext.Universities.Remove(entity);
            await _universityDbContext.SaveChangesAsync(cancellationToken);
        }
        
        return Ok();
    }
}