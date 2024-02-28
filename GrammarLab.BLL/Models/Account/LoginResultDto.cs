namespace GrammarLab.BLL.Models;

public class LoginResultDto
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public string AccessToken { get; set; }
}