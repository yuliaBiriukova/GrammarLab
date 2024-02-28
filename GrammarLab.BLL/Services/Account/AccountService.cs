using AutoMapper;
using GrammarLab.BLL.Configurations;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrammarLab.BLL.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtTokenOptions _tokenOptions;
    private readonly IMapper _mapper;

    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, 
        RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenOptions = configuration.GetSection(nameof(JwtTokenOptions)).Get<JwtTokenOptions>()!;
        _mapper = mapper;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterUserDto registerModel)
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

        var result = await _userManager.CreateAsync(newUser, registerModel.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        await AddUserToRoleAsync(newUser, registerModel.Role.ToString());

        return result;
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto loginModel)
    {
        var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            var token = await GenerateJwtTokenAsync(user);
            return new LoginResultDto { AccessToken = token, Success = true };
        }
        return new LoginResultDto { Success = false, Error = "Invalid email or password." };
    }

    public async Task<IdentityResult> DeleteUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var result = await _userManager.DeleteAsync(user);
        return result;
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

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenOptions.SecretKey);

        var roles = await _userManager.GetRolesAsync(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, string.Join(",", roles)),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = _tokenOptions.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}