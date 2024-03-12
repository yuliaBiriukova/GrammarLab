using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class CompletedTestExerciseMappingProfile : Profile
{
    public CompletedTestExerciseMappingProfile()
    {
        CreateMap<AddCompletedTestExerciseDto, CompletedTestExercise>();
        CreateMap<CompletedTestExercise, CompletedTestExerciseDto>();
        CreateMap<AddCompletedTestExerciseViewModel, AddCompletedTestExerciseDto>();
        CreateMap<CompletedTestExerciseDto, CompletedTestExerciseViewModel>();
    }
}