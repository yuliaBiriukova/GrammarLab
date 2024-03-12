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
[Authorize]
public class CompletedTestsController : ControllerBase
{
    private readonly ICompletedTestService _completedTestService;
    private readonly IMapper _mapper;

    public CompletedTestsController(ICompletedTestService completedTestService, IMapper mapper)
    {
        _completedTestService = completedTestService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompletedTests([FromQuery] CompletedTestFilterViewModel filterModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var filter = _mapper.Map<CompletedTestFilter>(filterModel);
        var completedTests = await _completedTestService.GetCompletedTestsAsync(filter, userId);
        return Ok(_mapper.Map<IEnumerable<CompletedTestViewModel>>(completedTests));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompletedTestById(int id)
    {
        var completedTest = await _completedTestService.GetByIdWithExercisesAsync(id);
        if (completedTest == null)
        {
            return NotFound(new { Error = new { Message = $"Completed test with id={id} was not found" } });
        }

        return Ok(_mapper.Map<CompletedTestViewModel>(completedTest));
    }

    [HttpGet("last")]
    public async Task<IActionResult> GetLastCompletedTestsByLevelId([FromQuery] CompletedTestFilterViewModel filterModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var filter = _mapper.Map<CompletedTestFilter>(filterModel);
        var completedTests = await _completedTestService.GetLastTopicsResultsAsync(filter, userId);
        return Ok(_mapper.Map<IEnumerable<CompletedTestViewModel>>(completedTests));
    }

    [HttpGet("best")]
    public async Task<IActionResult> GetBestCompletedTestsByLevelId([FromQuery] CompletedTestFilterViewModel filterModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var filter = _mapper.Map<CompletedTestFilter>(filterModel);
        var completedTests = await _completedTestService.GetBestTopicsResultsAsync(filter, userId);
        return Ok(_mapper.Map<IEnumerable<CompletedTestViewModel>>(completedTests));
    }

    [HttpPost]
    public async Task<IActionResult> AddCompletedTest(AddCompletedTestViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var completedTest = _mapper.Map<AddCompletedTestDto>(model);
        var result = await _completedTestService.AddAsync(completedTest, userId);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompletedTest(int id)
    {
        var result = await _completedTestService.DeleteAsync(id);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }
}