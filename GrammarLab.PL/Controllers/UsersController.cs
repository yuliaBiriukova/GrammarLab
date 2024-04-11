using AutoMapper;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Services;
using GrammarLab.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrammarLab.PL.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilterViewModel filterModel)
    {
        var filter = _mapper.Map<UserFilter>(filterModel);
        var users = await _userService.GetUsersAsync(filter);
        var model = _mapper.Map<IEnumerable<UserViewModel>>(users);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromForm] AddUserViewModel model)
    {
        var registerDto = _mapper.Map<AddUserDto>(model);
        var result = await _userService.AddUserAsync(registerDto);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result.Succeeded);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserById(string userId)
    {
        var result = await _userService.DeleteUserByIdAsync(userId);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result.Succeeded);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromForm] UpdateUserViewModel model)
    {
        var user = _mapper.Map<UpdateUserDto>(model);
        var result = await _userService.UpdateUserAsync(user);

        return result.Match<IActionResult>(
              value => Ok(value),
              err => BadRequest(new { Error = new { err.Message } }));
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangeUserPassword(string userId, string password)
    {
        var result = await _userService.ChangeUserPasswordAsync(userId, password);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result.Succeeded);
    }
}