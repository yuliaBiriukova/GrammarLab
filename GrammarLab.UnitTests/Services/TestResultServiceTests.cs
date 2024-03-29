using AutoMapper;
using FluentAssertions;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using GrammarLab.BLL.Services;
using GrammarLab.PL.Infrastructure.Mapping;
using Moq;

namespace GrammarLab.UnitTests.Services;

public class TestResultServiceTests
{
    private readonly Mock<ITestResultRepository> _testResultRepositoryMock;
    private readonly Mock<ITopicService> _topicServiceMock;
    private readonly Mock<IExerciseService> _exerciseServiceMock;
    private readonly IMapper _mapper;
    private readonly ITestResultService _testResultService;

    public TestResultServiceTests()
    {
        _testResultRepositoryMock = new Mock<ITestResultRepository>();
        _topicServiceMock = new Mock<ITopicService>();
        _exerciseServiceMock = new Mock<IExerciseService>();
        _mapper = CreateTestResultMapper();
        _testResultService = new TestResultService(
            _testResultRepositoryMock.Object,
            _topicServiceMock.Object,
            _exerciseServiceMock.Object,
            _mapper);
    }

    private IMapper CreateTestResultMapper()
    {
        var configuration = new MapperConfiguration(cfg => {
            cfg.AddProfile(new TestResultMappingProfile());
            cfg.AddProfile(new TestResultExerciseMappingProfile());
        }); 
        return new Mapper(configuration);
    }

    [Fact]
    public async Task AddTestResultAsync_WithValidData_ReturnsId()
    {
        // Arrange
        var newTestResult = new AddTestResultDto
        {
            TopicId = 1,
            TestResultExercises = new List<AddTestResultExerciseDto>
            {
                new () { ExerciseId = 1, UserAnswer = "CorrectAnswer1" },
                new () { ExerciseId = 2, UserAnswer = "CorrectAnswer2" },
            }
        };

        var expectedId = 1;

        _topicServiceMock
            .Setup(x => x.ValidateTopicIdAsync(newTestResult.TopicId))
            .ReturnsAsync(() => null);

        _exerciseServiceMock
            .Setup(x => x.GetByTopicIdAsync(newTestResult.TopicId))
            .ReturnsAsync(new List<ExerciseDto>
            {
               new () { Id = 1, Task = "Task1", Answer = "CorrectAnswer1" },
               new () { Id = 2, Task = "Task2", Answer = "CorrectAnswer2" },
            });

        _testResultRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TestResult>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _testResultService.AddTestResultAsync(newTestResult, "userId");
        var resultValue = result.Match(value => value, err => 0);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, resultValue);
    }

    [Fact]
    public async Task AddTestResultAsync_WithMissingExercises_ReturnsError()
    {
        // Arrange
        var newTestResult = new AddTestResultDto
        {
            TopicId = 1,
            TestResultExercises = new List<AddTestResultExerciseDto>
            {
                new () { ExerciseId = 1, UserAnswer = "CorrectAnswer1" },
            }
        };

        _topicServiceMock
            .Setup(x => x.ValidateTopicIdAsync(newTestResult.TopicId))
            .ReturnsAsync(() => null);

        _exerciseServiceMock
            .Setup(x => x.GetByTopicIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<ExerciseDto>
            {
               new () { Id = 1, Task = "Task1", Answer = "CorrectAnswer1" },
               new () { Id = 2, Task = "Task2", Answer = "CorrectAnswer2" },
            });

        // Act
        var result = await _testResultService.AddTestResultAsync(newTestResult, "userId");
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        Assert.Equal("Not all exercises from the topic are completed.", resultError.Message);
    }

    [Fact]
    public async Task DeleteTestResultAsync_WhenValidId_ReturnsTrue()
    {
        //Arrange 
        var id = 1;

        _testResultRepositoryMock
           .Setup(r => r.CheckExistsAsync(id))
           .ReturnsAsync(true);

        _testResultRepositoryMock
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _testResultService.DeleteTestResultAsync(id);
        var resultValue = result.Match(value => value, err => false);

        //Assert 
        Assert.True(result.IsSuccess);
        Assert.True(resultValue);
    }

    [Fact]
    public async Task GetByIdWithExercisesAsync_ReturnsMappedTestResultWithExercises()
    {
        //Arrange 
        var id = 1;

        var testResult = new TestResult()
        {
            Id = id,
            TestResultExercises = new List<TestResultExercise>()
            {
                new() { Id = 1 },
                new() { Id = 2 },
            }
        };

        var expectedTestResult = new TestResultDto()
        {
            Id = id,
            TestResultExercises = new List<TestResultExerciseDto>()
            {
                new() { Id = 1 },
                new() { Id = 2 },
            }
        };

        _testResultRepositoryMock
           .Setup(r => r.GetByIdWithExercisesAsync(id))
           .ReturnsAsync(testResult);

        // Act
        var testResultFromService = await _testResultService.GetByIdWithExercisesAsync(id);

        //Assert 
        testResultFromService.Should().NotBeNull();
        testResultFromService.Should().BeEquivalentTo(expectedTestResult);
    }
}