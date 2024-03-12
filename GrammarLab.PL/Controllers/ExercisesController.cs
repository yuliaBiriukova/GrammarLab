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
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    private readonly IMapper _mapper;

    public ExercisesController(IExerciseService exerciseService, IMapper mapper)
    {
        _exerciseService = exerciseService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExerciseById(int id)
    {
        var exercise =  await _exerciseService.GetByIdAsync(id);
        if (exercise == null)
        {
            return NotFound(new { Error = new { Message = $"Exerise with id={id} was not found" } });
        }

        return Ok(_mapper.Map<ExerciseViewModel>(exercise));
    }

    [HttpGet]
    public async Task<IActionResult> GetExercisesByTopicId(int topicId)
    {
        var exercises = await _exerciseService.GetByTopicIdAsync(topicId);
        return Ok(_mapper.Map<IEnumerable<ExerciseViewModel>>(exercises));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost]
    public async Task<IActionResult> AddExercise(AddExerciseViewModel model)
    {
        var exercise = _mapper.Map<AddExerciseDto>(model);
        var result = await _exerciseService.AddAsync(exercise);

        return result.Match<IActionResult>(
              value => Ok(value),
              err => BadRequest(new { Error = new { err.Message } }));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExercise(int id, [FromBody] ExerciseViewModel model)
    {
        var exercise = _mapper.Map<ExerciseDto>(model);
        var result = await _exerciseService.UpdateAsync(exercise);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        var result = await _exerciseService.DeleteAsync(id);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }
}