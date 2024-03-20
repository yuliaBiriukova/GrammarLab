using AutoMapper;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Services;
using GrammarLab.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrammarLab.PL.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LevelsController : ControllerBase
{
    private readonly ILevelService _levelService;
    private readonly IMapper _mapper;

    public LevelsController(ILevelService levelService, IMapper mapper)
    {
        _levelService = levelService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLevels()
    {
        var levels = await _levelService.GetAllLevelsAsync();
        return Ok(_mapper.Map<IEnumerable<LevelViewModel>>(levels));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLevelById(int id)
    {
        var level = await _levelService.GetLevelByIdAsync(id);
        if (level == null)
        {
            return NotFound(new { Error = new { Message = $"Level with id={id} was not found" } });
        }

        return Ok(_mapper.Map<LevelViewModel>(level));
    }

    [HttpGet("{id}/topics")]
    public async Task<IActionResult> GetLevelByIdWithTopics(int id)
    {
        var level = await _levelService.GetLevelByIdWithTopicsAsync(id);
        if (level == null)
        {
            return NotFound(new { Error = new { Message = $"Level with id={id} was not found" } });
        }

        return Ok(_mapper.Map<LevelViewModel>(level));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost]
    public async Task<IActionResult> AddLevel([FromForm] AddLevelViewModel model)
    {
        var level = _mapper.Map<AddLevelDto>(model);
        var result = await _levelService.AddLevelAsync(level);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLevel(int id, [FromForm] UpdateLevelViewModel model)
    {
        var level = _mapper.Map<LevelDto>(model);
        var result = await _levelService.UpdateLevelAsync(level);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLevel(int id)
    {
        var result = await _levelService.DeleteLevelAsync(id);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }
}