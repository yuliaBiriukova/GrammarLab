using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class CompletedTestMappingProfile : Profile
{
    public CompletedTestMappingProfile()
    {
        CreateMap<AddCompletedTestDto, CompletedTest>();
        CreateMap<CompletedTest, CompletedTestDto>();
        CreateMap<AddCompletedTestViewModel, AddCompletedTestDto>();
        CreateMap<CompletedTestDto, CompletedTestViewModel>();
    }
}