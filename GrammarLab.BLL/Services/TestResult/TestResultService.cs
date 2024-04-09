using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using LanguageExt.Common;
using System.ComponentModel.DataAnnotations;

namespace GrammarLab.BLL.Services;

public class TestResultService : ITestResultService
{
    private readonly ITestResultRepository _testResultRepository;
    private readonly ITopicService _topicService;
    private readonly IExerciseService _exerciseService;
    private readonly IMapper _mapper;

    public TestResultService(ITestResultRepository testResultRepository, ITopicService topicService, 
        IExerciseService exerciseService, IMapper mapper)
    {
        _testResultRepository = testResultRepository;
        _topicService = topicService;
        _exerciseService = exerciseService;
        _mapper = mapper;
    }

    public async Task<Result<int>> AddTestResultAsync(AddTestResultDto testResult, string userId)
    {
        var topicIdValidationError = await _topicService.ValidateTopicIdAsync(testResult.TopicId);
        if (topicIdValidationError != null)
        {
            return new Result<int>(topicIdValidationError);
        }

        var topicExercises = await _exerciseService.GetByTopicIdAsync(testResult.TopicId);

        var exercisesValidationError = ValidateTestResultExercisesAsync(testResult.TestResultExercises, topicExercises);
        if (exercisesValidationError != null)
        {
            return new Result<int>(exercisesValidationError);
        }

        foreach (var testResultExercise in testResult.TestResultExercises)
        {
            var topicExercise = topicExercises.FirstOrDefault(e => e.Id == testResultExercise.ExerciseId);
            if(topicExercise != null)
            {
                testResultExercise.Task = topicExercise.Task;
                testResultExercise.Answer = topicExercise.Answer;
            }
        }

        var newTestResult = _mapper.Map<TestResult>(testResult);
        newTestResult.UserId = userId;
        newTestResult.Percentage = CheckTestAnswersAsync(testResult.TestResultExercises);
        newTestResult.DateCompleted = DateTime.Now;

        var id = await _testResultRepository.AddAsync(newTestResult);
        return id;
    }

    public async Task<Result<bool>> DeleteTestResultAsync(int id)
    {
        var validationError = await ValidateTestResultIdAsync(id);
        if (validationError != null)
        {
            return new Result<bool>(validationError);
        }

        var isDeleted = await _testResultRepository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<TestResultDto?> GetByIdWithExercisesAsync(int id)
    {
        var testResult = await _testResultRepository.GetByIdWithExercisesAsync(id);

        var testResultDto = _mapper.Map<TestResultDto>(testResult);

        if (testResultDto?.TestResultExercises is not null)
        {
            foreach (var exercise in testResultDto.TestResultExercises) 
            {
                exercise.IsCorrect = string.Equals(exercise.Answer, exercise.UserAnswer);
            }
        }

        return testResultDto;
    }

    public async Task<IEnumerable<TestResultDto>> GetTestResultsAsync(TestResultFilter filter, string userId)
    {
        filter.UserId = userId;
        var testResults = await _testResultRepository.GetAsync(filter);
        return _mapper.Map<IEnumerable<TestResultDto>>(testResults);
    }

    public async Task<IEnumerable<TestResultDto>> GetLastTopicsTestResultsAsync(TestResultFilter filter, string userId)
    {
        filter.UserId = userId;
        var testResults = await _testResultRepository.GetLastTopicsResultsAsync(filter);
        return _mapper.Map<IEnumerable<TestResultDto>>(testResults);
    }

    public async Task<IEnumerable<TestResultDto>> GetBestTopicsTestResultsAsync(TestResultFilter filter, string userId)
    {
        filter.UserId = userId;
        var testResults = await _testResultRepository.GetBestTopicsResultsAsync(filter);
        return _mapper.Map<IEnumerable<TestResultDto>>(testResults);
    }

    private async Task<ValidationException?> ValidateTestResultIdAsync(int id)
    {
        var testResultExists = await _testResultRepository.CheckExistsAsync(id);
        if (!testResultExists)
        {
            return new ValidationException($"Completed test with id={id} does not exist.");
        }

        return null;
    }

    private ValidationException? ValidateTestResultExercisesAsync(IEnumerable<AddTestResultExerciseDto> testResultExercises,
        IEnumerable<ExerciseDto> topicExercises)
    {
        var hasExerciseIdDuplicates = CheckHasExerciseIdDuplicates(testResultExercises, out var duplicateIds);
        if(hasExerciseIdDuplicates)
        {
            return new ValidationException($"TestResultExercises has ExerciseId duplicates: {string.Join(",", duplicateIds)}");
        }

        var testResultExerciseIds = testResultExercises.Select(e => e.ExerciseId);
        var topicExerciseIds = topicExercises.Select(e => e.Id);

        var allTopicExercisesCompleted = topicExerciseIds.All(id => testResultExerciseIds.Contains(id));

        if (!allTopicExercisesCompleted)
        {
            return new ValidationException("Not all exercises from the topic are completed.");
        }

        var invalidExerciseIds = testResultExerciseIds.Except(topicExerciseIds);
        if (invalidExerciseIds.Any())
        {
            return new ValidationException($"Test result contains invalid exercise ids: {string.Join(",", invalidExerciseIds)}");
        }

        return null;
    }

    private bool CheckHasExerciseIdDuplicates(IEnumerable<AddTestResultExerciseDto> testResultExercises, out IEnumerable<int> duplicateIds)
    {
        duplicateIds = testResultExercises
            .GroupBy(e => e.ExerciseId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        return duplicateIds.Any();
    }

    private int CheckTestAnswersAsync(IEnumerable<AddTestResultExerciseDto> testResultExercises)
    {
        var correctAnswersCount = 0;

        foreach (var testResultExercise in testResultExercises)
        {
            var answersMatch = testResultExercise.Answer
                .Equals(testResultExercise.UserAnswer, StringComparison.OrdinalIgnoreCase);

            if (answersMatch)
            {
                correctAnswersCount++;
            }
        }

        var percentage = correctAnswersCount * 100 / testResultExercises.Count();
        return percentage;
    }
}