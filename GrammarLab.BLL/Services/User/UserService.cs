using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILevelService _levelService;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<User> userManager, IMapper mapper, ILevelService levelService, 
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _levelService = levelService;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> AddUserAsync(AddUserDto registerModel)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
        if (existingUser != null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = $"User with email {registerModel.Email} is already registered."
            });
        }

        var newUser = _mapper.Map<User>(registerModel);
        newUser.UserName = registerModel.Email;

        if (registerModel.LevelId is not null)
        {
            var levelError = await _levelService.ValidateLevelIdAsync(registerModel.LevelId.Value);
            if (levelError != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = levelError.Message
                });
            }
        }

        var result = await _userManager.CreateAsync(newUser, registerModel.Password);

        if (result.Succeeded)
        {
            await AddUserToRoleAsync(newUser, registerModel.Role.ToString());
        }

        return result;
    }

    public async Task<IdentityResult> ChangeUserPasswordAsync(string userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if(user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        return result;
    }

    public async Task<IdentityResult> DeleteUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var result = await _userManager.DeleteAsync(user);
        return result;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        var usersData = new List<UserDto>();

        foreach (var user in users)
        {
            var userData = _mapper.Map<UserDto>(user);

            userData.Roles = await _userManager.GetRolesAsync(user);

            if (user.LevelId != null)
            {
                userData.Level = await _levelService.GetLevelByIdAsync(user.LevelId.Value);
            }

            usersData.Add(userData);
        }

        return usersData;
    }

    public async Task<UserDto?> GetUserDataByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        var userData = _mapper.Map<UserDto>(user);
        userData.Roles = await _userManager.GetRolesAsync(user);

        if (user.LevelId != null)
        {
            userData.Level = await _levelService.GetLevelByIdAsync(user.LevelId.Value);
        }

        return userData;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(UserFilter filter)
    {
        IList<User> users;

        if (filter.UserRole.HasValue)
        {
            users = await _userManager.GetUsersInRoleAsync(filter.UserRole.ToString()!);
        } 
        else
        {
            users = await _userManager.Users.ToListAsync();
        }

        var usersData = new List<UserDto>();

        foreach (var user in users)
        {
            var userData = _mapper.Map<UserDto>(user);

            userData.Roles = await _userManager.GetRolesAsync(user);

            if (user.LevelId != null)
            {
                userData.Level = await _levelService.GetLevelByIdAsync(user.LevelId.Value);
            }

            usersData.Add(userData);
        }

        return usersData;
    }

    public async Task<Result<bool>> UpdateUserAsync(UpdateUserDto updatedUser)
    {
        var user = await _userManager.FindByIdAsync(updatedUser.Id);
        if (user == null)
        {
            return new Result<bool>(new Exception("User does to exist."));
        }

        if (updatedUser.LevelId is not null)
        {
            var levelError = await _levelService.ValidateLevelIdAsync(updatedUser.LevelId.Value);
            if (levelError != null)
            {
                return new Result<bool>(levelError);
            }
        }

        user.Email = updatedUser.Email;
        user.UserName = updatedUser.Email;
        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.LevelId = updatedUser.LevelId;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            await UpdateUserRoleAsync(user, updatedUser.Role.ToString());
        }

        return result.Succeeded;
    }

    private async Task AddUserToRoleAsync(User user, string role)
    {
        if (await _roleManager.RoleExistsAsync(role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, UserRole.Student.ToString());
        }
    }

    private async Task UpdateUserRoleAsync(User user, string role)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Contains(role))
        {
            var rolesToRemove = userRoles.Where(role => role != role);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            return;
        }

        await _userManager.RemoveFromRolesAsync(user, userRoles);

        await AddUserToRoleAsync(user, role);
    }
}