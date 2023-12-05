using Microsoft.AspNetCore.Mvc;
using UniversityService.Application.Interfaces;

namespace UniversityService.Controllers;

[ApiController]
[Route("[controller]")]
public class UniversityController : ControllerBase
{
    private readonly ILogger<UniversityController> _logger;
    private readonly IDownloadUniversityService _downloadUniversityService;
    private readonly IUniversityUiLoaderService _universityUiLoaderService;

    public UniversityController(ILogger<UniversityController> logger,
        IDownloadUniversityService downloadUniversityService, 
        IUniversityUiLoaderService universityUiLoaderService)
    {
        _logger = logger;
        _downloadUniversityService = downloadUniversityService;
        _universityUiLoaderService = universityUiLoaderService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> DownloadUniversities(string[]? countryNames, CancellationToken cancellationToken = default)
    {
        await _downloadUniversityService.DownloadAndSaveAsync(countryNames, cancellationToken);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetByCountryNameOrName(string? countryName, string? name, CancellationToken cancellationToken = default)
    {
        if (countryName is null && name is null)
            return BadRequest();

        var universities =
            await _universityUiLoaderService.GetUniversitiesByCountryNameOrNameAsync(countryName, name,
                cancellationToken);
        return new JsonResult(universities);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var universities = await _universityUiLoaderService.GetUniversitiesByCountryNameOrNameAsync(null, null, cancellationToken);
        return new JsonResult(universities);
    }
}