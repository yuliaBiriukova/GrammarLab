using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class TopicMappingProfile : Profile
{
    public TopicMappingProfile()
    {
        CreateMap<Topic, TopicDto>().ReverseMap();
        CreateMap<AddTopicDto, Topic>();
        CreateMap<AddTopicViewModel, AddTopicDto>();
        CreateMap<TopicDto, TopicViewModel>().ReverseMap();
    }
}
