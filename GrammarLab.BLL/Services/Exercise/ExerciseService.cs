using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly ITopicService _topicService;
    private readonly IMapper _mapper;

    public ExerciseService(IExerciseRepository exerciseRepository, ITopicService topicService, IMapper mapper)
    {
        _exerciseRepository = exerciseRepository;
        _topicService = topicService;
        _mapper = mapper;
    }

    public async Task<Result<int>> AddAsync(AddExerciseDto exercise)
    {
        var validationError = await _topicService.ValidateTopicIdAsync(exercise.TopicId);
        if (validationError != null)
        {
            return new Result<int>(validationError);
        }

        var newExercise = _mapper.Map<Exercise>(exercise);
        var id = await _exerciseRepository.AddAsync(newExercise);
        return id;
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var validationError = await ValidateExerciseIdAsync(id); 
        if (validationError != null) 
        {
            return new Result<bool>(validationError);
        }

        var isDeleted = await _exerciseRepository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<ExerciseDto?> GetByIdAsync(int id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        return _mapper.Map<ExerciseDto>(exercise);
    }

    public async Task<IEnumerable<ExerciseDto>> GetByTopicIdAsync(int topicId)
    {
        var exercises = await _exerciseRepository.GetByTopicIdAsync(topicId);
        return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
    }

    public async Task<Result<bool>> UpdateAsync(ExerciseDto exercise)
    {
        var validationError = await ValidateExerciseAsync(exercise);
        if (validationError != null)
        {
            return new Result<bool>(validationError);
        }

        var updatedExercise = _mapper.Map<Exercise>(exercise);
        var isUpdated = await _exerciseRepository.UpdateAsync(updatedExercise);
        return isUpdated;
    }

    private async Task<ValidationException?> ValidateExerciseIdAsync(int id)
    {
        var exerciseExists = await _exerciseRepository.CheckExistsAsync(id);
        if (!exerciseExists)
        {
            return new ValidationException($"Exercise with id={id} does not exist.");
        }

        return null;
    }

    private async Task<ValidationException?> ValidateExerciseAsync(ExerciseDto exercise)
    {
        var idValidationError = await ValidateExerciseIdAsync(exercise.Id);
        if (idValidationError != null)
        {
            return idValidationError;
        }

        return await _topicService.ValidateTopicIdAsync(exercise.TopicId);
    }
}