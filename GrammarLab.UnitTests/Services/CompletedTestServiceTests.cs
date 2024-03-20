using AutoMapper;
using FluentAssertions;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using GrammarLab.BLL.Services;
using GrammarLab.PL.Infrastructure.Mapping;
using Moq;

namespace GrammarLab.UnitTests.Services;

public class CompletedTestServiceTests
{
    private readonly Mock<ICompletedTestRepository> _completedTestRepositoryMock;
    private readonly Mock<ITopicService> _topicServiceMock;
    private readonly Mock<IExerciseService> _exerciseServiceMock;
    private readonly IMapper _mapper;
    private readonly ICompletedTestService _completedTestService;

    public CompletedTestServiceTests()
    {
        _completedTestRepositoryMock = new Mock<ICompletedTestRepository>();
        _topicServiceMock = new Mock<ITopicService>();
        _exerciseServiceMock = new Mock<IExerciseService>();
        _mapper = CreateCompletedTestsMapper();
        _completedTestService = new CompletedTestService(
            _completedTestRepositoryMock.Object,
            _topicServiceMock.Object,
            _exerciseServiceMock.Object,
            _mapper);
    }

    private IMapper CreateCompletedTestsMapper()
    {
        var configuration = new MapperConfiguration(cfg => {
            cfg.AddProfile(new CompletedTestMappingProfile());
            cfg.AddProfile(new CompletedTestExerciseMappingProfile());
        }); 
        return new Mapper(configuration);
    }

    [Fact]
    public async Task AddCompletedTestAsync_WithValidData_ReturnsId()
    {
        // Arrange
        var newCompletedTest = new AddCompletedTestDto
        {
            TopicId = 1,
            CompletedTestExercises = new List<AddCompletedTestExerciseDto>
            {
                new () { ExerciseId = 1, UserAnswer = "CorrectAnswer1" },
                new () { ExerciseId = 2, UserAnswer = "CorrectAnswer2" },
            }
        };

        var expectedId = 1;

        _topicServiceMock
            .Setup(x => x.ValidateTopicIdAsync(newCompletedTest.TopicId))
            .ReturnsAsync(() => null);

        _exerciseServiceMock
            .Setup(x => x.GetByTopicIdAsync(newCompletedTest.TopicId))
            .ReturnsAsync(new List<ExerciseDto>
            {
               new () { Id = 1, Task = "Task1", Answer = "CorrectAnswer1" },
               new () { Id = 2, Task = "Task2", Answer = "CorrectAnswer2" },
            });

        _completedTestRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<CompletedTest>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _completedTestService.AddCompletedtTestAsync(newCompletedTest, "userId");
        var resultValue = result.Match(value => value, err => 0);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, resultValue);
    }

    [Fact]
    public async Task AddCompletedTestAsync_WithMissingExercises_ReturnsError()
    {
        // Arrange
        var newCompletedTest = new AddCompletedTestDto
        {
            TopicId = 1,
            CompletedTestExercises = new List<AddCompletedTestExerciseDto>
            {
                new () { ExerciseId = 1, UserAnswer = "CorrectAnswer1" },
            }
        };

        _topicServiceMock
            .Setup(x => x.ValidateTopicIdAsync(newCompletedTest.TopicId))
            .ReturnsAsync(() => null);

        _exerciseServiceMock
            .Setup(x => x.GetByTopicIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<ExerciseDto>
            {
               new () { Id = 1, Task = "Task1", Answer = "CorrectAnswer1" },
               new () { Id = 2, Task = "Task2", Answer = "CorrectAnswer2" },
            });

        // Act
        var result = await _completedTestService.AddCompletedtTestAsync(newCompletedTest, "userId");
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        Assert.Equal("Not all exercises from the topic are completed.", resultError.Message);
    }

    [Fact]
    public async Task DeleteCompletedTestAsync_WhenValidId_ReturnsTrue()
    {
        //Arrange 
        var id = 1;

        _completedTestRepositoryMock
           .Setup(r => r.CheckExistsAsync(id))
           .ReturnsAsync(true);

        _completedTestRepositoryMock
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _completedTestService.DeleteCompletedTestAsync(id);
        var resultValue = result.Match(value => value, err => false);

        //Assert 
        Assert.True(result.IsSuccess);
        Assert.True(resultValue);
    }

    [Fact]
    public async Task GetByIdWithExercisesAsync_ReturnsMappedCompletedTestWithExercises()
    {
        //Arrange 
        var id = 1;

        var completedTest = new CompletedTest()
        {
            Id = id,
            CompletedTestExercises = new List<CompletedTestExercise>()
            {
                new() { Id = 1 },
                new() { Id = 2 },
            }
        };

        var expectedCompletedTest = new CompletedTestDto()
        {
            Id = id,
            CompletedTestExercises = new List<CompletedTestExerciseDto>()
            {
                new() { Id = 1 },
                new() { Id = 2 },
            }
        };

        _completedTestRepositoryMock
           .Setup(r => r.GetByIdWithExercisesAsync(id))
           .ReturnsAsync(completedTest);

        // Act
        var completedTestFromService = await _completedTestService.GetByIdWithExercisesAsync(id);

        //Assert 
        completedTestFromService.Should().NotBeNull();
        completedTestFromService.Should().BeEquivalentTo(expectedCompletedTest);
    }
}