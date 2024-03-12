using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class ExerciseMappingProfile : Profile
{
    public ExerciseMappingProfile()
    {
        CreateMap<ExerciseDto, Exercise>().ReverseMap();
        CreateMap<AddExerciseDto, Exercise>();
        CreateMap<AddExerciseViewModel, AddExerciseDto>();
        CreateMap<ExerciseViewModel, ExerciseDto>().ReverseMap();
    }
}