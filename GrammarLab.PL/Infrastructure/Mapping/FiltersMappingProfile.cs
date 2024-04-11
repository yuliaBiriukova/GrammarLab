using AutoMapper;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure;

public class FiltersMappingProfile : Profile
{
    public FiltersMappingProfile()
    {
        CreateMap<TestResultFilterViewModel, TestResultFilter>();
        CreateMap<BaseFilterViewModel, BaseFilter>();
        CreateMap<UserFilterViewModel, UserFilter>();
    }
}