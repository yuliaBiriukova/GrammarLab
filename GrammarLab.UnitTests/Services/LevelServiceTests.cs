using AutoMapper;
using FluentAssertions;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using GrammarLab.BLL.Services;
using GrammarLab.PL.Infrastructure.Mapping;
using Moq;

namespace GrammarLab.UnitTests.Services;

public class LevelServiceTests
{
    private Mock<ILevelRepository> _levelRepositoryMock;
    private IMapper _mapper;
    private ILevelService _levelService;

    public LevelServiceTests()
    {
        _levelRepositoryMock = new Mock<ILevelRepository>();
        _mapper = CreateLevelMapper();
        _levelService = new LevelService(_levelRepositoryMock.Object, _mapper);
    }

    private IMapper CreateLevelMapper()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new LevelMappingProfile()));
        return new Mapper(configuration);
    }

    [Fact]
    public async Task AddAsync_WhenValidLevel_ReturnsId()
    {
        // Arrange
        var newLevel = new AddLevelDto()
        {
            Code = "A1",
            Name = "Beginner",
        }; 

        var expectedId = 1;

        _levelRepositoryMock
            .Setup(r => r.CheckIsCodeUniqueAsync(newLevel.Code))
            .ReturnsAsync(true);

        _levelRepositoryMock
            .Setup(r => r.CheckIsNameUniqueAsync(newLevel.Name))
            .ReturnsAsync(true);

        _levelRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Level>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _levelService.AddLevelAsync(newLevel);
        var resultValue = result.Match(value => value, err => 0);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedId, resultValue);
    }

    [Fact]
    public async Task AddAsync_WhenInvalidLevelCode_ReturnsError()
    {
        // Arrange
        var newLevel = new AddLevelDto()
        {
            Code = "A1",
            Name = "Beginner",
        };

        _levelRepositoryMock
            .Setup(r => r.CheckIsCodeUniqueAsync(newLevel.Code))
            .ReturnsAsync(false);

        // Act
        var result = await _levelService.AddLevelAsync(newLevel);
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        resultError.Should().NotBeNull();
        resultError.Message.Should().Be($"Level with code {newLevel.Code} exists.");
    }

    [Fact]
    public async Task AddAsync_WhenInvalidLevelName_ReturnsError()
    {
        // Arrange
        var newLevel = new AddLevelDto()
        {
            Code = "A1",
            Name = "Beginner",
        };

        _levelRepositoryMock
            .Setup(r => r.CheckIsCodeUniqueAsync(newLevel.Code))
            .ReturnsAsync(true);

        _levelRepositoryMock
            .Setup(r => r.CheckIsNameUniqueAsync(newLevel.Code))
            .ReturnsAsync(false);

        // Act
        var result = await _levelService.AddLevelAsync(newLevel);
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        resultError.Should().NotBeNull();
        resultError.Message.Should().Be($"Level with name {newLevel.Name} exists.");
    }

    [Fact]
    public async Task UpdateLevelAsync_WhenValidLevel_ReturnsTrue()
    {
        // Arrange
        var updatedLevel = new LevelDto()
        {
            Id = 1,
            Code = "A1",
            Name = "Updated",
        };

        var existingLevel = new Level()
        {
            Id = 1,
            Code = "A1",
            Name = "Beginner",
        };

        _levelRepositoryMock
            .Setup(r => r.GetByIdAsync(updatedLevel.Id))
            .ReturnsAsync(existingLevel);

        _levelRepositoryMock
            .Setup(r => r.CheckIsCodeUniqueAsync(updatedLevel.Code))
            .ReturnsAsync(true);

        _levelRepositoryMock
            .Setup(r => r.CheckIsNameUniqueAsync(updatedLevel.Name))
            .ReturnsAsync(true);

        _levelRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Level>()))
            .ReturnsAsync(true);

        // Act
        var result = await _levelService.UpdateLevelAsync(updatedLevel);
        var resultValue = result.Match(value => value, err => false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(resultValue);
    }

    [Fact]
    public async Task UpdateLevelAsync_WhenInvalidLevelId_ReturnsError()
    {
        // Arrange
        var updatedLevel = new LevelDto()
        {
            Id = 2,
            Code = "A1",
            Name = "Updated",
        };

        _levelRepositoryMock
            .Setup(r => r.GetByIdAsync(updatedLevel.Id))
            .ReturnsAsync(() => null);
        
        // Act
        var result = await _levelService.UpdateLevelAsync(updatedLevel);
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        resultError.Should().NotBeNull();
        resultError.Message.Should().Be($"Level with id={updatedLevel.Id} does not exist.");
    }

    [Fact]
    public async Task UpdateLevelAsync_WhenInvalidLevelName_ReturnsError()
    {
        // Arrange
        var updatedLevel = new LevelDto()
        {
            Id = 1,
            Code = "A1",
            Name = "ExistingName",
        };

        var existingLevel = new Level()
        {
            Id = 1,
            Code = "A1",
            Name = "Beginner",
        };

        _levelRepositoryMock
            .Setup(r => r.GetByIdAsync(updatedLevel.Id))
            .ReturnsAsync(existingLevel);

        _levelRepositoryMock
            .Setup(r => r.CheckIsNameUniqueAsync(updatedLevel.Name))
            .ReturnsAsync(false);

        // Act
        var result = await _levelService.UpdateLevelAsync(updatedLevel);
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        resultError.Should().NotBeNull();
        resultError.Message.Should().Be($"Level with name {updatedLevel.Name} exists.");
    }

    [Fact]
    public async Task DeleteLevelAsync_WhenValidId_ReturnsTrue()
    {
        //Arrange 
        var id = 1;

        _levelRepositoryMock
           .Setup(r => r.CheckLevelExistsAsync(id))
           .ReturnsAsync(true);

        _levelRepositoryMock
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _levelService.DeleteLevelAsync(id);
        var resultValue = result.Match(value => value, err => false);

        //Assert 
        Assert.True(result.IsSuccess);
        Assert.True(resultValue);
    }

    [Fact]
    public async Task DeleteLevelAsync_WhenInvalidId_ReturnsError()
    {
        //Arrange 
        var id = 1;

        _levelRepositoryMock
           .Setup(r => r.CheckLevelExistsAsync(id))
           .ReturnsAsync(false);

        // Act
        var result = await _levelService.DeleteLevelAsync(id);
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        resultError.Should().NotBeNull();
        resultError.Message.Should().Be($"Level with id={id} does not exist.");
    }

    [Fact]
    public async Task GetAllLevelsAsync_ReturnsMappedLevels()
    {
        //Arrange 
        var expectedLevels = new List<Level>
           {
               new () { Id = 1, Code = "A1", Name = "Beginner" },
               new () { Id = 2, Code = "A2", Name = "Pre-Intermediate"},
           };

        _levelRepositoryMock
           .Setup(r => r.GetAllAsync())
           .ReturnsAsync(expectedLevels);

        // Act
        var levelsFromService = await _levelService.GetAllLevelsAsync();

        //Assert 
        levelsFromService.Should().NotBeNull().And.HaveCount(2);
        levelsFromService.FirstOrDefault().Name.Should().NotBeNull().And.BeSameAs(expectedLevels.FirstOrDefault().Name);
    }

    [Fact]
    public async Task GetLevelByIdAsync_ReturnsMappedLevel()
    {
        //Arrange 
        var id = 1;
        var level = new Level() { Id = id, Code = "A1", Name = "Beginner" };
        var expectedLevel = new LevelDto() { Id = id, Code = "A1", Name = "Beginner", Topics = new List<TopicDto>() };

        _levelRepositoryMock
           .Setup(r => r.GetByIdAsync(id))
           .ReturnsAsync(level);

        // Act
        var levelFromService = await _levelService.GetLevelByIdAsync(id);

        //Assert 
        levelFromService.Should().NotBeNull();
        levelFromService.Should().BeEquivalentTo(expectedLevel);
    }
}