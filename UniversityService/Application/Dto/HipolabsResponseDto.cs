using System.Text.Json.Serialization;

namespace UniversityService.Application.Dto;

public readonly record struct HipolabsResponseDto
{
    [JsonPropertyName("country")]
    public string CountryName { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("web_pages")]
    public string[] Sites { get; init; }
}