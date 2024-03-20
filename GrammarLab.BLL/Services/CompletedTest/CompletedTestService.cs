using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public class CompletedTestService : ICompletedTestService
{
    private readonly ICompletedTestRepository _completedTestRepository;
    private readonly ITopicService _topicService;
    private readonly IExerciseService _exerciseService;
    private readonly IMapper _mapper;

    public CompletedTestService(ICompletedTestRepository completedTestRepository, ITopicService topicService, 
        IExerciseService exerciseService, IMapper mapper)
    {
        _completedTestRepository = completedTestRepository;
        _topicService = topicService;
        _exerciseService = exerciseService;
        _mapper = mapper;
    }

    public async Task<Result<int>> AddCompletedtTestAsync(AddCompletedTestDto completedTest, string userId)
    {
        var topicIdValidationError = await _topicService.ValidateTopicIdAsync(completedTest.TopicId);
        if (topicIdValidationError != null)
        {
            return new Result<int>(topicIdValidationError);
        }

        var topicExercises = await _exerciseService.GetByTopicIdAsync(completedTest.TopicId);

        var exercisesValidationError = ValidateCompletedTestExercisesAsync(completedTest.CompletedTestExercises, topicExercises);
        if (exercisesValidationError != null)
        {
            return new Result<int>(exercisesValidationError);
        }

        foreach (var completedTestExercise in completedTest.CompletedTestExercises)
        {
            var topicExercise = topicExercises.FirstOrDefault(e => e.Id == completedTestExercise.ExerciseId);
            if(topicExercise != null)
            {
                completedTestExercise.Task = topicExercise.Task;
                completedTestExercise.Answer = topicExercise.Answer;
            }
        }

        var newCompletedTest = _mapper.Map<CompletedTest>(completedTest);
        newCompletedTest.UserId = userId;
        newCompletedTest.Percentage = CheckTestAnswersAsync(completedTest.CompletedTestExercises);
        newCompletedTest.DateCompleted = DateTime.Now;

        var id = await _completedTestRepository.AddAsync(newCompletedTest);
        return id;
    }

    public async Task<Result<bool>> DeleteCompletedTestAsync(int id)
    {
        var validationError = await ValidateCompletedTestIdAsync(id);
        if (validationError != null)
        {
            return new Result<bool>(validationError);
        }

        var isDeleted = await _completedTestRepository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<CompletedTestDto?> GetByIdWithExercisesAsync(int id)
    {
        var completedTest = await _completedTestRepository.GetByIdWithExercisesAsync(id);
        return _mapper.Map<CompletedTestDto>(completedTest);
    }

    public async Task<IEnumerable<CompletedTestDto>> GetCompletedTestsAsync(CompletedTestFilter filter, string userId)
    {
        filter.UserId = userId;
        var completedTests = await _completedTestRepository.GetAsync(filter);
        return _mapper.Map<IEnumerable<CompletedTestDto>>(completedTests);
    }

    public async Task<IEnumerable<CompletedTestDto>> GetLastTopicsResultsAsync(CompletedTestFilter filter, string userId)
    {
        filter.UserId = userId;
        var completedTests = await _completedTestRepository.GetLastTopicsResultsAsync(filter);
        return _mapper.Map<IEnumerable<CompletedTestDto>>(completedTests);
    }

    public async Task<IEnumerable<CompletedTestDto>> GetBestTopicsResultsAsync(CompletedTestFilter filter, string userId)
    {
        filter.UserId = userId;
        var completedTests = await _completedTestRepository.GetBestTopicsResultsAsync(filter);
        return _mapper.Map<IEnumerable<CompletedTestDto>>(completedTests);
    }

    private async Task<ValidationException?> ValidateCompletedTestIdAsync(int id)
    {
        var completedTestExists = await _completedTestRepository.CheckExistsAsync(id);
        if (!completedTestExists)
        {
            return new ValidationException($"Completed test with id={id} does not exist.");
        }

        return null;
    }

    private ValidationException? ValidateCompletedTestExercisesAsync(IEnumerable<AddCompletedTestExerciseDto> completedTestExercises,
        IEnumerable<ExerciseDto> topicExercises)
    {
        var hasExerciseIdDuplicates = CheckHasExerciseIdDuplicates(completedTestExercises, out var duplicateIds);
        if(hasExerciseIdDuplicates)
        {
            return new ValidationException($"CompletedTestExercises has ExerciseId duplicates: {string.Join(",", duplicateIds)}");
        }

        var completedTestExerciseIds = completedTestExercises.Select(e => e.ExerciseId);
        var topicExerciseIds = topicExercises.Select(e => e.Id);

        var allTopicExercisesCompleted = topicExerciseIds.All(id => completedTestExerciseIds.Contains(id));

        if (!allTopicExercisesCompleted)
        {
            return new ValidationException("Not all exercises from the topic are completed.");
        }

        var invalidExerciseIds = completedTestExerciseIds.Except(topicExerciseIds);
        if (invalidExerciseIds.Any())
        {
            return new ValidationException($"Completed test contains invalid exercise ids: {string.Join(",", invalidExerciseIds)}");
        }

        return null;
    }

    private bool CheckHasExerciseIdDuplicates(IEnumerable<AddCompletedTestExerciseDto> completedTestExercises, out IEnumerable<int> duplicateIds)
    {
        duplicateIds = completedTestExercises
            .GroupBy(e => e.ExerciseId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        return duplicateIds.Any();
    }

    private int CheckTestAnswersAsync(IEnumerable<AddCompletedTestExerciseDto> completedTestExercises)
    {
        var correctAnswersCount = 0;

        foreach (var completedTestExercise in completedTestExercises)
        {
            var answersMatch = completedTestExercise.Answer
                .Equals(completedTestExercise.UserAnswer, StringComparison.OrdinalIgnoreCase);

            if (answersMatch)
            {
                correctAnswersCount++;
            }
        }

        var percentage = correctAnswersCount * 100 / completedTestExercises.Count();
        return percentage;
    }
}