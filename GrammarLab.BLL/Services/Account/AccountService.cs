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
    private readonly JwtTokenOptions _tokenOptions;

    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenOptions = configuration.GetSection(nameof(JwtTokenOptions)).Get<JwtTokenOptions>()!;
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto loginModel)
    {
        var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            var token = await GenerateJwtTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            return new LoginResultDto { AccessToken = token, Success = true, Roles = roles };
        }
        return new LoginResultDto { Success = false, Error = "Invalid email or password." };
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