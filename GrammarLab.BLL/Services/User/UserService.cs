using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GrammarLab.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILevelRepository _levelRepository;

    public UserService(UserManager<User> userManager, IMapper mapper, ILevelRepository levelRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _levelRepository = levelRepository;
    }

    public async Task<IEnumerable<UserDto>?> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        if (users == null)
        {
            return null;
        }

        if (!users.Any())
        {
            return new List<UserDto>();
        }

        var usersData = new List<UserDto>();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var userData = _mapper.Map<UserDto>(user);
            userData.Roles = userRoles;

            usersData.Add(userData);
        }

        return usersData;
    }

    public async Task<UserDto?> GetUserDataById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        if (user.LevelId != null)
        {
            user.Level = await _levelRepository.GetByIdAsync(user.LevelId.Value);
        }

        var userData = _mapper.Map<UserDto>(user);

        var roles = await _userManager.GetRolesAsync(user);
        userData.Roles = roles;

        return userData;
    }
}