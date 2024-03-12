using AutoMapper;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure;

public class FiltersMappingProfile : Profile
{
    public FiltersMappingProfile()
    {
        CreateMap<CompletedTestFilterViewModel, CompletedTestFilter>();
        CreateMap<BaseFilterViewModel, BaseFilter>();
    }
}