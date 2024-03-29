using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class TestResultMappingProfile : Profile
{
    public TestResultMappingProfile()
    {
        CreateMap<AddTestResultDto, TestResult>();
        CreateMap<TestResult, TestResultDto>();
        CreateMap<AddTestResultViewModel, AddTestResultDto>();
        CreateMap<TestResultDto, TestResultViewModel>();
    }
}