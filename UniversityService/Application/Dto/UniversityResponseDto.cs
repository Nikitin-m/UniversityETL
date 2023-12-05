namespace UniversityService.Application.Dto;

public readonly record struct UniversityResponseDto
{
    public string CountryName { get; init; }
    
    public string Name { get; init; }

    public string Sites { get; init; }
}