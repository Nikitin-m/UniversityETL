using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityService.Domain;

public class University
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    
    public string Name { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public List<Site>? Sites { get; set; } = null!;

    //for ef
    protected internal University()
    {
    }

    public University(string name, string countryName, IEnumerable<string> sites)
    {
        Name = name;
        CountryName = countryName;
        Sites = sites.Select(x=> new Site(x)).ToList();
    }
}