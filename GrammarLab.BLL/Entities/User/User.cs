using Microsoft.AspNetCore.Identity;

namespace GrammarLab.BLL.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? LevelId { get; set; }
    public Level? Level { get; set; }
}