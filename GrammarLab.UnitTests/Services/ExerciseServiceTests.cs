using AutoMapper;
using FluentAssertions;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using GrammarLab.BLL.Services;
using GrammarLab.PL.Infrastructure.Mapping;
using Moq;

namespace GrammarLab.UnitTests.Services;

public class ExerciseServiceTests
{
    private Mock<IExerciseRepository> _exerciseRepositoryMock;
    private Mock<ITopicService> _topicServiceMock;
    private IMapper _mapper;
    private IExerciseService _exerciseService;

    public ExerciseServiceTests()
    {
        _exerciseRepositoryMock =  new Mock<IExerciseRepository>();
        _topicServiceMock = new Mock<ITopicService>();
        _mapper = CreateExerciseMapper();
        _exerciseService = new ExerciseService(_exerciseRepositoryMock.Object, _topicServiceMock.Object, _mapper);
    }

    private IMapper CreateExerciseMapper()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ExerciseMappingProfile()));
        return new Mapper(configuration);
    }

    [Fact]
    public async Task AddAsync_WhenValidExercise_ReturnsId()
    {
        // Arrange
        var newExercise = new AddExerciseDto()
        {
            TopicId = 1,
            Type = ExerciseType.Translation,
            Task = "Вона",
            Answer = "She",
        };

        var expectedId = 1;

        _topicServiceMock
            .Setup(r => r.ValidateTopicIdAsync(newExercise.TopicId))
            .ReturnsAsync(() => null);

        _exerciseRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Exercise>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _exerciseService.AddAsync(newExercise);
        var resultValue = result.Match(value => value, err => 0);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, resultValue);
    }

    [Fact]
    public async Task DeleteAsync_WhenValidId_ReturnsTrue()
    {
        //Arrange 
        var id = 1;

        _exerciseRepositoryMock
           .Setup(r => r.CheckExistsAsync(id))
           .ReturnsAsync(true);

        _exerciseRepositoryMock
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _exerciseService.DeleteAsync(id);
        var resultValue = result.Match(value => value, err => false);

        //Assert 
        Assert.True(result.IsSuccess);
        Assert.True(resultValue);
    }

    [Fact]
    public async Task DeleteAsync_WhenInvalidId_ReturnsError()
    {
        //Arrange 
        var id = 1;

        _exerciseRepositoryMock
            .Setup(r => r.CheckExistsAsync(id))
            .ReturnsAsync(false);

        // Act
        var result = await _exerciseService.DeleteAsync(id);
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        resultError.Should().NotBeNull();
        resultError.Message.Should().Be($"Exercise with id={id} does not exist.");
    }

    [Fact]
    public async Task UpdateAsync_WhenValidExercise_ReturnsTrue()
    {
        // Arrange
        var updatedExercise = new ExerciseDto()
        {
            Id = 1,
            TopicId = 1,
            Type = ExerciseType.Translation,
            Task = "Вона",
            Answer = "She",
        };

        _exerciseRepositoryMock
           .Setup(r => r.CheckExistsAsync(updatedExercise.Id))
           .ReturnsAsync(true);

        _topicServiceMock
            .Setup(r => r.ValidateTopicIdAsync(updatedExercise.TopicId))
            .ReturnsAsync(() => null);

        _exerciseRepositoryMock
           .Setup(r => r.UpdateAsync(It.IsAny<Exercise>()))
           .ReturnsAsync(true);

        // Act
        var result = await _exerciseService.UpdateAsync(updatedExercise);
        var resultValue = result.Match(value => value, err => false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(resultValue);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedExercise()
    {
        //Arrange 
        var id = 1;

        var exercise = new Exercise()
        {
            Id = id,
            TopicId = 1,
            Type = ExerciseType.Translation,
            Task = "Вона",
            Answer = "She",
        };

        var expectedExercise = new ExerciseDto()
        {
            Id = id,
            TopicId = 1,
            Type = ExerciseType.Translation,
            Task = "Вона",
            Answer = "She",
        };

        _exerciseRepositoryMock
           .Setup(r => r.GetByIdAsync(id))
           .ReturnsAsync(exercise);

        // Act
        var exerciseFromService = await _exerciseService.GetByIdAsync(id);

        //Assert 
        exerciseFromService.Should().NotBeNull();
        exerciseFromService.Should().BeEquivalentTo(expectedExercise);
    }

    [Fact]
    public async Task GetByTopiIdAsync_ReturnsMappedExercises()
    {
        //Arrange 
        var topicId = 1;
        var expectedExercises = new List<Exercise>
        {
           new ()
           {
                Id = 1,
                TopicId = 1,
                Type = ExerciseType.Translation,
                Task = "Вона",
                Answer = "She",
           },
           new ()
           {
                Id = 2,
                TopicId = 1,
                Type = ExerciseType.Translation,
                Task = "Він",
                Answer = "He",
           },
        };

        _exerciseRepositoryMock
           .Setup(r => r.GetByTopicIdAsync(topicId))
           .ReturnsAsync(expectedExercises);

        // Act
        var exercisesFromService = await _exerciseService.GetByTopicIdAsync(topicId);

        //Assert 
        exercisesFromService.Should().NotBeNull().And.HaveCount(2);
        exercisesFromService.FirstOrDefault().Task.Should().NotBeNull().And.BeSameAs(expectedExercises.FirstOrDefault().Task);
    }
}