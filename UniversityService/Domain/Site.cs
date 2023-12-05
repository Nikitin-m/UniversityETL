using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityService.Domain;

public class Site
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int UniversityId { get; set; }
    public University University { get; set; }

    public string StringUri { get; set; }

    protected internal Site()
    {
    }

    public Site(string stringUri)
    {
        StringUri = stringUri;
    }
}