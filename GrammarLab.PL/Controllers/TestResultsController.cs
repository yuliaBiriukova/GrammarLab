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
public class TestResultsController : ControllerBase
{
    private readonly ITestResultService _testResultService;
    private readonly IMapper _mapper;

    public TestResultsController(ITestResultService testResultService, IMapper mapper)
    {
        _testResultService = testResultService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTestResults([FromQuery] TestResultFilterViewModel filterModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var filter = _mapper.Map<TestResultFilter>(filterModel);
        var testResults = await _testResultService.GetTestResultsAsync(filter, userId);
        return Ok(_mapper.Map<IEnumerable<TestResultViewModel>>(testResults));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTestResultById(int id)
    {
        var testResult = await _testResultService.GetByIdWithExercisesAsync(id);
        return Ok(_mapper.Map<TestResultViewModel>(testResult));
    }

    [HttpGet("last")]
    public async Task<IActionResult> GetLastTestResultsByLevelId([FromQuery] TestResultFilterViewModel filterModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var filter = _mapper.Map<TestResultFilter>(filterModel);
        var testResults = await _testResultService.GetLastTopicsTestResultsAsync(filter, userId);
        return Ok(_mapper.Map<IEnumerable<TestResultViewModel>>(testResults));
    }

    [HttpGet("best")]
    public async Task<IActionResult> GetBestTestResultsByLevelId([FromQuery] TestResultFilterViewModel filterModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var filter = _mapper.Map<TestResultFilter>(filterModel);
        var testResults = await _testResultService.GetBestTopicsTestResultsAsync(filter, userId);
        return Ok(_mapper.Map<IEnumerable<TestResultViewModel>>(testResults));
    }

    [HttpPost]
    public async Task<IActionResult> AddTestResult(AddTestResultViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var testResult = _mapper.Map<AddTestResultDto>(model);
        var result = await _testResultService.AddTestResultAsync(testResult, userId);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestResult(int id)
    {
        var result = await _testResultService.DeleteTestResultAsync(id);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }
}