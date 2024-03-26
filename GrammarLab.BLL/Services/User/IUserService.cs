using GrammarLab.BLL.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;

namespace GrammarLab.BLL.Services;

public interface IUserService
{
    Task<IdentityResult> AddUserAsync(AddUserDto registerModel);

    Task<IdentityResult> ChangeUserPasswordAsync(string userId, string password);

    Task<IdentityResult> DeleteUserByIdAsync(string userId);

    Task<IEnumerable<UserDto>?> GetAllUsersAsync();

    Task<UserDto?> GetUserDataByIdAsync(string userId);

    Task<Result<bool>> UpdateUserAsync(UpdateUserDto updatedUser);
}