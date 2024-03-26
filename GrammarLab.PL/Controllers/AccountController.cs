using AutoMapper;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Services;
using GrammarLab.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrammarLab.PL.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AccountController(IAccountService accountService, IUserService userService, IMapper mapper)
    {
        _accountService = accountService;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        var loginDto = _mapper.Map<LoginDto>(model);
        var result = await _accountService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return BadRequest(new { Error = new { Message = result.Error } });
        }

        return Ok(new { result.AccessToken, result.Roles });
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentAccountData()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(userId == null)
        {
            return BadRequest(new { Error = new { Message = "UserId was not found" } });
        }

        var userData = await _userService.GetUserDataByIdAsync(userId);

        if(userData == null)
        {
            return BadRequest(new { Error = new { Message = "User was not found" } });
        }

        var model = _mapper.Map<UserViewModel>(userData);
        return Ok(model);
    }
}