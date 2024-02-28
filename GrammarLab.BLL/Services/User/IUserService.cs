using GrammarLab.BLL.Models;

namespace GrammarLab.BLL.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>?> GetAllUsers();

    Task<UserDto?> GetUserDataById(string userId);
}