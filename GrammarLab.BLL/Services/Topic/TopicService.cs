using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly ILevelService _levelService;
    private readonly IMapper _mapper;

    public TopicService(ITopicRepository topicRepository, ILevelService levelService, IMapper mapper)
    {
        _topicRepository = topicRepository;
        _levelService = levelService;
        _mapper = mapper;
    }

    public async Task<Result<int>> AddAsync(AddTopicDto topic)
    {
        var validationError = await _levelService.ValidateLevelIdAsync(topic.LevelId);
        if (validationError != null)
        {
            return new Result<int>(validationError);
        }

        var newTopic = _mapper.Map<Topic>(topic);
        var id = await _topicRepository.AddAsync(newTopic);
        return id;
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var validationError = await ValidateTopicIdAsync(id);
        if(validationError != null)
        {
            return new Result<bool>(validationError);
        }

        var isDeleted = await _topicRepository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<TopicDto?> GetByIdAsync(int id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        return _mapper.Map<TopicDto>(topic);
    }

    public async Task<IEnumerable<TopicDto>> GetByLevelIdAsync(int levelId)
    {
        var topics =  await _topicRepository.GetByLevelIdAsync(levelId);
        return _mapper.Map<IEnumerable<TopicDto>>(topics);
    }

    public async Task<Result<bool>> UpdateAsync(TopicDto topic)
    {
        var validationError = await ValidateTopicAsync(topic);
        if(validationError != null)
        {
            return new Result<bool>(validationError);
        }

        var updatedTopic = _mapper.Map<Topic>(topic);
        var isUpdated = await _topicRepository.UpdateAsync(updatedTopic);
        return isUpdated;
    }

    public async Task<ValidationException?> ValidateTopicIdAsync(int id)
    {
        var topicExists = await _topicRepository.CheckExistsAsync(id);
        if(!topicExists)
        {
            return new ValidationException($"Topic with id={id} does not exist.");
        }

        return null;
    }

    private async Task<ValidationException?> ValidateTopicAsync(TopicDto topic)
    {
        var idValidationError = await ValidateTopicIdAsync(topic.Id);
        if (idValidationError != null)
        {
            return idValidationError;
        }

        return await _levelService.ValidateLevelIdAsync(topic.LevelId);
    }
}