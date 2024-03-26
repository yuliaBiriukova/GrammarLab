namespace GrammarLab.BLL.Models;

public class UserDto
{
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public int LevellId { get; set; }
    public LevelDto? Level{ get; set; }
    public IEnumerable<string> Roles { get; set; }
}