using GrammarLab.BLL.Models;

namespace GrammarLab.BLL.Services;

public interface IAccountService
{
    Task<LoginResultDto> LoginAsync(LoginDto loginModel);
}