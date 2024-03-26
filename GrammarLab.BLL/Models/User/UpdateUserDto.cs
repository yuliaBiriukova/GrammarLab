using GrammarLab.BLL.Entities;

namespace GrammarLab.BLL.Models;

public class UpdateUserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? LevelId { get; set; }
}