using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class LevelMappingProfile : Profile
{
    public LevelMappingProfile()
    {
        CreateMap<Level, LevelDto>().ReverseMap();
        CreateMap<AddLevelDto, Level>();
        CreateMap<AddLevelViewModel, AddLevelDto>();
        CreateMap<UpdateLevelViewModel, LevelDto>();
        CreateMap<LevelDto, LevelViewModel>().ReverseMap();
    }
}