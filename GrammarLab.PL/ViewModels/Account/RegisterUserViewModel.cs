using GrammarLab.BLL.Entities;

namespace GrammarLab.PL.ViewModels;

public class RegisterUserViewModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}