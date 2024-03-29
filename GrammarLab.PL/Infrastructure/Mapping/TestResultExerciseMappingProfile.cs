using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class TestResultExerciseMappingProfile : Profile
{
    public TestResultExerciseMappingProfile()
    {
        CreateMap<AddTestResultExerciseDto, TestResultExercise>();
        CreateMap<TestResultExercise, TestResultExerciseDto>();
        CreateMap<AddTestResultExerciseViewModel, AddTestResultExerciseDto>();
        CreateMap<TestResultExerciseDto, TestResultExerciseViewModel>();
    }
}