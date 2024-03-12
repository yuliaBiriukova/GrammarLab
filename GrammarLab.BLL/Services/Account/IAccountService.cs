using GrammarLab.BLL.Models;
using Microsoft.AspNetCore.Identity;

namespace GrammarLab.BLL.Services;

public interface IAccountService
{
    Task<IdentityResult> RegisterAsync(RegisterUserDto registerModel);

    Task<LoginResultDto> LoginAsync(LoginDto loginModel);

    Task<IdentityResult> DeleteUserByEmailAsync(string email);
}