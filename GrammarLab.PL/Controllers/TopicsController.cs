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
public class TopicsController : ControllerBase
{
    private readonly ITopicService _topicService;
    private readonly IMapper _mapper;

    public TopicsController(ITopicService topicService, IMapper mapper)
    {
        _topicService = topicService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTopicById(int id)
    {
        var topic = await _topicService.GetByIdAsync(id);
        if(topic == null)
        {
            return NotFound(new { Error = new { Message = $"Topic with id={id} was not found" } });
        }

        return Ok(_mapper.Map<TopicViewModel>(topic));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchTopicsByName([FromQuery] string query)
    {
        var topics = await _topicService.SearchByNameAsync(query);
        return Ok(_mapper.Map<IEnumerable<TopicViewModel>>(topics));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpPost]
    public async Task<IActionResult> AddTopic([FromForm] AddTopicViewModel model)
    {
        var topic = _mapper.Map<AddTopicDto>(model);
        var result = await _topicService.AddAsync(topic);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTopic(int id, [FromForm] TopicViewModel model)
    {
        var topic = _mapper.Map<TopicDto>(model);
        var result = await _topicService.UpdateAsync(topic);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }

    [Authorize(Roles = "Admin, Teacher")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        var result = await _topicService.DeleteAsync(id);

        return result.Match<IActionResult>(
               value => Ok(value),
               err => BadRequest(new { Error = new { err.Message } }));
    }
}