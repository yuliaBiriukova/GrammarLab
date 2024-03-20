using AutoMapper;
using FluentAssertions;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.BLL.Repositories;
using GrammarLab.BLL.Services;
using GrammarLab.PL.Infrastructure.Mapping;
using Moq;

namespace GrammarLab.UnitTests.Services;

public class TopicServiceTests
{
    private Mock<ITopicRepository> _topicRepositoryMock;
    private Mock<ILevelService> _levelServiceMock;
    private IMapper _mapper;
    private ITopicService _topicService;

    public TopicServiceTests()
    {
        _topicRepositoryMock = new Mock<ITopicRepository>();
        _levelServiceMock = new Mock<ILevelService>();
        _mapper = CreateTopicMapper();
        _topicService = new TopicService(_topicRepositoryMock.Object, _levelServiceMock.Object, _mapper);
    }

    private IMapper CreateTopicMapper()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new TopicMappingProfile()));
        return new Mapper(configuration);
    }

    [Fact]
    public async Task AddAsync_WhenValidTopic_ReturnsId()
    {
        // Arrange
        var newTopic = new AddTopicDto()
        {
            Name = "Present Simple",
            Content = "Present Simple rules",
            LevelId = 1,
        };

        var expectedId = 1;

        _levelServiceMock
            .Setup(r => r.ValidateLevelIdAsync(newTopic.LevelId))
            .ReturnsAsync(() => null);

        _topicRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Topic>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _topicService.AddAsync(newTopic);
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

        _topicRepositoryMock
           .Setup(r => r.CheckExistsAsync(id))
           .ReturnsAsync(true);

        _topicRepositoryMock
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _topicService.DeleteAsync(id);
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

        _topicRepositoryMock
           .Setup(r => r.CheckExistsAsync(id))
           .ReturnsAsync(false);

        // Act
        var result = await _topicService.DeleteAsync(id);
        var resultError = result.Match(value => null, err => err);

        // Assert
        Assert.True(result.IsFaulted);
        resultError.Should().NotBeNull();
        resultError.Message.Should().Be($"Topic with id={id} does not exist.");
    }

    [Fact]
    public async Task UpdateAsync_WhenValidTopic_ReturnsTrue()
    {
        // Arrange
        var updatedTopic = new TopicDto()
        {
            Id = 1,
            Name = "Present Simple",
            Content = "Present Simple rules Updated",
            LevelId = 1,
        };

        _topicRepositoryMock
           .Setup(r => r.CheckExistsAsync(updatedTopic.Id))
           .ReturnsAsync(true);

        _levelServiceMock
            .Setup(r => r.ValidateLevelIdAsync(updatedTopic.LevelId))
            .ReturnsAsync(() => null);

        _topicRepositoryMock
           .Setup(r => r.UpdateAsync(It.IsAny<Topic>()))
           .ReturnsAsync(true);

        // Act
        var result = await _topicService.UpdateAsync(updatedTopic);
        var resultValue = result.Match(value => value, err => false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(resultValue);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedTopic()
    {
        //Arrange 
        var id = 1;
        var topic = new Topic()
        {
            Id = id,
            Name = "Present Simple",
            Content = "Present Simple rules Updated",
            LevelId = 1,
        };

        var expectedTopic = new TopicDto()
        {
            Id = id,
            Name = "Present Simple",
            Content = "Present Simple rules Updated",
            LevelId = 1,
        };

        _topicRepositoryMock
           .Setup(r => r.GetByIdAsync(id))
           .ReturnsAsync(topic);

        // Act
        var topicFromService = await _topicService.GetByIdAsync(id);

        //Assert 
        topicFromService.Should().NotBeNull();
        topicFromService.Should().BeEquivalentTo(expectedTopic);
    }

    [Fact]
    public async Task GetByLevelIdAsync_ReturnsMappedTopics()
    {
        //Arrange 
        var levelId = 1;
        var expectedTopics = new List<Topic>
        {
           new () 
           {
                Id = 1,
                Name = "Present Simple",
                Content = "Present Simple rules",
                LevelId = levelId,
           },
           new ()
           {
                Id = 1,
                Name = "Present Perfect",
                Content = "Present Perfect rules",
                LevelId = levelId,
           },
        };

        _topicRepositoryMock
           .Setup(r => r.GetByLevelIdAsync(levelId))
           .ReturnsAsync(expectedTopics);

        // Act
        var topicsFromService = await _topicService.GetByLevelIdAsync(levelId);

        //Assert 
        topicsFromService.Should().NotBeNull().And.HaveCount(2);
        topicsFromService.FirstOrDefault().Name.Should().NotBeNull().And.BeSameAs(expectedTopics.FirstOrDefault().Name);
    }
}